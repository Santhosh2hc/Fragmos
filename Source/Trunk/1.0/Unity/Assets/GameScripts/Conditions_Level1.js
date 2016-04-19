#pragma strict

private var POS : Vector2[] = new Vector2[9];

POS[0] = Vector2(-1.5, 1.5); //Coordinates for 3x3 tile arrangement with a tile size of 1.4(140 pixels)
POS[1] = Vector2(0, 1.5);
POS[2] = Vector2(1.5, 1.5);
POS[3] = Vector2(-1.5, 0);
POS[4] = Vector2(0, 0);
POS[5] = Vector2(1.5, 0);
POS[6] = Vector2(-1.5, -1.5);
POS[7] = Vector2(0, -1.5);
POS[8] = Vector2(1.5, -1.5);

private var LOC_TOLERANCE : float = TileMovement_Swipe.TOLERANCE;

var tilePos : Transform[] = new Transform[9];

var foreGround2 : Transform;


var moveSkin : GUISkin;
var resetSkin : GUISkin;
var settingSkin : GUISkin;
var clueSkin : GUISkin;
var bottomBorder : GUISkin;
var timepowerSkin : GUISkin;
var movepowerSkin : GUISkin;
var swappowerSkin : GUISkin;
var timepowerSkin1 : GUISkin;
var movepowerSkin1 : GUISkin;
var swappowerSkin1 : GUISkin;
var timepowerSkin2 : GUISkin;
var movepowerSkin2 : GUISkin;
var swappowerSkin2 : GUISkin;
var pauseSkin : GUISkin;
var redButtonSkin : GUISkin;
var turqButtonSkin : GUISkin;
var purpleButtonSkin : GUISkin;

private var xUnit : int;
private var yUnit : int;
private var xyUnit : int;
private var GameMode : String;

private var isDisplayResult : boolean = false;

private var isWin : boolean; //True if matchCount reaches WIN_NUMBER

public static var isRePosition : boolean = false;

private var movePower : int = 0;
private var timePower : int = 0;

function Start()
{
timePower = PlayerPrefs.GetInt("timePower");
movePower = PlayerPrefs.GetInt("movePower");
TileMovement_Swipe.isFinalPos = false;
GameMode = PlayerPrefs.GetString("GameMode");
isRePosition = true;
}

function OnGUI()
{

xUnit = Screen.width/5;
yUnit = Screen.height/10;
xyUnit = (xUnit + yUnit)/2;

GUI.skin = moveSkin;
moveSkin.label.fontSize = xUnit/4;

GUI.Label(new Rect(Screen.width/2 - xUnit*1.25, yUnit * 2, xUnit * 3, yUnit), "Match the Pattern");

GUI.Label(new Rect(xUnit/4, yUnit/4, xUnit * 2, yUnit), "Level");
GUI.Label(new Rect(xUnit/4, yUnit, xUnit * 2, yUnit), "" + TileMovement_Swipe.level);

if (GameMode == "Timed")
{
GUI.Label(new Rect(Screen.width - xUnit, yUnit/4, xUnit * 2, yUnit), "Time");
GUI.Label(new Rect(Screen.width - xUnit, yUnit, xUnit * 2, yUnit), "" + (Mathf.Round(TileMovement_Swipe.timeRemaining * 1000)/1000));

GUI.skin = timepowerSkin;
GUI.Label(new Rect(0.2 * xyUnit, Screen.height - 0.7 * xyUnit, 0.5 * xyUnit, 0.5 * xyUnit), "");
GUI.skin = timepowerSkin1;
timepowerSkin.label.fontSize = xUnit/4;
GUI.Label(new Rect(0.7 * xyUnit, Screen.height - 0.7 * xyUnit, 0.5 * xyUnit, 0.5 * xyUnit), ""+timePower);
GUI.skin = timepowerSkin2;
if ((GUI.Button(new Rect(0, Screen.height - 0.9 * xyUnit, 1.3 * xyUnit, 0.9 * xyUnit), "")) && (timePower > 0))
{
timePower = timePower -1;
PlayerPrefs.SetInt("timePower", timePower);
TileMovement_Swipe.timeRemaining = TileMovement_Swipe.timeRemaining + 5;
}
}



if (GameMode == "Moves")
{
GUI.Label(new Rect(Screen.width - xUnit, yUnit/4, xUnit * 2, yUnit), "Moves");
GUI.Label(new Rect(Screen.width - xUnit, yUnit, xUnit * 2, yUnit), "" + TileMovement_Swipe.moves);

GUI.skin = movepowerSkin;
GUI.Label(new Rect(0.2 * xyUnit, Screen.height - 0.7 * xyUnit, 0.5 * xyUnit, 0.5 * xyUnit), "");
GUI.skin = movepowerSkin1;
movepowerSkin.label.fontSize = xUnit/4;
GUI.Label(new Rect(0.7 * xyUnit, Screen.height - 0.7 * xyUnit, 0.5 * xyUnit, 0.5 * xyUnit), ""+movePower);
GUI.skin = movepowerSkin2;
if ((GUI.Button(new Rect(0, Screen.height - 0.9 * xyUnit, 1.3 * xyUnit, 0.9 * xyUnit), "")) && (movePower > 0))
{
movePower = movePower - 1;
PlayerPrefs.SetInt("movePower", movePower);
TileMovement_Swipe.moves = TileMovement_Swipe.moves + 5;
}
}


if (GameMode == "Endless")
{
}

GUI.skin = clueSkin;
GUI.Button(new Rect(Screen.width/2 - xUnit/2, yUnit/4, xyUnit, xyUnit), "");

GUI.skin = swappowerSkin;
GUI.Label(new Rect(Screen.width - 1.1 * xyUnit, Screen.height - 0.7 * xyUnit, 0.5 * xyUnit, 0.5 * xyUnit), "");
GUI.skin = swappowerSkin1;
timepowerSkin.label.fontSize = xUnit/4;
GUI.Label(new Rect((Screen.width - 1.2 * xyUnit) + (0.6 * xyUnit), Screen.height - 0.7 * xyUnit, 0.5 * xyUnit, 0.5 * xyUnit), ""+TileMovement_Swipe.swapPower);
GUI.skin = swappowerSkin2;
if ((GUI.Button(new Rect(Screen.width - 1.3 * xyUnit, Screen.height - 0.9 * xyUnit, 1.3 * xyUnit, 0.9 * xyUnit), "")) && (TileMovement_Swipe.swapPower > 0))
{
TileMovement_Swipe.isApplyPowerUp = true;
}
GUI.skin = pauseSkin;
if (GUI.Button(new Rect(Screen.width/2 - 0.25 * xyUnit, Screen.height - 0.7 * xyUnit, 0.5 * xyUnit, 0.5 * xyUnit), ""))
{
if(TileMovement_Swipe.isPause == false)
{
TileMovement_Swipe.isPause = true;
}
else
{
TileMovement_Swipe.isPause = false;
}
}

if(TileMovement_Swipe.isPause == true)
{
foreGround2.transform.position.z = -1;
GUI.skin = redButtonSkin;
redButtonSkin.label.fontSize = xUnit/4;
if (GUI.Button(new Rect(Screen.width - 3.5 * xUnit, Screen.height/2 - 0.75 * xyUnit, 2 * xUnit, 0.6 * xyUnit), ""))
{
TileMovement_Swipe.isPause = false;
}
GUI.Label(new Rect(Screen.width - 3.5 * xUnit, Screen.height/2 - 0.75 * xyUnit, 2 * xUnit, 0.6 * xyUnit), "Unpause");
GUI.skin = turqButtonSkin;
turqButtonSkin.label.fontSize = xUnit/4;
if (GUI.Button(new Rect(Screen.width - 3.5 * xUnit, Screen.height/2, 2 * xUnit, 0.6 * xyUnit), ""))
{
isRePosition = true;
TileMovement_Swipe.timeRemaining = 60;
TileMovement_Swipe.moves = 60;
TileMovement_Swipe.score = 0;
TileMovement_Swipe.level = 1;
TileMovement_Swipe.isPause = false;
}
GUI.Label(new Rect(Screen.width - 3.5 * xUnit, Screen.height/2, 2 * xUnit, 0.6 * xyUnit), "Restart");
GUI.skin = purpleButtonSkin;
purpleButtonSkin.label.fontSize = xUnit/4;
if (GUI.Button(new Rect(Screen.width - 3.5 * xUnit, Screen.height/2 + 0.75 * xyUnit, 2 * xUnit, 0.6 * xyUnit), ""))
{
Application.LoadLevel("MainMenu");
}
GUI.Label(new Rect(Screen.width - 3.5 * xUnit, Screen.height/2 + 0.75 * xyUnit, 2 * xUnit, 0.6* xyUnit), "Exit to Menu");
}
else
{
foreGround2.transform.position.z = 1;
}

}

function Update ()  
{
if(isWin == false)
{
checkWinCondition();
}
else
{
isRePosition = true;
}
if (isRePosition == true)
{
	rePositionTiles();
}
if (GameMode == "Timed")
{
	if ((isWin == false)&&(TileMovement_Swipe.timeRemaining > 0)&&(TileMovement_Swipe.isPause == false))
	{
	TileMovement_Swipe.timeRemaining = TileMovement_Swipe.timeRemaining - Time.deltaTime;	
	}
}
if (TileMovement_Swipe.moves <= 0)
{
	isDisplayResult = true;
}
if (TileMovement_Swipe.timeRemaining <= 0)								
{
	TileMovement_Swipe.timeRemaining = 0;
	isDisplayResult = true;
}
if (isDisplayResult == true)
{
Application.LoadLevel("Score");
}
}

function checkWinCondition()
{
	//---------------------------------------------------------------------------------------------------------------------------------//
	//isWinCondition()	:	Checks Winning condition
	//
	//For 3x_RefTile_Div2Mul3 with maximumn of three different colors,
	//If 6 tiles(WIN_NUMBER)are matching the required position then its a WIN	
	//
	//---------------------------------------------------------------------------------------------------------------------------------//
	var matchCount : int = 0; //Number of matching tiles
	var WIN_NUMBER = 6; 	
	
	if (((tilePos[0].transform.position - POS[4]).magnitude < LOC_TOLERANCE)||((tilePos[1].transform.position - POS[4]).magnitude < LOC_TOLERANCE)||((tilePos[2].transform.position - POS[4]).magnitude < LOC_TOLERANCE))
	{
	matchCount++;		
	}		
	if (((tilePos[0].transform.position - POS[6]).magnitude < LOC_TOLERANCE)||((tilePos[1].transform.position - POS[6]).magnitude < LOC_TOLERANCE)||((tilePos[2].transform.position - POS[6]).magnitude < LOC_TOLERANCE))
	{
	matchCount++;			
	}
	if (((tilePos[0].transform.position - POS[7]).magnitude < LOC_TOLERANCE)||((tilePos[1].transform.position - POS[7]).magnitude < LOC_TOLERANCE)||((tilePos[2].transform.position - POS[7]).magnitude < LOC_TOLERANCE))
	{
	matchCount++;			
	}
	if (((tilePos[3].transform.position - POS[2]).magnitude < LOC_TOLERANCE)||((tilePos[5].transform.position - POS[2]).magnitude < LOC_TOLERANCE)||((tilePos[7].transform.position - POS[2]).magnitude < LOC_TOLERANCE))
	{
	matchCount++;		
	}		
	if (((tilePos[3].transform.position - POS[5]).magnitude < LOC_TOLERANCE)||((tilePos[5].transform.position - POS[5]).magnitude < LOC_TOLERANCE)||((tilePos[7].transform.position - POS[5]).magnitude < LOC_TOLERANCE))
	{
	matchCount++;			
	}
	if (((tilePos[3].transform.position - POS[8]).magnitude < LOC_TOLERANCE)||((tilePos[5].transform.position - POS[8]).magnitude < LOC_TOLERANCE)||((tilePos[7].transform.position - POS[8]).magnitude < LOC_TOLERANCE))
	{
	matchCount++;			
	}		
	if (matchCount == WIN_NUMBER)
	{
	matchCount = 0;
	isWin = true;	
	TileMovement_Swipe.isFinalPos = isWin;	
	TileMovement_Swipe.score = TileMovement_Swipe.score + 1;	
	TileMovement_Swipe.level = TileMovement_Swipe.level + 1;
	Handheld.Vibrate();
	//yield WaitForSeconds(1);		
	}
	else
	{
	isWin = false;
	}	
} 

function rePositionTiles()
{
//---------------------------------------------------------------------------------------------------------------------------------//
// Arranges tiles in the specified position
//---------------------------------------------------------------------------------------------------------------------------------//

var n : int = 9; 
var List : int[] = new int[9];
var  h : int;
for (h = 0; h < n; h++)
{
List[h] = h;
}
var tempList : int = 0;
var i : int;
for (i = 0; i < n; i++)
{
// Random.Range(0, 10) returns a random number between 0 and 10.
var r : int = 0; 
r = Random.Range(0, 10);
tempList = List[r];
List[r] = List[i];
List[i] = tempList;
}

var j : int;
for (j = 0; j < n; j++)
{
var k : int;
k = List[j];
tilePos[k].transform.position = POS[j];
}

isRePosition = false;
isWin = false;
TileMovement_Swipe.isFinalPos = false;
isDisplayResult = false;
}