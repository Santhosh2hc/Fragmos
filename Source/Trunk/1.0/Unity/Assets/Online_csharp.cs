using UnityEngine;
using System.Collections;

public class Online_csharp : MonoBehaviour {
	
	int xUnit, yUnit, xyUnit, score = 0, a = 0;
	public GUISkin infoSkin = null;
	public GUISkin greylineSkin = null;
	public GUISkin titleSkin = null;
	public GUISkin purpleButtonSkin = null;

	private bool isPlayerConnected = false;
	private bool isPlayerDisconnected = false;
		
	void Start () 
	{   
		isPlayerConnected = false;
		isPlayerDisconnected = false;
		//listen for GooglePlayConnection events	
		GooglePlayConnection.ActionPlayerConnected +=  OnPlayerConnected;
		GooglePlayConnection.ActionPlayerDisconnected += OnPlayerDisconnected;		
		GooglePlayConnection.ActionConnectionResultReceived += OnConnectionResult;		
		
		if(GooglePlayConnection.State == GPConnectionState.STATE_CONNECTED) 
		{
			//checking if player already connected
			OnPlayerConnected ();
		} 
		
		GooglePlayConnection.instance.Connect ();
	}
	
	private void OnPlayerConnected() 
	{
		isPlayerConnected = true;
		findMatch();
	}
	
	private void findMatch() 
	{
		int minPlayers = 1;
		int maxPlayers = 2;
		
		GooglePlayRTM.instance.FindMatch(minPlayers, maxPlayers);
	}
	
	private void OnConnectionResult(GooglePlayConnectionResult result) 
	{
		Debug.Log(result.code.ToString());
	}
	
	private void OnPlayerDisconnected() 
	{
		isPlayerDisconnected = true;
	}
	
	void OnGUI() 
	{
		xUnit = Screen.width/5;
		yUnit = Screen.height/10;
		xyUnit = (xUnit + yUnit)/2;
		
		GUI.skin = infoSkin;
		infoSkin.label.fontSize = xUnit/4;
		if (isPlayerDisconnected == true) 
		{
			GUI.Label (new Rect (Screen.width / 2 - xUnit * 3/2, yUnit * 4, xUnit * 3, yUnit), "Connection Lost!");
		} 
		else 
		{
			GUI.Label (new Rect (Screen.width / 2 - xUnit * 4/2, yUnit * 4, xUnit * 4, yUnit), "SEARCHING FOR OPPONENT...");
		}
		GUI.skin = greylineSkin;
		GUI.Label(new Rect(xUnit/2, yUnit, xUnit * 4, 2), "");
		GUI.Label(new Rect(xUnit/2, yUnit * 9, xUnit * 4, 2), "");
		
		GUI.skin = titleSkin;
		titleSkin.label.fontSize = xUnit/2;
		GUI.Label(new Rect((Screen.width/2) - (xUnit * 3f/2f), yUnit/3.8f, xUnit * 3, xUnit/2), "ONLINE");

		GUI.skin = purpleButtonSkin;
		purpleButtonSkin.label.fontSize = xUnit/4;
		if (GUI.Button(new Rect(Screen.width - 3.5f * xUnit, Screen.height/2f + 0.75f * xyUnit, 2 * xUnit, 0.6f * xyUnit), ""))
		{
			Application.LoadLevel("MainMenu");
		}
		GUI.Label(new Rect(Screen.width - 3.5f * xUnit, Screen.height/2f + 0.75f * xyUnit, 2 * xUnit, 0.6f* xyUnit), "Exit to Menu");

	}
	
	void Update()
	{

		if (isPlayerConnected == true) 
		{	
			isPlayerDisconnected = false;
		}
		if(GooglePlayRTM.instance.currentRoom.status == GP_RTM_RoomStatus.ROOM_STATUS_ACTIVE) 
		{
			gameObject.SendMessage ("openLevel1");				
		}
	}
}
