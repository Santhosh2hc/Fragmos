using UnityEngine;
using System.Collections;

public class Online_ingame_csharp : MonoBehaviour {

	private float msg_timer2;
	int player_Score = 0;

	// Use this for initialization
	void Start () 
	{
		GooglePlayRTM.ActionDataRecieved += OnGCDataReceived;
	}
	
	// Update is called once per frame
	void Update () 
	{
		msg_timer2 = msg_timer2 + Time.deltaTime;	
		if(msg_timer2 > 0.25)
		{	player_Score = PlayerPrefs.GetInt ("player_Score");
			#if (UNITY_ANDROID && !UNITY_EDITOR) || SA_DEBUG_MODE
			string msg = player_Score.ToString();
			System.Text.UTF8Encoding  encoding = new System.Text.UTF8Encoding();
			byte[] data = encoding.GetBytes(msg);			
			GooglePlayRTM.instance.SendDataToAll(data, GP_RTM_PackageType.RELIABLE);
			#endif
			msg_timer2 = 0;
		}

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
			PlayerPrefs.SetInt("opp_Score", dataRcd);	
			
		}
		
		#endif
		
	}
}
