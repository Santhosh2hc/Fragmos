#pragma strict

function openLevel0_step1()
{
TileMovement_Swipe.timeRemaining = 60;
TileMovement_Swipe.score = 0;
TileMovement_Swipe.level = 0;
TileMovement_Swipe.moves = 999;
Application.LoadLevel("Level0");

}