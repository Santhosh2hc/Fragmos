//#define VUNGLE_ENABLE

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class VungleAdProvider : AdProvider {
	
	public event Action<bool> OnInterstitialFinished = delegate {};
	public event Action<bool> OnInterstitialLoadComplete = delegate {};
	
	public event Action<bool> OnVideoFinished = delegate {};
	public event Action<bool> OnVideoLoadComplete = delegate {};
	


	//--------------------------------------
	// Initialization
	//--------------------------------------
	
	public void Init() {

		#if VUNGLE_ENABLE
		M_Logger.Log ("VungleAdProvider Init()");


		Vungle.onAdStartedEvent += onAdStartedEvent;
		Vungle.onAdEndedEvent += onAdEndedEvent;
		Vungle.onAdViewedEvent += onAdViewedEvent;
		Vungle.onCachedAdAvailableEvent += onCachedAdAvailableEvent;


		Vungle.init(M_Settings.Instance.Vungle_Android_ID, M_Settings.Instance.Vungle_IOS_ID);

		Debug.Log("Vungle intialized with IOS Id: " + M_Settings.Instance.Vungle_IOS_ID);
		Debug.Log("Vungle intialized with Android ID: " + M_Settings.Instance.Vungle_Android_ID);
		#endif

	}
	


	public void LoadVideo() {
		M_Logger.Log ("VungleAdProvider LoadVideo()");
		
		if(IsVideoReady) {
			OnVideoLoadComplete(true);
		}
	}	
	
	public void ShowVideo() {		
		M_Logger.Log ("VungleAdProvider ShowVideo()");

		#if VUNGLE_ENABLE
		Vungle.playAd();
		#endif
	}


	public bool IsVideoReady {
		get {
			#if VUNGLE_ENABLE
			return Vungle.isAdvertAvailable();
			#else
			return false;
			#endif
		}
	} 



	private void onAdEndedEvent() {
		Debug.Log("VungleAdProvider VideoFinished onAdEndedEvent()");

		OnVideoFinished(true);
	}
		

	private void onCachedAdAvailableEvent()	{
		Debug.Log("VungleAdProvider onCachedAdAvailableEvent");
		OnVideoLoadComplete(true);
	}








	private void onAdViewedEvent(double watched, double length) {
		Debug.Log("VungleAdProvider onAdViewedEvent. watched: " + watched + ", length: " + length);
	}
	
	
	private void onAdStartedEvent()	{
		Debug.Log("onAdStartedEvent");
	}





	public M_VideoProvider VideoProvider {
		get {
			return M_VideoProvider.Vungle;
		}
	}
	
	private List<RuntimePlatform> _AvaliablePlatfroms  = new List<RuntimePlatform>{ RuntimePlatform.Android, RuntimePlatform.IPhonePlayer};
	public List<RuntimePlatform> AvaliablePlatfroms {
		get {
			return _AvaliablePlatfroms;
		}
	}



	public bool IsInterstitialReady {
		get {
			return false;
		}
	}	

	public M_InterstitialProvider InterstitialProvider {
		get {
			return M_InterstitialProvider.None;
		}
	} 


	public void LoadInterstitial() {
	}
	
	public void ShowInterstitial() {
	}

}
