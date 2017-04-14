using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlotMachine : MonoBehaviour {

	private int _playerMoney = 1000;
	private int _winnings = 0;
	private int _jackpot = 5000;
	private float _turn = 0.0f;
	private int _playerBet = 0;

	private float _winNumber = 0.0f;
	private float _lossNumber = 0.0f;
	private string[] _spinResult;
	private string _fruits = "";
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

	const float _OUT_OF_SCREEN = 4000.0f;
	private bool _isSpinned = false;
	private bool _isJackpot = false;

	private const float _defaultPositionXDisabledButton = 512.0f;
	private const float _defaultPositionXJackpotWinImage = 0.0f;

	private GameObject _thankYouImage;
	private GameObject _playerMoneyText;
	private GameObject _playerBetText;
	private GameObject _winningsText;
	private GameObject _jackpotText;
	private GameObject _spinDisabledButton;
	private GameObject _jackpotWinImage;
	private GameObject _jackpotWinText;

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

		//Debug.Log (_jackpotWinImage.transform.position.x);

		_resetAll ();
	}

	private void _refresh () {
		if (_playerMoney == 0 || _playerBet > _playerMoney || _playerBet < 0) {
			_setActiveDisabledButton(_spinDisabledButton, true); // need to show disabled button
		} else {
			_setActiveDisabledButton(_spinDisabledButton, false); // hide disabled button
		}

		//Debug.Log (gameObject.transform.position.y);
		_setActiveJackpotWinImage (_isJackpot);
		_isJackpot = false; // after showing jackpot, it needs to be hidden. 

		_setText (_playerMoneyText, _playerMoney.ToString());
		_setText (_jackpotText, _jackpot.ToString ());
		_setText (_playerBetText, _playerBet.ToString ());
		_setText (_winningsText, _winnings.ToString ());
	}
	private void _setActiveDisabledButton(GameObject gameObject, bool isActive) {
		if (isActive) {
			gameObject.transform.position = new Vector2 (_defaultPositionXDisabledButton, gameObject.transform.position.y);
		} else {
			gameObject.transform.position = new Vector2 (_OUT_OF_SCREEN, gameObject.transform.position.y);
		}
	}
	private void _setActiveJackpotWinImage(bool isActive) {
		GameObject gameObject = _jackpotWinImage;
		if (isActive) {
			gameObject.transform.position = new Vector2 (_defaultPositionXJackpotWinImage, gameObject.transform.position.y);
		} else {
			gameObject.transform.position = new Vector2 (_OUT_OF_SCREEN, gameObject.transform.position.y);
		}
	}
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

		_refresh ();
	}

	/* Check to see if the player won the jackpot */
	private void _checkJackPot()
	{
		/* compare two random values */
		var jackPotTry = Random.Range (1, 21);
		var jackPotWin = Random.Range (1, 21);
		if (jackPotTry == jackPotWin)
		{
			Debug.Log("You Won the $" + _jackpot + " Jackpot!!");
			_setText (_jackpotWinText, "You Won the $" + _jackpot + " Jackpot!!");
			_isJackpot = true;
			_playerMoney += _jackpot;
			_jackpot = 1000;
		}
	}

	/* Utility function to show a win message and increase player money */
	private void _showWinMessage()
	{
		_playerMoney += _winnings;
		Debug.Log("You Won: $" + _winnings);
		_resetFruitTally();
		_checkJackPot();
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
	private string[] _reels()
	{
		string[] betLine = { " ", " ", " " };
		int[] outCome = { 0, 0, 0 };

		for (var spin = 0; spin < 3; spin++)
		{
			outCome[spin] = Random.Range(1,65);

			if (_checkRange(outCome[spin], 1, 27)) {  // 41.5% probability
				betLine[spin] = "blank";
				_blanks++;
			}
			else if (_checkRange(outCome[spin], 28, 37)){ // 15.4% probability
				betLine[spin] = "Grapes";
				_grapes++;
			}
			else if (_checkRange(outCome[spin], 38, 46)){ // 13.8% probability
				betLine[spin] = "Banana";
				_bananas++;
			}
			else if (_checkRange(outCome[spin], 47, 54)){ // 12.3% probability
				betLine[spin] = "Orange";
				_oranges++;
			}
			else if (_checkRange(outCome[spin], 55, 59)){ //  7.7% probability
				betLine[spin] = "Cherry";
				_cherries++;
			}
			else if (_checkRange(outCome[spin], 60, 62)){ //  4.6% probability
				betLine[spin] = "Bar";
				_bars++;
			}
			else if (_checkRange(outCome[spin], 63, 64)){ //  3.1% probability
				betLine[spin] = "Bell";
				_bells++;
			}
			else if (_checkRange(outCome[spin], 65, 65)){ //  1.5% probability
				betLine[spin] = "Seven";
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
			_showWinMessage();
		}
		else
		{
			_lossNumber++;
			_showLossMessage();
			_winnings = 0;
		}
		_jackpot += _playerBet / 5; // Add the ratio of player bet to jackpot
	}

	public void onSpinButtonClick()
	{
		if (_playerBet <= _playerMoney)
		{
			_spinResult = _reels();
			_fruits = _spinResult[0] + " - " + _spinResult[1] + " - " + _spinResult[2];
			Debug.Log(_fruits);
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
	}
	public void onResetButtonClick()
	{
		_resetAll ();
		Debug.Log("Initialze playerMoney to " + _playerMoney + ", jackpot to " + _jackpot);
	}
	public void onQuitButtonClick()
	{
		Debug.Log("Quit application");
		_thankYouImage.transform.position = new Vector2 (_thankYouImage.transform.position.x, 300.0f);
	}
	public void onBetButtonClick(int value)
	{
		if (_isSpinned) { // when user wants to continue using previous betting amount.
			_playerBet = value;
		} else { // when user wants to set betting amount again.
			_playerBet += value;		
		}
		_isSpinned = false;
		_refresh ();
	}
	public void onCreditButtonClick(int value) {
		_isSpinned = true; // In order to start betting again from start 
		_refresh ();
	}
}