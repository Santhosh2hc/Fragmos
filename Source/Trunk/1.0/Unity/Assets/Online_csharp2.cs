using UnityEngine;
using System.Collections;

public class Online_csharp2 : MonoBehaviour {
	
	int xUnit, yUnit, xyUnit, score = 0, a = 0;
	
	float msg_Timer, msg_timer2 = 0;
	
	bool isDataReceived = false, isDisplayResult = false;
	int player_Score = 0, opp_Score = 0, handShake = 10001, sent = 0, received = 0, isWin = 0;
	
	public GUISkin infoSkin = null;

	
	void Start () 
	{   
		player_Score = PlayerPrefs.GetInt("player_Score");
		opp_Score = PlayerPrefs.GetInt ("opp_Score");
		//networking event
		GooglePlayRTM.ActionDataRecieved += OnGCDataReceived;
	}
	
	private void OnGCDataReceived(GP_RTM_Network_Package package) 
	{	
		
		int dataRcd = 0;
		#if (UNITY_ANDROID && !UNITY_EDITOR ) || SA_DEBUG_MODE
		
		System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
		string str = enc.GetString(package.buffer);		
		dataRcd = System.Int32.Parse(str);
		if (dataRcd < 10000) //If data is less than 10000 then it is valid data. Data above 10000 are used for handshake mechanisms
		{
			opp_Score = dataRcd;		
		}
		else
		{	
			isDisplayResult = true; //If msg_timer is greater than or equal to 15 or if data received then make isDisplayResult as True
			isDataReceived = true;
		}
		#endif
		
	}
	
	void OnGUI() 
	{
		xUnit = Screen.width/5;
		yUnit = Screen.height/10;
		xyUnit = (xUnit + yUnit)/2;
		
		GUI.skin = infoSkin;
		infoSkin.label.fontSize = xUnit/4;
				
		if (isDisplayResult == false)
		{
			GUI.Label(new Rect(Screen.width/2 - xUnit * 4/2, yUnit * 2, xUnit * 4, yUnit), "Waiting for result...");
		}
		else
		{
			GooglePlayRTM.instance.LeaveRoom();
			if(isDataReceived == false)
			{
				GUI.Label(new Rect(Screen.width/2 - xUnit * 4/2, yUnit * 2, xUnit * 4, yUnit), "Opponent Left");
			}
			else
			{
				GUI.Label(new Rect(Screen.width/2 - xUnit * 4/2, yUnit * 2, xUnit * 4, yUnit), "You   " + player_Score + "        Opponent   " + opp_Score);
				if (isWin == 1)
				{
					GUI.Label(new Rect(Screen.width/2 - xUnit * 4/2, yUnit * 3f, xUnit * 4, yUnit), "You Won!");
				}
				else if (isWin == 2)
				{
					GUI.Label(new Rect(Screen.width/2 - xUnit * 4/2, yUnit * 3f, xUnit * 4, yUnit), "Draw!!");
				}
				else
				{
					GUI.Label(new Rect(Screen.width/2 - xUnit * 4/2, yUnit * 3f, xUnit * 4, yUnit), "They Won!");
				}				
			}
		}
	}
	
	void FixedUpdate()
	{
		msg_Timer = msg_Timer + Time.deltaTime;
		msg_timer2 = msg_timer2 + Time.deltaTime;
		
		if (isDataReceived == true)
		{
			if(player_Score > opp_Score)
			{
				isWin = 1;
			}
			else if(player_Score == opp_Score)
			{
				isWin = 2;
			}
			else
			{
				isWin = 3;
			}
		}
		
		if (msg_Timer < 30)
		{
			if(msg_timer2 > 0.25)
			{
				#if (UNITY_ANDROID && !UNITY_EDITOR) || SA_DEBUG_MODE

				string msg;
				if (msg_Timer < 2)
				{
					msg = player_Score.ToString();
				}
				else
				{
					msg = handShake.ToString();
				}

				System.Text.UTF8Encoding  encoding = new System.Text.UTF8Encoding();
				byte[] data = encoding.GetBytes(msg);			
				GooglePlayRTM.instance.SendDataToAll(data, GP_RTM_PackageType.RELIABLE);
				#endif
				msg_timer2 = 0;
			}
		}	
		else
		{
			isDisplayResult = true; //If msg_timer is greater than or equal to 15 or if data received then make isDisplayResult as True
		}		

	}
}
