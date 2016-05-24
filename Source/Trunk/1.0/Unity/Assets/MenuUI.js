#pragma strict

var redSkin : GUISkin;
var turqSkin : GUISkin;
var yellowSkin : GUISkin;
var blueSkin : GUISkin;
var greylineSkin : GUISkin;
var titleSkin : GUISkin;
var iconSkin : GUISkin;
var demoSkin : GUISkin;

private var xUnit : int;
private var yUnit : int;
private var xyUnit : int;

var NewInstallation : String;

function Start () 
{

PlayerPrefs.SetInt("player_Score", 0);
PlayerPrefs.SetInt("opp_Score", 0);

NewInstallation = PlayerPrefs.GetString("NewInstallation");

if (NewInstallation == "NewInst")
{

}
else
{
PlayerPrefs.SetString("NewInstallation", "NewInst");
PlayerPrefs.SetInt("movePower", 10);
PlayerPrefs.SetInt("timePower", 10);
PlayerPrefs.SetInt("swapPower", 10);
}
TileMovement_Swipe.timeRemaining = 60;
TileMovement_Swipe.moves = 60;
TileMovement_Swipe.score = 0;
TileMovement_Swipe.level = 1;
TileMovement_Swipe.isPause = false;
TileMovement_Swipe.actLevel = 1;
}

function Update () {

}

function OnGUI()
{

xUnit = Screen.width/5;
yUnit = Screen.height/10;
xyUnit = (xUnit + yUnit)/2;

GUI.skin = redSkin;
redSkin.label.fontSize = xUnit/4;
if (GUI.Button(new Rect(Screen.width/2 - 1.9 * xyUnit, yUnit * 3, 1.4 * xyUnit, 1.4 * xyUnit), ""))
{
audio.Play();
PlayerPrefs.SetString("GameMode", "Timed");
PlayerPrefs.SetString("GameMode2", "none");
Application.LoadLevel("Level1");
}
GUI.Label(new Rect(Screen.width/2 - 1.9 * xyUnit, (yUnit * 3)+(1.5 * xyUnit), 1.4 * xyUnit, yUnit), "Timed");

GUI.skin = turqSkin;
turqSkin.label.fontSize = xUnit/4;
if (GUI.Button(new Rect(Screen.width - 2.3 * xyUnit, yUnit * 3, 1.4 * xyUnit, 1.4 * xyUnit), ""))
{
audio.Play();
PlayerPrefs.SetString("GameMode", "Moves");
PlayerPrefs.SetString("GameMode2", "none");
Application.LoadLevel("Level1");
}
GUI.Label(new Rect(Screen.width - 2.3 * xyUnit, (yUnit * 3)+(1.5 * xyUnit), 1.4 * xyUnit, yUnit), "Moves");

GUI.skin = blueSkin;
blueSkin.label.fontSize = xUnit/4;
if (GUI.Button(new Rect(Screen.width/2 - 1.9 * xyUnit, yUnit * 5.5, 1.4 * xyUnit, 1.4 * xyUnit), ""))
{
audio.Play();
PlayerPrefs.SetString("GameMode", "Timed");
PlayerPrefs.SetString("GameMode2", "Online");
Application.LoadLevel("Online");
}
GUI.Label(new Rect(Screen.width/2 - 1.9 * xyUnit, (yUnit * 5.5)+(1.5 * xyUnit), 1.4 * xyUnit, yUnit), "Online");

GUI.skin = yellowSkin;
yellowSkin.label.fontSize = xUnit/4;
if (GUI.Button(new Rect(Screen.width - 2.3 * xyUnit, yUnit * 5.5, 1.4 * xyUnit, 1.4 * xyUnit), ""))
{
audio.Play();
PlayerPrefs.SetString("GameMode", "Endless");
PlayerPrefs.SetString("GameMode2", "none");
Application.LoadLevel("Level1");
}
GUI.Label(new Rect(Screen.width - 2.3 * xyUnit, (yUnit * 5.5)+(1.5 * xyUnit), 1.4 * xyUnit, yUnit), "Endless");

GUI.skin = greylineSkin;
GUI.Label(new Rect(xUnit/2, yUnit, xUnit * 4, 2), "");
GUI.Label(new Rect(xUnit/2, yUnit * 9, xUnit * 4, 2), "");

GUI.skin = titleSkin;
titleSkin.label.fontSize = xUnit/2;
GUI.Label(new Rect(Screen.width/2 - xUnit * 3/2, yUnit/3.8, xUnit * 3, xUnit/2), "FRAGMOS");

GUI.skin = iconSkin;
GUI.Label(new Rect(xUnit/2, yUnit/6, xyUnit * 0.7, xyUnit * 0.7), "");

}