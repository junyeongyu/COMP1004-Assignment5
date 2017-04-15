/**
* The App name: Slot Machine
* Author's name: Junyeong Yu (200328206)
* App Creation Date: April 10, 2017
* App Last Modification Date: April 14, 2017
* App Short Revision History
*  -           April 10, 2017: Initial Commit
*  -           April 11, 2017: Implement minor functions and controls
*  -           April 13, 2017: Implement Functionality of enable and disable spin buttons
*  - 03:40 PM, April 14, 2017: Betting and jackpot functionalities
*  - 09:20 pM, April 14, 2017: Adding furuits functions
*  - 10:00 PM, April 14, 2017: Adding audio functions & fixing bugs
*  - 11:00 PM, April 14, 2017: Add all necessary comments
* App description: Play the slot maching using buttons of "Spin", "Reset", "Bet" and "Quit".
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlotMachine : MonoBehaviour {

	// basic slot machine variables
	private int _playerMoney = 1000;
	private int _winnings = 0;
	private int _jackpot = 5000;
	private float _turn = 0.0f;
	private int _playerBet = 0;

	private float _winNumber = 0.0f;
	private float _lossNumber = 0.0f;
	private int[] _spinResult = new int[] {7, 7, 7};
	private string[] _fruits = {"blank", "grapes", "banana", "orange", "cherry", "bar", "bell", "seven"};
	private float _winRatio = 0.0f;
	private float _lossRatio = 0.0f;
	private int _grapes = 0;
	private int _bananas = 0;
	private int _oranges = 0;
	private int _cherries = 0;
	private int _bars = 0;
	private int _bells = 0;
	private int _sevens = 0;
	private int _blanks = 0;

	// indexes of fruits array
	private int _BLANK = 0;
	private int _GRAPES = 1;
	private int _BANANA = 2;
	private int _ORANGE = 3;
	private int _CHERRY = 4;
	private int _BAR = 5;
	private int _BELL = 6;
	private int _SEVEN = 7;

	// make compenent as invisible
	private const float _OUT_OF_SCREEN = 4000.0f;

	// for checking status
	private bool _isSpinned = false;
	private bool _isJackpot = false;

	// location information
	private const float _defaultPositionYDisabledButton = 128.0f;
	private const float _defaultPositionXJackpotWinImage = 0.0f;
	private const float _defaultPositionYFruitsImage = -0.33f;

	// gameobject to approace necessary function and value
	private GameObject _thankYouImage;
	private GameObject _playerMoneyText;
	private GameObject _playerBetText;
	private GameObject _winningsText;
	private GameObject _jackpotText;
	private GameObject _spinDisabledButton;
	private GameObject _jackpotWinImage;
	private GameObject _jackpotWinText;
	private GameObject[] _fruitOnes;
	private GameObject[] _fruitTwos;
	private GameObject[] _fruitThrees;
	private GameObject _spinButton;
	private GameObject _resetButton;
	private GameObject _quitButton;

	// Use this for initialization
	void Start () {
		_thankYouImage = GameObject.Find("thankYouImage");
		_playerMoneyText = GameObject.Find("playerMoneyText");
		_playerBetText = GameObject.Find("playerBetText");
		_winningsText = GameObject.Find("winningsText");
		_jackpotText = GameObject.Find("jackpotText");
		_spinDisabledButton = GameObject.Find("spinDisabledButton");
		_jackpotWinImage = GameObject.Find("jackpotWinImage");
		_jackpotWinText = GameObject.Find("jackpotWinText");
		_fruitOnes = new GameObject[_fruits.Length];
		_fruitTwos = new GameObject[_fruits.Length];
		_fruitThrees = new GameObject[_fruits.Length];
		for (int i = 0; i < _fruits.Length; i++) { // Load all fruits game object
			_fruitOnes[i] = GameObject.Find(_fruits[i] + "One");
			_fruitTwos[i] = GameObject.Find(_fruits[i] + "Two");
			_fruitThrees[i] = GameObject.Find(_fruits[i] + "Three");
		}
		_spinButton = GameObject.Find ("spinButton");
		_resetButton = GameObject.Find ("resetButton");
		_quitButton = GameObject.Find ("quitButton");
		_resetAll ();
	}

	// refresh as an initial status
	private void _refresh () {
		// When user doet not have enough money to bet, spin button is disabled
		if (_playerMoney == 0 || _playerBet > _playerMoney || _playerBet < 0) {
			_setActiveDisabledButton(_spinDisabledButton, true); // need to show disabled button
		} else {
			_setActiveDisabledButton(_spinDisabledButton, false); // hide disabled button
		}

		// When user wins jackpot, user can see message.
		_setActiveJackpotWinImage (_isJackpot);
		_isJackpot = false; // after showing jackpot, it needs to be hidden. 

		// Setting data into each UI text.
		_setText (_playerMoneyText, _playerMoney.ToString());
		_setText (_jackpotText, _jackpot.ToString ());
		_setText (_playerBetText, _playerBet.ToString ());
		_setText (_winningsText, _winnings.ToString ());

		// Selecting proper the fruits images that slot machine chooses
		for (int i = 0; i < _fruits.Length; i++) {
			_setActiveFruitsImage (_fruitOnes [i], _spinResult[0] == i);
			_setActiveFruitsImage (_fruitTwos [i], _spinResult[1] == i);
			_setActiveFruitsImage (_fruitThrees [i], _spinResult[2] == i);
		}
	}
	// It functions as if components are visible or invisible for fruits images
	private void _setActiveFruitsImage(GameObject gameObject, bool isActive) {
		if (isActive) {
			gameObject.transform.position = new Vector2 (gameObject.transform.position.x, _defaultPositionYFruitsImage);
		} else {
			gameObject.transform.position = new Vector2 (gameObject.transform.position.x, _OUT_OF_SCREEN);
		}
	}
	// It functions as if components are visible or invisible for disabled button
	private void _setActiveDisabledButton(GameObject gameObject, bool isActive) {
		if (isActive) {
			gameObject.transform.position = new Vector2 (gameObject.transform.position.x, _defaultPositionYDisabledButton);
		} else {
			gameObject.transform.position = new Vector2 (gameObject.transform.position.x, _OUT_OF_SCREEN);
		}
	}
	// It functions as if components are visible or invisible for jackpot winning message
	private void _setActiveJackpotWinImage(bool isActive) {
		GameObject gameObject = _jackpotWinImage;
		if (isActive) {
			gameObject.transform.position = new Vector2 (_defaultPositionXJackpotWinImage, gameObject.transform.position.y);
		} else {
			gameObject.transform.position = new Vector2 (_OUT_OF_SCREEN, gameObject.transform.position.y);
		}
	}
	// for setting text for general purpose
	private void _setText(GameObject gameObject, string text) {
		gameObject.GetComponent<Text>().text = text;
	}

	/* Utility function to show Player Stats */
	private void _showPlayerStats()
	{
		_winRatio = _winNumber / _turn;
		_lossRatio = _lossNumber / _turn;
		string stats = "";
		stats += ("Jackpot: " + _jackpot + "\n");
		stats += ("Player Money: " + _playerMoney + "\n");
		stats += ("Turn: " + _turn + "\n");
		stats += ("Wins: " + _winNumber + "\n");
		stats += ("Losses: " + _lossNumber + "\n");
		stats += ("Win Ratio: " + (_winRatio * 100) + "%\n");
		stats += ("Loss Ratio: " + (_lossRatio * 100) + "%\n");
		Debug.Log(stats);
	}

	/* Utility function to reset all fruit tallies*/
	private void _resetFruitTally()
	{
		_grapes = 0;
		_bananas = 0;
		_oranges = 0;
		_cherries = 0;
		_bars = 0;
		_bells = 0;
		_sevens = 0;
		_blanks = 0;
	}

	/* Utility function to reset the player stats */
	private void _resetAll()
	{
		_playerMoney = 1000;
		_winnings = 0;
		_jackpot = 5000;
		_turn = 0;
		_playerBet = 10; // default bet amount
		_winNumber = 0;
		_lossNumber = 0;
		_winRatio = 0.0f;
		_spinResult = new int[] {7, 7, 7}; // default showing images are 7,7,7

		_refresh ();
	}

	/* Check to see if the player won the jackpot */
	private void _checkJackPot()
	{
		/* compare two random values */
		var jackPotTry = Random.Range (1, 31);
		var jackPotWin = Random.Range (1, 31);
		if (jackPotTry == jackPotWin)
		{
			Debug.Log("You Won the $" + _jackpot + " Jackpot!!");
			_setText (_jackpotWinText, "You Won the $" + _jackpot + " Jackpot!!");
			_isJackpot = true; // it can be used in _refresh() function
			_playerMoney += _jackpot;
			_jackpot = 300;
		}
	}

	/* Utility function to show a win message and increase player money */
	private void _showWinMessage()
	{
		_playerMoney += _winnings;
		Debug.Log("You Won: $" + _winnings);
		gameObject.GetComponent<AudioSource>().Play (); // winning sound
		_resetFruitTally();
		_checkJackPot(); // when user win, user get probability to get jackpot
	}

	/* Utility function to show a loss message and reduce player money */
	private void _showLossMessage()
	{
		Debug.Log("You Lost!");
		_resetFruitTally();
	}

	/* Utility function to check if a value falls within a range of bounds */
	private bool _checkRange(int value, int lowerBounds, int upperBounds)
	{
		return (value >= lowerBounds && value <= upperBounds) ? true : false;

	}

	/* When this function is called it determines the betLine results.
    e.g. Bar - Orange - Banana */
	private int[] _reels()
	{
		int[] betLine = {_BLANK, _BLANK, _BLANK};
		int[] outCome = { 0, 0, 0 };

		for (var spin = 0; spin < 3; spin++)
		{
			outCome[spin] = Random.Range(1,65);

			if (_checkRange(outCome[spin], 1, 27)) {  // 41.5% probability
				betLine[spin] = _BLANK;
				_blanks++;
			}
			else if (_checkRange(outCome[spin], 28, 37)){ // 15.4% probability
				betLine[spin] = _GRAPES;
				_grapes++;
			}
			else if (_checkRange(outCome[spin], 38, 46)){ // 13.8% probability
				betLine[spin] = _BANANA;
				_bananas++;
			}
			else if (_checkRange(outCome[spin], 47, 54)){ // 12.3% probability
				betLine[spin] = _ORANGE;
				_oranges++;
			}
			else if (_checkRange(outCome[spin], 55, 59)){ //  7.7% probability
				betLine[spin] = _CHERRY;
				_cherries++;
			}
			else if (_checkRange(outCome[spin], 60, 62)){ //  4.6% probability
				betLine[spin] = _BAR;
				_bars++;
			}
			else if (_checkRange(outCome[spin], 63, 64)){ //  3.1% probability
				betLine[spin] = _BELL;
				_bells++;
			}
			else if (_checkRange(outCome[spin], 65, 65)){ //  1.5% probability
				betLine[spin] = _SEVEN;
				_sevens++;
			}
		}
		return betLine;
	}

	/* This function calculates the player's winnings, if any */
	private void _determineWinnings()
	{
		if (_blanks == 0)
		{
			if (_grapes == 3)
			{
				_winnings = _playerBet * 10;
			}
			else if (_bananas == 3)
			{
				_winnings = _playerBet * 20;
			}
			else if (_oranges == 3)
			{
				_winnings = _playerBet * 30;
			}
			else if (_cherries == 3)
			{
				_winnings = _playerBet * 40;
			}
			else if (_bars == 3)
			{
				_winnings = _playerBet * 50;
			}
			else if (_bells == 3)
			{
				_winnings = _playerBet * 75;
			}
			else if (_sevens == 3)
			{
				_winnings = _playerBet * 100;
			}
			else if (_grapes == 2)
			{
				_winnings = _playerBet * 2;
			}
			else if (_bananas == 2)
			{
				_winnings = _playerBet * 2;
			}
			else if (_oranges == 2)
			{
				_winnings = _playerBet * 3;
			}
			else if (_cherries == 2)
			{
				_winnings = _playerBet * 4;
			}
			else if (_bars == 2)
			{
				_winnings = _playerBet * 5;
			}
			else if (_bells == 2)
			{
				_winnings = _playerBet * 10;
			}
			else if (_sevens == 2)
			{
				_winnings = _playerBet * 20;
			}
			else if (_sevens == 1)
			{
				_winnings = _playerBet * 5;
			}
			else
			{
				_winnings = _playerBet * 1;
			}
			_winNumber++;
			_showWinMessage(); // not only showing message but also setting data
		}
		else
		{
			_lossNumber++;
			_showLossMessage();
			_winnings = 0;
		}
		_jackpot += _playerBet / 3; // Add the ratio of player bet to jackpot
	}

	// event handler for spin button
	public void onSpinButtonClick()
	{
		if (_playerBet <= _playerMoney)
		{
			_spinResult = _reels();
			Debug.Log(_fruits[_spinResult[0]] + " - " + _fruits[_spinResult[1]] + " - " + _fruits[_spinResult[2]]);
			_playerMoney -= _playerBet; // regardless of winning or losing, user need to pay for every game
			_determineWinnings();
			_turn++;
			_showPlayerStats();
		}
		else
		{
			Debug.Log("Please enter a valid bet amount");
		}
		_isSpinned = true;
		_refresh ();

		_spinButton.GetComponent<AudioSource>().Play (); // spinng sound
	}
	// event handler for spin button
	public void onResetButtonClick()
	{
		_resetAll ();
		_resetButton.GetComponent<AudioSource>().Play (); // reset sound
		Debug.Log("Initialze playerMoney to " + _playerMoney + ", jackpot to " + _jackpot);
	}
	// event handler for quit button
	public void onQuitButtonClick()
	{
		Debug.Log("Quit application");
		_thankYouImage.transform.position = new Vector2 (_thankYouImage.transform.position.x, 300.0f);
		_quitButton.GetComponent<AudioSource>().Play (); // quit sound
	}
	// event handler for betting buttons
	public void onBetButtonClick(int value)
	{
		if (_isSpinned) { // when user wants to continue using previous betting amount.
			_playerBet = value;
		} else { // when user wants to set betting amount again.
			_playerBet += value;		
		}
		_isSpinned = false;
		_refresh ();
		_resetButton.GetComponent<AudioSource>().Play (); // betting sound
	}
	// event handler for disabled button
	public void onCreditButtonClick(int value) {
		_isSpinned = true; // In order to start betting again from start 
		_refresh ();
	}
}