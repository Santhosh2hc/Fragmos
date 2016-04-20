#pragma strict

var titleSkin : GUISkin;
var greylineSkin : GUISkin;
var demoSkin : GUISkin;
var turqButtonSkin : GUISkin;
var purpleButtonSkin : GUISkin;

private var xUnit : int;
private var yUnit : int;
private var xyUnit : int;
private var tempScore : int;

function Start () 
{
tempScore = TileMovement_Swipe.score * 10;
}

function Update () {

}

function OnGUI()
{

xUnit = Screen.width/5;
yUnit = Screen.height/10;
xyUnit = (xUnit + yUnit)/2;

GUI.skin = greylineSkin;
GUI.Label(new Rect(xUnit/2, yUnit, xUnit * 4, 2), "");
GUI.Label(new Rect(xUnit/2, yUnit * 9, xUnit * 4, 2), "");

GUI.skin = titleSkin;
titleSkin.label.fontSize = xUnit/2;
GUI.Label(new Rect(Screen.width/2 - xUnit * 3/2, yUnit/3.8, xUnit * 3, xUnit/2), "SCORE");

GUI.skin = demoSkin;
demoSkin.label.fontSize = xUnit/4;
GUI.Label(new Rect(Screen.width/2 - xUnit/2, yUnit * 2, xUnit * 3, yUnit), "  Score  "+tempScore);

GUI.skin = turqButtonSkin;
turqButtonSkin.label.fontSize = xUnit/4;
if (GUI.Button(new Rect(Screen.width - 3.5 * xUnit, Screen.height/2, 2 * xUnit, 0.6 * xyUnit), ""))
{
TileMovement_Swipe.timeRemaining = 60;
TileMovement_Swipe.moves = 60;
TileMovement_Swipe.score = 0;
TileMovement_Swipe.level = 1;
TileMovement_Swipe.isPause = false;
Application.LoadLevel("Level1");
}
GUI.Label(new Rect(Screen.width - 3.5 * xUnit, Screen.height/2, 2 * xUnit, 0.6 * xyUnit), "Play Again");
GUI.skin = purpleButtonSkin;
purpleButtonSkin.label.fontSize = xUnit/4;
if (GUI.Button(new Rect(Screen.width - 3.5 * xUnit, Screen.height/2 + 0.75 * xyUnit, 2 * xUnit, 0.6 * xyUnit), ""))
{
Application.LoadLevel("MainMenu");
}
GUI.Label(new Rect(Screen.width - 3.5 * xUnit, Screen.height/2 + 0.75 * xyUnit, 2 * xUnit, 0.6* xyUnit), "Menu");
}