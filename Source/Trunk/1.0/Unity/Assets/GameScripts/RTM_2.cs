using UnityEngine;
using System.Collections;

public class RTM_2 : MonoBehaviour {
	
	int xUnit, yUnit, xyUnit, score = 0, a = 0;

	float msg_Timer, msg_timer2 = 0;

	bool isWin = false, isDataReceived = false, isNewLevel = false, isDisplayResult = false;
	int player_Score = 0, opp_Score = 0;

	public GUISkin moveSkin = null;

	void Start () 
	{   
		player_Score = PlayerPrefs.GetInt("Fragmos Player Score");
		//networking event
		GooglePlayRTM.ActionDataRecieved += OnGCDataReceived;
	}

	private void OnGCDataReceived(GP_RTM_Network_Package package) 
	{	
		GooglePlayRTM.ActionDataRecieved -= OnGCDataReceived;
		#if (UNITY_ANDROID && !UNITY_EDITOR ) || SA_DEBUG_MODE
		
		System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
		string str = enc.GetString(package.buffer);		

		opp_Score = System.Int32.Parse(str);

		isDataReceived = true;
		isDisplayResult = true; //If msg_timer is greater than or equal to 15 or if data received then make isDisplayResult as True
					
		#endif

	}

	void OnGUI() 
	{
		xUnit = Screen.width/5;
		yUnit = Screen.height/10;
		xyUnit = (xUnit + yUnit)/2;
		
		GUI.skin = moveSkin;
		moveSkin.label.fontSize = xUnit/4;

		GUI.Label(new Rect (Screen.width / 2 - xUnit / 2, Screen.height - yUnit / 2, xUnit * 2, yUnit), "Start New Game");

		if (isDisplayResult == false)
		{
			GUI.Label(new Rect(Screen.width/2 - xUnit/2, yUnit * 2, xUnit * 3, yUnit), "Waiting for the result...");
		}
		else
		{
			if(isDataReceived == false)
			{
				GUI.Label(new Rect(Screen.width/2 - xUnit/2, yUnit * 2, xUnit * 3, yUnit), "Opponent left the match...");
			}
			else
			{
				if (isWin == true)
				{
				GUI.Label(new Rect(Screen.width/2 - xUnit/2, yUnit * 2, xUnit * 3, yUnit), "You won..!");
				}
				else
				{
				GUI.Label(new Rect(Screen.width/2 - xUnit/2, yUnit * 2, xUnit * 3, yUnit), "You loose..!");
				}
				if (GUI.Button(new Rect (Screen.width / 2 - xUnit / 2, Screen.height - yUnit / 2, xUnit * 2, yUnit), ""))
				{
					isNewLevel = true;
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
			if((player_Score) > opp_Score)
			{
				isWin = true;
			}
		}

		if (msg_Timer < 15)
		{
			if(msg_timer2 > 0.25)
			{
				#if (UNITY_ANDROID && !UNITY_EDITOR) || SA_DEBUG_MODE
				string msg = player_Score.ToString();
				System.Text.UTF8Encoding  encoding = new System.Text.UTF8Encoding();
				byte[] data = encoding.GetBytes(msg);			
				GooglePlayRTM.instance.SendDataToAll(data, GP_RTM_PackageType.RELIABLE);
				#endif
				Debug.Log("playerscore"+player_Score.ToString());
				Debug.Log("oppscore"+opp_Score.ToString());
				msg_timer2 = 0;
			}
		}	
		else
		{
			isDisplayResult = true; //If msg_timer is greater than or equal to 15 or if data received then make isDisplayResult as True
		}

		if (isNewLevel == true) 
		{
			GooglePlayRTM.instance.LeaveRoom();
			gameObject.SendMessage ("openLevel0_step1");
		}
	}
}
