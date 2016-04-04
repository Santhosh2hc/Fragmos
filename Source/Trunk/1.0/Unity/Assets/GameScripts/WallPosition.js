#pragma strict

var mainCam : Camera;

var topWall : BoxCollider2D;
var bottomWall : BoxCollider2D;
var rightWall : BoxCollider2D;
var leftWall : BoxCollider2D;

var RefTilesize : BoxCollider2D;

function Start () {

var x_RefTile_By2 : float = RefTilesize.size.x/2; 
var x_RefTile_By2Mul5 : float = x_RefTile_By2 * 5; // Distance of Wall from center (0, 0)

var y_RefTile_By2 : float = RefTilesize.size.y/2; 
var y_RefTile_By2Mul5 : float = y_RefTile_By2 * 5; // Distance of Wall from center (0, 0)

//---------------------------------------------------------------------------------------------------------------------------------//
// Make a boxcollider to stop the movement of tiles at appropriate positions    		   										   //
//---------------------------------------------------------------------------------------------------------------------------------//

topWall.size = new Vector2 (RefTilesize.size.x * 4, 1f);
topWall.center = new Vector2 (0f, (y_RefTile_By2Mul5 + 1.01));

bottomWall.size = new Vector2 (RefTilesize.size.x * 4, 1f);
bottomWall.center = new Vector2 (0f, -(y_RefTile_By2Mul5 + 1.01));

rightWall.size = new Vector2 (1f, RefTilesize.size.y * 4);
rightWall.center = new Vector2 ((x_RefTile_By2Mul5 + 1.01), 0f);

leftWall.size = new Vector2 (1f, RefTilesize.size.y * 4);
leftWall.center = new Vector2 (-(x_RefTile_By2Mul5 + 1.01), 0f);

}

