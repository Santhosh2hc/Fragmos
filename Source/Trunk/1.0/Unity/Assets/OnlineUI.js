#pragma strict

function Start () 
{
PlayerPrefs.SetInt("player_Score", 0);
PlayerPrefs.SetInt("opp_Score", 0);
}

function Update () {

}

function openLevel1()
{
Application.LoadLevel("Level1");
}