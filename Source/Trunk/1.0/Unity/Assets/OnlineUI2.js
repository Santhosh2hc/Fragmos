#pragma strict

var turqButtonSkin : GUISkin;
var purpleButtonSkin : GUISkin;

private var xUnit : int;
private var yUnit : int;
private var xyUnit : int;

function Start () {

}

function Update () {

}

function OnGUI()
{
xUnit = Screen.width/5;
yUnit = Screen.height/10;
xyUnit = (xUnit + yUnit)/2;

GUI.skin = turqButtonSkin;
turqButtonSkin.label.fontSize = xUnit/4;
if (GUI.Button(new Rect(Screen.width - 3.5f * xUnit, Screen.height/2, 2 * xUnit, 0.6f * xyUnit), ""))
{
	Conditions_Level1.isRePosition = true;
	TileMovement_Swipe.timeRemaining = 60;
	TileMovement_Swipe.moves = 60;
	TileMovement_Swipe.score = 0;
	TileMovement_Swipe.level = 1;
	TileMovement_Swipe.isPause = false;
	Application.LoadLevel("Online");
}
GUI.Label(new Rect(Screen.width - 3.5f * xUnit, Screen.height/2, 2 * xUnit, 0.6f * xyUnit), "Play Again");
GUI.skin = purpleButtonSkin;
purpleButtonSkin.label.fontSize = xUnit/4;
if (GUI.Button(new Rect(Screen.width - 3.5f * xUnit, Screen.height/2 + 0.75f * xyUnit, 2 * xUnit, 0.6f * xyUnit), ""))
{
	Application.LoadLevel("MainMenu");
}
GUI.Label(new Rect(Screen.width - 3.5f * xUnit, Screen.height/2 + 0.75f * xyUnit, 2 * xUnit, 0.6f* xyUnit), "Exit to Menu");
}