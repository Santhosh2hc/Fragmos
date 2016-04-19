#pragma strict

//---------------------------------------------------------------------------------------------------------------------------------//
// All the movements (Identifying the tiles to be moved, Movement distance etc.) are based on the size of refTilesize			   //
//---------------------------------------------------------------------------------------------------------------------------------//

private var isMovementInProgress : boolean = false;

var refTilesize : BoxCollider2D;

private var xPosition : float; //Position of the tiles to be moved
private var yPosition : float;

private var tileSpeed : float = 10f;//Speed with which the tiles are moved
private var tileVelocity : float; //Velocity with which the tiles are moved(Has direction)

private var startPos : Vector2; //Start Position of swipe
private var endPos : Vector2;  //End Position of Swipe
private var xMagnitude : float; //X magnitude of swipe
private var yMagnitude : float; //Y magniitude of swipe

private var swapPosition : Vector2;
private var swapPos1 : Vector2;
private var swapPos2 : Vector2;
private var tempSwapPos : Vector2 = Vector2(10f, 10f);
private var swapTileNumber : int;

var tilePos : Transform[] = new Transform[9];
var tileVel : Rigidbody2D[] = new Rigidbody2D[9];
var tileCol : BoxCollider2D[] = new BoxCollider2D[9];
public var tile_GameObj : GameObject[] = new GameObject[9];

private var cloneTile : GameObject;

private var cloneTile_initPosx : float;
private var cloneTile_initPosy : float;

public static var TOLERANCE : float = 0.3;

var mainCam : Camera;

private var x_RefTile_Div2 : float; //Reference Tile Sizes
private var x_RefTile : float;
private var x_RefTile_Div2Mul3 : float;
private var y_RefTile_Div2 : float;
private var y_RefTile : float;
private var y_RefTile_Div2Mul3 : float;

//Global Variables
public static var moves : int = 60; //Default value (This value will be used if this variable remains unchanged in a level)
public static var timeRemaining : float = 60;
public static var score : int = 0;
public static var level : int = 0;
public static var isFinalPos : boolean = false; //Final (Winning) Position
public static var isApplyPowerUp : boolean = false;
public static var swapPower : int = 0;
public static var isPause : boolean = false;
private var isNewTouch : boolean = false;
private var isValidMagnit : boolean = false; //Valid Magnitude (Magnitude above required threshold)

private var TOUCHSENS : float = 0.20; //Touch Sensitivity

private var isGameStart : boolean = false;
private var isGameOver : boolean = false;
private var isGetSwapPos2 : boolean = false;
private var isValidSwapTile : boolean = false;

public static var isCollision : boolean = false;

private var tileNumber : int = 0;
private var movingTiles : int[] = new int[3];
private var movingTilePosx : float[] = new float[3];
private var movingTilePosy : float[] = new float[3];

function Start()
{
swapPower = PlayerPrefs.GetInt("swapPower");
}

function Update ()  
{
	x_RefTile_Div2 = refTilesize.size.x/2; //Reference Tile Sizes
	x_RefTile = refTilesize.size.x;
	x_RefTile_Div2Mul3 = (refTilesize.size.x/2)*3;
	y_RefTile_Div2 = refTilesize.size.y/2;
	y_RefTile = refTilesize.size.y;
	y_RefTile_Div2Mul3 = (refTilesize.size.y/2)*3;
	
	if (isMovementInProgress == false)
	{
		getSwipeCoordinates(); //Get swipe coordinates and magnitude	
		setSpeed();	
		moveTiles();	//Move tiles				
	}
	if (isMovementInProgress == true)
	{
		stopTileMovement();	// Stops tiles(at approprioate position) if isMovementInProgress == true 					
	}
	if (isApplyPowerUp == true)
	{
	swapPosition = getSwapTile();
	reScale_andSwapTiles();	
	}
	
}

function getSwipeCoordinates()
{

//---------------------------------------------------------------------------------------------------------------------------------//
//Get Start co-ordinate and End co-ordinate for the swipe                                                                          //
//Get X Magnitude and Y Magnitude for the swipe using the coordinates                                                              //
//---------------------------------------------------------------------------------------------------------------------------------//

	if (Input.touchCount > 0) 
	{
	var touch : Touch = Input.touches[0];
		
		switch (touch.phase) 
		{
		case TouchPhase.Began:
		startPos = touch.position;		
		startPos.x = mainCam.ScreenToWorldPoint (Vector3 (startPos.x, 0, 0)).x;
		startPos.y = mainCam.ScreenToWorldPoint (Vector3 (0, startPos.y, 0)).y;
		isNewTouch = true;
		isValidMagnit = false; // Make isValidMagnit true only after getting endpos		
		break;
		
		case TouchPhase.Moved:
		
		endPos = touch.position;			
		endPos.x = mainCam.ScreenToWorldPoint (Vector3 (endPos.x, 0, 0)).x;
		endPos.y = mainCam.ScreenToWorldPoint (Vector3 (0, endPos.y, 0)).y;
		xMagnitude = (new Vector2(endPos.x, 0) - new Vector2(startPos.x, 0)).magnitude;
		yMagnitude = (new Vector2(0, endPos.y) - new Vector2(0, startPos.y)).magnitude;
		if(((xMagnitude > TOUCHSENS) || (yMagnitude > TOUCHSENS)) &&  (isValidMagnit == false))
		{
		isValidMagnit = true;
		}
		break;
							
		case TouchPhase.Ended:
		endPos = touch.position;				
		endPos.x = mainCam.ScreenToWorldPoint (Vector3 (endPos.x, 0, 0)).x;
		endPos.y = mainCam.ScreenToWorldPoint (Vector3 (0, endPos.y, 0)).y;
		xMagnitude = (new Vector2(endPos.x, 0) - new Vector2(startPos.x, 0)).magnitude;
		yMagnitude = (new Vector2(0, endPos.y) - new Vector2(0, startPos.y)).magnitude;
		if(((xMagnitude > TOUCHSENS) || (yMagnitude > TOUCHSENS)) &&  (isValidMagnit == false))
		{
		isValidMagnit = true;
		}
		break;
		default :
		break;
		}
	}
}

function setSpeed()
{
	tileSpeed = 20; //20 is a magic number which can be changed to improve user experience
}

function moveTiles()
{
	if ((Mathf.Abs(startPos.x) < x_RefTile_Div2Mul3)&&(Mathf.Abs(startPos.y) < y_RefTile_Div2Mul3)&&(moves > 0)&&(isFinalPos == false)&&(isNewTouch == true)&&(isValidMagnit == true)&&(timeRemaining > 0)&&(isApplyPowerUp == false)&&(isPause == false))
	{
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		// Do not move
		//If Start Position.x  or Start Position.y is outside the tiles 
		//If number of remaining moves is 0
		//If isFinalPos(Final position) is achieved
		//If the swipe is from the same old touch
		//If the magnitude is less than the threshold	(VMAG will be false in that case)		
		//If Time Remaining is less than 0
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		moves--; //For every valid move decrement the move count		
		audio.Play();
		isNewTouch = false; 
		
		if(xMagnitude > yMagnitude)
		{
		
			//------------------------------------------------------------------------------------------------------------------------------------------------------------------//
			// If Start Position.x  is greater than End Position.x then swipe is towards left direction and the movement should be towards left direction and vice versa        //
			//------------------------------------------------------------------------------------------------------------------------------------------------------------------//

			if(startPos.x > endPos.x)
			{
			tileVelocity = -tileSpeed;
			}
			else if(endPos.x > startPos.x)
			{
			tileVelocity = tileSpeed;
			}
							
			//---------------------------------------------------------------------------------------------------------------------------------//
			// If xMagnitude is greater than yMagnitude then movement is in x direction and vice versa                                         //
			// In x direction the swipe may be done on three possible tiles, which can be determined by the range of startpos.y                //
			//---------------------------------------------------------------------------------------------------------------------------------//
		
			if(startPos.y >= y_RefTile_Div2 && startPos.y <= y_RefTile_Div2Mul3)
			{
			yPosition = y_RefTile;		
			}
			if(startPos.y >= -y_RefTile_Div2 && startPos.y <= y_RefTile_Div2)
			{
			yPosition = 0;
			}
			if(startPos.y >= -y_RefTile_Div2Mul3 && startPos.y <= -y_RefTile_Div2)
			{
			yPosition = -y_RefTile;
			}
							
			//---------------------------------------------------------------------------------------------------------------------------------//
			// Identify the position of the tile to be cloned     									   										   //
			//---------------------------------------------------------------------------------------------------------------------------------//				
			
			var l : int;
			for(l = 0; l < 9; l++)
			{
				if((tilePos[l].transform.position.y - yPosition <= TOLERANCE) && (tilePos[l].transform.position.y - yPosition >= -TOLERANCE))
 				{
	 				if(startPos.x > endPos.x)
					{
						if((tilePos[l].transform.position.x - (-x_RefTile) <= TOLERANCE) && (tilePos[l].transform.position.x - (-x_RefTile) >= -TOLERANCE))
	 					{
	 					break;
	 					}
					}
					else if(endPos.x > startPos.x)
					{
						if((tilePos[l].transform.position.x - (x_RefTile) <= TOLERANCE) && (tilePos[l].transform.position.x - (x_RefTile) >= -TOLERANCE))
	 					{
	 					break;
	 					}
					}
 				}
			}
			
			//---------------------------------------------------------------------------------------------------------------------------------//
			// Instatiate a Game object at appropriate position* and assign to the GameObject 'cloneTile'     									   //
			//---------------------------------------------------------------------------------------------------------------------------------//
			
			cloneTile = Instantiate (tilePos[l].gameObject, Vector3(tilePos[l].transform.position.x, tilePos[l].transform.position.y, 0), Quaternion.identity);
			cloneTile.name = "cloneTile";
			
			//---------------------------------------------------------------------------------------------------------------------------------//
			// Move the original tile to top or bottom after cloning based on the swipe direction						   					   //
			//---------------------------------------------------------------------------------------------------------------------------------//
			
			if(startPos.x > endPos.x)
			{
			tilePos[l].transform.position.x = x_RefTile + x_RefTile;
			}
			else if(endPos.x > startPos.x)
			{
			tilePos[l].transform.position.x = -(x_RefTile + x_RefTile);
			}				
			
			//---------------------------------------------------------------------------------------------------------------------------------//
			// Store the initial cloneTile position before movement in a variable										   					   //
			//---------------------------------------------------------------------------------------------------------------------------------//
			
			cloneTile_initPosx = cloneTile.transform.position.x;
			cloneTile_initPosy = cloneTile.transform.position.y;
			
			//----------------------------------------------------------------------------------------------------------------------------------//
			// Find the tiles to be moved
			// Store the tile number
			// Compute and Store the final position to which the tiles are being moved				  					
			//---------------------------------------------------------------------------------------------------------------------------------//	
			
			tileNumber = 0;
			for(var i : int = 0; i < 9; i++)
 			{ 	
 				if((tilePos[i].transform.position.y - yPosition <= TOLERANCE) && (tilePos[i].transform.position.y - yPosition >= -TOLERANCE))
 				{ 				
	 				movingTiles[tileNumber] = i;
	 				movingTilePosy[tileNumber] = yPosition;
	 				if(tileVelocity > 0)
	 				{
		 				movingTilePosx[tileNumber] = tilePos[i].transform.position.x + 1.5; 
		 				if(movingTilePosx[tileNumber] > (1.5 + TOLERANCE))
		 				{
		 					movingTilePosx[tileNumber] = -1.5;
		 				}
	 				}
	 				if(tileVelocity < 0)
	 				{
	 					movingTilePosx[tileNumber] = tilePos[i].transform.position.x - 1.5; 
		 				if(movingTilePosx[tileNumber] < -(1.5 + TOLERANCE))
		 				{
		 					movingTilePosx[tileNumber] = 1.5;
		 				}
	 				}
	 				tileNumber++;
 				} 
 			}
 			
 			//---------------------------------------------------------------------------------------------------------------------------------//
			// Apply velocity to tiles and add Box Collider so that it will hit the wall and stop
			//---------------------------------------------------------------------------------------------------------------------------------//
			var b : int = 0;
			cloneTile.rigidbody2D.velocity.x = tileVelocity;
			for(var a : int = 0; a < 3; a++)
 			{
 			b = movingTiles[a];
 			tileVel[b].rigidbody2D.velocity.x = tileVelocity;  			
 			tileCol[b].size = refTilesize.size;	
			}
 			
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		// Make isMovementInProgress true  																																    //
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------//

		isMovementInProgress = true;
		}
		
		else if(xMagnitude < yMagnitude)
		{
		
			//------------------------------------------------------------------------------------------------------------------------------------------------------------------//
			// If Start Position.y  is greater than End Position.y then swipe is towards down direction and the movement should be towards down direction and vice versa        //
			//------------------------------------------------------------------------------------------------------------------------------------------------------------------//

			if(startPos.y > endPos.y)
			{
			tileVelocity = -tileSpeed;
			}
			else if(endPos.y > startPos.y)
			{
			tileVelocity = tileSpeed;
			}
			
			//---------------------------------------------------------------------------------------------------------------------------------//
			// If xMagnitude is greater than yMagnitude then movement is in x direction and vice versa                                         //
			// In y direction the swipe may be done on three possible tiles, which can be determined by the range of startpos.x                //
			//---------------------------------------------------------------------------------------------------------------------------------//
			
			if(startPos.x >= x_RefTile_Div2 && startPos.x <= x_RefTile_Div2Mul3)
			{
			xPosition = x_RefTile;
			}
			if(startPos.x >= -x_RefTile_Div2 && startPos.x <= x_RefTile_Div2)
			{
			xPosition = 0;
			}
			if(startPos.x >= -x_RefTile_Div2Mul3 && startPos.x <= -x_RefTile_Div2)
			{
			xPosition = -x_RefTile;
			}
			
			//---------------------------------------------------------------------------------------------------------------------------------//
			// Identify the position of the tile to be cloned     									   										   //
			//---------------------------------------------------------------------------------------------------------------------------------//				
			
			var k : int;
			for(k = 0; k < 9; k++)
			{
				if((tilePos[k].transform.position.x - xPosition <= TOLERANCE) && (tilePos[k].transform.position.x - xPosition >= -TOLERANCE))
 				{
	 				if(startPos.y > endPos.y)
					{
						if((tilePos[k].transform.position.y - (-y_RefTile) <= TOLERANCE) && (tilePos[k].transform.position.y - (-y_RefTile) >= -TOLERANCE))
	 					{
	 					break;
	 					}
					}
					else if(endPos.y > startPos.y)
					{
						if((tilePos[k].transform.position.y - (y_RefTile) <= TOLERANCE) && (tilePos[k].transform.position.y - (y_RefTile) >= -TOLERANCE))
	 					{
	 					break;
	 					}
					}
 				}
			}
			
			//---------------------------------------------------------------------------------------------------------------------------------//
			// Instatiate a Game object at appropriate position* and assign to the GameObject 'cloneTile'     									   //
			//---------------------------------------------------------------------------------------------------------------------------------//
			
			cloneTile = Instantiate (tilePos[k].gameObject, Vector3(tilePos[k].transform.position.x, tilePos[k].transform.position.y, 0), Quaternion.identity);
			cloneTile.name = "cloneTile";
			
			//---------------------------------------------------------------------------------------------------------------------------------//
			// Move the original tile to top or bottom after cloning based on the swipe direction						   					   //
			//---------------------------------------------------------------------------------------------------------------------------------//
			
			if(startPos.y > endPos.y)
			{
			tilePos[k].transform.position.y = y_RefTile + y_RefTile;
			}
			else if(endPos.y > startPos.y)
			{
			tilePos[k].transform.position.y = -(y_RefTile + y_RefTile);
			}				
			
			//---------------------------------------------------------------------------------------------------------------------------------//
			// Store the initial cloneTile position before movement in a variable											   					   //
			//---------------------------------------------------------------------------------------------------------------------------------//
			
			cloneTile_initPosx = cloneTile.transform.position.x;
			cloneTile_initPosy = cloneTile.transform.position.y;
			
			//----------------------------------------------------------------------------------------------------------------------------------//
			// Find the tiles to be moved
			// Store the tile number
			// Compute and Store the final position to which the tiles are being moved				  					
			//---------------------------------------------------------------------------------------------------------------------------------//	
			
			tileNumber = 0;
			for(var j : int = 0; j < 9; j++)
 			{ 
 				if((tilePos[j].transform.position.x - xPosition <= TOLERANCE) && (tilePos[j].transform.position.x - xPosition >= -TOLERANCE))
 				{ 					
 				movingTiles[tileNumber] = j;
 				movingTilePosx[tileNumber] = xPosition;
 				if(tileVelocity > 0)
 				{
	 				movingTilePosy[tileNumber] = tilePos[j].transform.position.y + 1.5; 
	 				if(movingTilePosy[tileNumber] > (1.5 + TOLERANCE))
	 				{
	 					movingTilePosy[tileNumber] = -1.5;
	 				}
 				}
 				if(tileVelocity < 0)
 				{
 					movingTilePosy[tileNumber] = tilePos[j].transform.position.y - 1.5; 
	 				if(movingTilePosy[tileNumber] < -(1.5 + TOLERANCE))
	 				{
	 					movingTilePosy[tileNumber] = 1.5;
	 				}
 				}
 				tileNumber++;
 				}
 			}
 			
 			//---------------------------------------------------------------------------------------------------------------------------------//
			// Apply velocity to tiles and add Box Collider so that it will hit the wall and stop
			//---------------------------------------------------------------------------------------------------------------------------------//
			var d : int = 0;
			cloneTile.rigidbody2D.velocity.y = tileVelocity;
			for(var c : int = 0; c < 3; c++)
 			{
 			d = movingTiles[c];
 			tileVel[d].rigidbody2D.velocity.y = tileVelocity; 
 			tileCol[d].size = refTilesize.size;	
			}
 			 			
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		// Make isMovementInProgress True																																        //
		//------------------------------------------------------------------------------------------------------------------------------------------------------------------//
		
		isMovementInProgress = true; 			
		}						
	}	
}

function stopTileMovement()
{

//---------------------------------------------------------------------------------------------------------------------------------//
// Stops the tiles if the distance of movement is greater than or equal to one unit (size of one tile)			  				   //
//---------------------------------------------------------------------------------------------------------------------------------//

	if ((((new Vector2(cloneTile.transform.position.x, 0) - new Vector2(cloneTile_initPosx, 0)).magnitude) >= x_RefTile)||(((new Vector2(0, cloneTile.transform.position.y) - new Vector2(0, cloneTile_initPosy)).magnitude) >= y_RefTile) || (isCollision == true))
	{
		isCollision = false;
		cloneTile.rigidbody2D.velocity.y = 0;
		for(var m : int = 0; m < 9; m++)
		{
		tileVel[m].rigidbody2D.velocity.x = 0; 
		tileVel[m].rigidbody2D.velocity.y = 0; 
		tileCol[m].size = new Vector2(1, 1);	
		}
		
		//---------------------------------------------------------------------------------------------------------------------------------//
		// Destroy the cloneTile																										   //
		//---------------------------------------------------------------------------------------------------------------------------------//
		
		Destroy (cloneTile);
		isMovementInProgress = false;			
		
		//---------------------------------------------------------------------------------------------------------------------------------//
		// Re - arrange the tiles in appropriate position      																			   //
		//---------------------------------------------------------------------------------------------------------------------------------//
		
		var p : int = 0;
		for(var n : int = 0; n < 3; n++)
		{
		p = movingTiles[n];
		tilePos[p].transform.position.x = movingTilePosx[n];
		tilePos[p].transform.position.y = movingTilePosy[n];				
		}
	}	
}

function reScale_andSwapTiles()
{	
	var tempTilePos : float;	
	if((isNewTouch == true)&&(isValidSwapTile == true))
	{
		isNewTouch = false;
		isValidSwapTile = false;
		//---------------------------------------------------------------------------------------------------------------------------------//
		// Break out of for loop once the tile number of the tile touched by user is identified   																			  
		//---------------------------------------------------------------------------------------------------------------------------------//
		
		var q : int;
		for(q = 0; q < 9; q++)
		{
			if((tilePos[q].transform.position.x - swapPosition.x <= TOLERANCE) && (tilePos[q].transform.position.x - swapPosition.x >= -TOLERANCE))
			{
				if((tilePos[q].transform.position.y - swapPosition.y <= TOLERANCE) && (tilePos[q].transform.position.y - swapPosition.y >= -TOLERANCE))
				{
				break;
				}
			}		
		}	
		
		//---------------------------------------------------------------------------------------------------------------------------------//
		// Re size the tile touched by the user(make it smaller) and change their z values so that they dont collide with each other																	  
		//---------------------------------------------------------------------------------------------------------------------------------//
		
		tile_GameObj[q].gameObject.transform.localScale -= new Vector3(0.2f,0.2f,0);
		tilePos[q].transform.position.z = tempTilePos;
		
		//---------------------------------------------------------------------------------------------------------------------------------//
		// If the user touches any of the tiles and if the touch is new then get the get swap position 1 and swap position 2     																			  
		//---------------------------------------------------------------------------------------------------------------------------------//
		
		if(isGetSwapPos2 == false)
		{
			swapPos1 = swapPosition;			
			isGetSwapPos2 = true;
			tempTilePos = 1;	
			swapTileNumber = q;	
			swapPower = swapPower - 1;
			PlayerPrefs.SetInt("swapPower", swapPower);
		}
		else
		{
			swapPos2 = swapPosition;			
			isGetSwapPos2 = false;			
			tempTilePos = 2;
			isApplyPowerUp = false;
			
			//----------------------------------------------------------------------------------------------------------------------------------//
			// Swap the tile positions and resize them 																			  
			//----------------------------------------------------------------------------------------------------------------------------------//
			
			yield WaitForSeconds (0.5);
			tilePos[q].transform.position.x = swapPos1.x;
			tilePos[q].transform.position.y = swapPos1.y;				
			tilePos[swapTileNumber].transform.position.x = swapPos2.x;
			tilePos[swapTileNumber].transform.position.y = swapPos2.y;
			yield WaitForSeconds (0.5);
			tile_GameObj[q].gameObject.transform.localScale += new Vector3(0.2f,0.2f,0);
			tile_GameObj[swapTileNumber].gameObject.transform.localScale += new Vector3(0.2f,0.2f,0);
			
			//---------------------------------------------------------------------------------------------------------------------------------//
			// Change the Z values of both the tiles as 0																			  
			//---------------------------------------------------------------------------------------------------------------------------------//
					
			tilePos[q].transform.position.z = 0;
			tilePos[swapTileNumber].transform.position.z = 0;		
		}		
	}	
}

function getSwapTile()
{	
	var swapPos : Vector2;
	if((Mathf.Abs(startPos.x) < x_RefTile_Div2Mul3)&&(Mathf.Abs(startPos.y) < y_RefTile_Div2Mul3)&&(isNewTouch == true))
	{
	//---------------------------------------------------------------------------------------------------------------------------------//
	// Get Y Position of the tile to be swapped      																			  
	//---------------------------------------------------------------------------------------------------------------------------------//
	if(startPos.y >= y_RefTile_Div2 && startPos.y <= y_RefTile_Div2Mul3)
	{
	swapPos.y = y_RefTile;		
	}
	if(startPos.y >= -y_RefTile_Div2 && startPos.y <= y_RefTile_Div2)
	{
	swapPos.y = 0;
	}
	if(startPos.y >= -y_RefTile_Div2Mul3 && startPos.y <= -y_RefTile_Div2)
	{
	swapPos.y = -y_RefTile;
	}
	
	//---------------------------------------------------------------------------------------------------------------------------------//
	// Get X Position of the tile to be swapped      																			  
	//---------------------------------------------------------------------------------------------------------------------------------//
	
	if(startPos.x >= x_RefTile_Div2 && startPos.x <= x_RefTile_Div2Mul3)
	{
	swapPos.x = x_RefTile;
	}
	if(startPos.x >= -x_RefTile_Div2 && startPos.x <= x_RefTile_Div2)
	{
	swapPos.x = 0;
	}
	if(startPos.x >= -x_RefTile_Div2Mul3 && startPos.x <= -x_RefTile_Div2)
	{
	swapPos.x = -x_RefTile;
	}
	
	//---------------------------------------------------------------------------------------------------------------------------------//
	// Cache the first swipe tile's position in tempSwapPos and if the user touches the same tile again then do not do swapping      																			  
	//---------------------------------------------------------------------------------------------------------------------------------//
	
	if (swapPos == tempSwapPos)
	{
	return Vector2(0f, 0f);
	}
	else
	{
	isValidSwapTile = true;	
	tempSwapPos = swapPos;
	return swapPos;
	}
	
	}
	else
	{
	return Vector2(0f, 0f);
	}
}
