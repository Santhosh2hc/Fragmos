using UnityEngine;
using System.Collections;

public class RTM_1 : MonoBehaviour {
	
	int xUnit, yUnit, xyUnit, score = 0, a = 0;
	public GUISkin moveSkin = null;

	void Start () 
	{   
		//listen for GooglePlayConnection events	
		GooglePlayConnection.ActionPlayerConnected +=  OnPlayerConnected;
		GooglePlayConnection.ActionPlayerDisconnected += OnPlayerDisconnected;		
		GooglePlayConnection.ActionConnectionResultReceived += OnConnectionResult;		
		
		if(GooglePlayConnection.State == GPConnectionState.STATE_CONNECTED) {
			//checking if player already connected
			OnPlayerConnected ();
		} 

		GooglePlayConnection.instance.Connect ();
	}

	private void OnPlayerConnected() 
	{
		SA_StatusBar.text = "Player Connected";
		findMatch ();
	}

	private void findMatch() 
	{
		int minPlayers = 1;
		int maxPlayers = 2;
		
		GooglePlayRTM.instance.FindMatch(minPlayers, maxPlayers);
	}

	private void OnConnectionResult(GooglePlayConnectionResult result) 
	{
		SA_StatusBar.text = "ConnectionResul:  " + result.code.ToString();
		Debug.Log(result.code.ToString());
	}

	private void OnPlayerDisconnected() 
	{
		SA_StatusBar.text = "Player Disconnected";
	}

	void OnGUI() 
	{
		xUnit = Screen.width/5;
		yUnit = Screen.height/10;
		xyUnit = (xUnit + yUnit)/2;
		
		GUI.skin = moveSkin;
		moveSkin.label.fontSize = xUnit/4;
		GUI.Label(new Rect(Screen.width/2 - xUnit/2, yUnit * 2, xUnit * 3, yUnit), "Waiting for Opponent...");
	}

	void Update()
	{
		if(GooglePlayRTM.instance.currentRoom.status == GP_RTM_RoomStatus.ROOM_STATUS_ACTIVE) 
		{
			gameObject.SendMessage ("openLevel1_step1");				
		}
	}
}
