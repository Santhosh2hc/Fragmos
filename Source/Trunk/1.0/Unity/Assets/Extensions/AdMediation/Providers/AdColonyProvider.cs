#define ADCLONY_ENABLE

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class AdColonyProvider : MonoBehaviour, AdProvider {
	
	public event Action<bool> OnInterstitialFinished = delegate {};
	public event Action<bool> OnInterstitialLoadComplete = delegate {};
	
	public event Action<bool> OnVideoFinished = delegate {};
	public event Action<bool> OnVideoLoadComplete = delegate {};
	
	private bool _IsInterstitialReady = false;
	private bool _IsVideoReady = false;



	#if ADCLONY_ENABLE
	private string _zoneId = string.Empty;
	#endif
	
	//--------------------------------------
	// Initialization
	//--------------------------------------
	
	public static AdColonyProvider Create() {
		GameObject g = new GameObject ("AdColonyProvider");
		g.transform.parent = AdMediation.Instance.gameObject.transform;
		return g.AddComponent<AdColonyProvider>();

	}
	
	public void Init() {

		#if ADCLONY_ENABLE
		M_Logger.Log ("AdColonyProvider Init()");

		string appId = string.Empty;
		switch(Application.platform) {
		case RuntimePlatform.Android:
			appId = M_Settings.Instance.AdColony_Android_ID;
			_zoneId = M_Settings.Instance.AdColonyZone_Android;
			break;
		case RuntimePlatform.IPhonePlayer:
			appId = M_Settings.Instance.AdColony_IOS_ID;
			_zoneId = M_Settings.Instance.AdColonyZone_IOS;
			break;
		}

		AdColony.Configure(M_Settings.Instance.AppVersion, appId, _zoneId);

		AdColony.OnAdAvailabilityChange  += OnAdAvailabilityChange;
		AdColony.OnVideoStarted          += OnAdColonyVideoStarted;
		AdColony.OnVideoFinished         += OnAdColonyVideoFinished;
		AdColony.OnVideoFinishedWithInfo += OnAdColonyVideoFinishedWithInfo;

		#endif
	}
	
	void FixedUpdate() {
		if (!_IsVideoReady) {
			CheckVideoState();
		}
	}
	
	private void CheckVideoState() {
		#if ADCLONY_ENABLE
		if (AdColony.IsVideoAvailable (_zoneId)) {
			M_Logger.Log ("AdColonyProvider IsVideoAvailable " + AdColony.IsVideoAvailable (_zoneId));
			
			_IsVideoReady = true;
			OnVideoLoadComplete(true);
		}
		#endif
	}
	
	public void LoadInterstitial() {
	}
	
	public void ShowInterstitial() {
	}	
	
	public void LoadVideo() {
		M_Logger.Log ("AdColonyProvider LoadVideo()");
		
		CheckVideoState();
	}	
	
	public void ShowVideo() {		
		#if ADCLONY_ENABLE

		M_Logger.Log ("AdColonyProvider ShowVideo()");

		AdColony.ShowVideoAd(_zoneId);
		_IsVideoReady = false;
		#endif
	}

	public void OnAdAvailabilityChange(bool availability, string zoneId) {
		#if ADCLONY_ENABLE

		if(availability && _zoneId == zoneId) {
			Debug.Log("Zone is ready + " + AdColony.IsVideoAvailable (zoneId));
		}
		else {
			Debug.Log("Zone is not ready. Please config console");
		}
		#endif
	}

	public void OnAdColonyVideoStarted () {
		M_Logger.Log ("AdColonyProvider OnAdColonyVideoStarted()");
	}

	public void OnAdColonyVideoFinished (bool ad_shown) {
		M_Logger.Log ("AdColonyProvider OnAdColonyVideoFinished()+ shown " + ad_shown);

		OnVideoFinished(true);
	}

	#if ADCLONY_ENABLE

	public void OnAdColonyVideoFinishedWithInfo (AdColonyAd ad_shown) {
		M_Logger.Log ("AdColonyProvider OnAdColonyVideoFinishedWithInfo()  + shown " + ad_shown.shown);

		//OnVideoFinished(true);
	}
	#endif
	
	public bool IsInterstitialReady {
		get {
			return _IsInterstitialReady;
		}
	}	
	
	public bool IsVideoReady {
		get {
			return _IsVideoReady;
		}
		set {
			_IsVideoReady = value;
		}
	} 
	
	public M_InterstitialProvider InterstitialProvider {
		get {
			return M_InterstitialProvider.None;
		}
	} 
	
	public M_VideoProvider VideoProvider {
		get {
			return M_VideoProvider.AdColony;
		}
	}

	private List<RuntimePlatform> _AvaliablePlatfroms  = new List<RuntimePlatform>{ RuntimePlatform.Android, RuntimePlatform.IPhonePlayer};
	public List<RuntimePlatform> AvaliablePlatfroms {
		get {
			return _AvaliablePlatfroms;
		}
	}
}
