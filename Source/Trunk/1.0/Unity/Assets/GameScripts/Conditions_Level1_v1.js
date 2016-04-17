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

var moveSkin : GUISkin;
var resetSkin : GUISkin;
var settingSkin : GUISkin;
var clueSkin : GUISkin;
var bottomBorder : GUISkin;

private var xUnit : int;
private var yUnit : int;
private var xyUnit : int;
private var GameMode : String;

private var isDisplayResult : boolean = false;

private var isWin : boolean; //True if matchCount reaches WIN_NUMBER

private var isRePosition : boolean = true;

function Start()
{
TileMovement_Swipe.isFinalPos = false;
TileMovement_Swipe.level = TileMovement_Swipe.level + 1;

GameMode = PlayerPrefs.GetString("GameMode");
}

function OnGUI()
{

xUnit = Screen.width/5;
yUnit = Screen.height/10;
xyUnit = (xUnit + yUnit)/2;

GUI.skin = moveSkin;
moveSkin.label.fontSize = xUnit/4;
if(isDisplayResult == true)
{
	if(isWin == false)
	{
	GUI.Label(new Rect(Screen.width/2 - xUnit/1.5, yUnit * 2, xUnit * 3, yUnit), "Game Over !");
	}

	if(isWin == true)
	{
	GUI.Label(new Rect(Screen.width/2 - xUnit*1.25, yUnit * 2, xUnit * 3, yUnit), "Success! Loading Next Level...");		
	}
}
else
{
GUI.Label(new Rect(Screen.width/2 - xUnit*1.25, yUnit * 2, xUnit * 3, yUnit), "Match the Pattern to Win");
}

GUI.Label(new Rect(xUnit/4, yUnit/4, xUnit * 2, yUnit), "score  ");
GUI.Label(new Rect(xUnit/4, yUnit, xUnit * 2, yUnit), "" + TileMovement_Swipe.score);

if (GameMode == "Timed")
{
GUI.Label(new Rect(Screen.width - xUnit, yUnit/4, xUnit * 2, yUnit), "Time");
GUI.Label(new Rect(Screen.width - xUnit, yUnit, xUnit * 2, yUnit), "" + (Mathf.Round(TileMovement_Swipe.timeRemaining * 1000)/1000));
}

if (GameMode == "Moves")
{
GUI.Label(new Rect(Screen.width - xUnit, yUnit/4, xUnit * 2, yUnit), "Moves");
GUI.Label(new Rect(Screen.width - xUnit, yUnit, xUnit * 2, yUnit), "" + TileMovement_Swipe.moves);
}

if (GameMode == "Endless")
{
}

GUI.skin = clueSkin;
GUI.Button(new Rect(Screen.width/2 - xUnit/2, yUnit/4, xyUnit, xyUnit), "");

GUI.skin = bottomBorder;
GUI.Label(new Rect(0, Screen.height - 0.8 * xyUnit, Screen.width, 0.8 * xyUnit), "");

GUI.skin = moveSkin;
GUI.Label(new Rect(Screen.width/2 - xUnit/2, Screen.height - yUnit/2, xUnit * 2, yUnit), "level  " + TileMovement_Swipe.level);

GUI.skin = settingSkin;
if (GUI.Button(new Rect(0, Screen.height - 0.8 * xyUnit, 0.8 * xyUnit, 0.8 * xyUnit), ""))
{
	Application.LoadLevel("MainMenu");
}

GUI.skin = resetSkin;
if (GUI.Button(new Rect(Screen.width - 0.8 * xyUnit, Screen.height - 0.8 * xyUnit, 0.8 * xyUnit, 0.8 * xyUnit), ""))
{
isRePosition = true;
}

if (TileMovement_Swipe.timeRemaining <= 0 || isWin == true)								
{
	isDisplayResult = true;
}

}

function Update ()  
{
if(isWin == false)
	{
	checkWinCondition();
	}
if (isRePosition == true)
	{
		rePositionTiles();
	}
if (GameMode == "Timed")
{
	if ((isWin == false)&&(TileMovement_Swipe.timeRemaining > 0))
		{
		TileMovement_Swipe.timeRemaining = TileMovement_Swipe.timeRemaining - Time.deltaTime;	
		}
}
if (TileMovement_Swipe.timeRemaining <= 0)
	{
	TileMovement_Swipe.timeRemaining = 0;
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
	yield WaitForSeconds(0.5);
	Application.LoadLevel("Level2");	
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

tilePos[0].transform.position = POS[0];
tilePos[1].transform.position = POS[1];
tilePos[2].transform.position = POS[2];
tilePos[3].transform.position = POS[3];
tilePos[4].transform.position = POS[4];
tilePos[5].transform.position = POS[5];
tilePos[6].transform.position = POS[6];
tilePos[7].transform.position = POS[7];
tilePos[8].transform.position = POS[8];

isRePosition = false;
isWin = false;
TileMovement_Swipe.isFinalPos = false;
isDisplayResult = false;
TileMovement_Swipe.timeRemaining = 60;
TileMovement_Swipe.moves = 60;
}