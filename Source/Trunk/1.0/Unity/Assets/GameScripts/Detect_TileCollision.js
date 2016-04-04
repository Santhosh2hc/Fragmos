#pragma strict

function OnCollisionEnter2D (colInfo : Collision2D) 
{
	if (colInfo.collider.name == "cloneTile") 
	{
		TileMovement_Swipe.isCollision = true;
	}
}