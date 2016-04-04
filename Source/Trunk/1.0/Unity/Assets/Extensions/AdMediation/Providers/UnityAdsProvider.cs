#define UNITY_ADS_ENABLE

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_ADS_ENABLE
using UnityEngine.Advertisements;
#endif


public class UnityAdsProvider : MonoBehaviour, AdProvider {
	
	public event Action<bool> OnInterstitialFinished = delegate {};
	public event Action<bool> OnInterstitialLoadComplete = delegate {};
		
	public event Action<bool> OnVideoFinished = delegate {};
	public event Action<bool> OnVideoLoadComplete = delegate {};
	
	private bool _IsInterstitialReady = false;
	private bool _IsVideoReady = false;



	#if UNITY_ADS_ENABLE
	private ShowOptions options;
	#endif

	//--------------------------------------
	// Initialization
	//--------------------------------------

	public static UnityAdsProvider Create() {
		GameObject g = new GameObject ("UnityAdsProvider");
		g.transform.parent = AdMediation.Instance.gameObject.transform;
		return g.AddComponent<UnityAdsProvider>();
	}

	public void Init() {
		#if UNITY_ADS_ENABLE

		string appId = string.Empty;
		switch(Application.platform) {
		case RuntimePlatform.Android:
			appId = M_Settings.Instance.UnityAds_Android_ID;
			break;
		case RuntimePlatform.IPhonePlayer:
			appId = M_Settings.Instance.UnityAds_IOS_ID;
			break;
		}

		M_Logger.Log ("UnityAdsProvider Init with appId: " + appId);

		Advertisement.Initialize (appId, false);
		#endif
	}
	
	void FixedUpdate() {
		if (!_IsVideoReady) {
			OnVideoLoadEvent();
		}
	}

	private void OnVideoLoadEvent() {
		#if UNITY_ADS_ENABLE

		if(Advertisement.IsReady()) {
			M_Logger.Log ("UnityAdsProvider Advertisement.IsReady()");

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
		M_Logger.Log ("UnityAdsProvider LoadVideo()");

		OnVideoLoadEvent();
	}	
	
	public void ShowVideo() {		
		#if UNITY_ADS_ENABLE
		M_Logger.Log ("UnityAdsProvider ShowVideo()");

		options = new ShowOptions();
		options.resultCallback += VideoFinished;

		Advertisement.Show(null, options);
		_IsVideoReady = false;
		#endif
	}


	#if UNITY_ADS_ENABLE
	private void VideoFinished(ShowResult result) {
		M_Logger.Log ("UnityAdsProvider VideoFinished() with result " + result.ToString());

		options.resultCallback -= VideoFinished;
		OnVideoFinished(true);
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
			return M_VideoProvider.UnityAds;
		}
	}

	private List<RuntimePlatform> _AvaliablePlatfroms  = new List<RuntimePlatform>{ RuntimePlatform.Android, RuntimePlatform.IPhonePlayer};
	public List<RuntimePlatform> AvaliablePlatfroms {
		get {
			return _AvaliablePlatfroms;
		}
	}
}
