//#define ADMOB_ENABLED

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AdMobProvider  : AdProvider {

	public event Action<bool> OnInterstitialFinished;
	public event Action<bool> OnInterstitialLoadComplete;
	

	public event Action<bool> OnVideoFinished;
	public event Action<bool> OnVideoLoadComplete;

	private bool _IsInterstitialReady = false;



	public void Init() {

		#if  ADMOB_ENABLED

		GoogleMobileAd.Init ();

		GoogleMobileAd.OnInterstitialClosed += HandleOnInterstitialClosed;
		GoogleMobileAd.OnInterstitialLoaded += HandleOnInterstitialLoaded;
		GoogleMobileAd.OnInterstitialFailedLoading += HandleOnInterstitialFailedLoading;

		#endif
	}




	public void LoadInterstitial() {
		#if  ADMOB_ENABLED

		if(!_IsInterstitialReady) {
			GoogleMobileAd.LoadInterstitialAd();
		}
		#endif
	}

	public void ShowInterstitial() {
		#if  ADMOB_ENABLED
		if(_IsInterstitialReady) {
			_IsInterstitialReady = false;
			GoogleMobileAd.ShowInterstitialAd();
		}

		#endif
	}
	

	
	
	public void LoadVideo() {

	}


	public void ShowVideo() {

	}


	public bool IsInterstitialReady {
		get {
			return _IsInterstitialReady;
		}
	}

	
	public bool IsVideoReady {
		get {
			return true;
		}
	} 


	public M_InterstitialProvider InterstitialProvider {
		get {
			return M_InterstitialProvider.AdMob;
		}
	} 

	public M_VideoProvider VideoProvider {
		get {
			return M_VideoProvider.UnityAds;
		}
	}

	private List<RuntimePlatform> _AvaliablePlatfroms  = new List<RuntimePlatform>{RuntimePlatform.Android, RuntimePlatform.IPhonePlayer, RuntimePlatform.WP8Player};
	public List<RuntimePlatform> AvaliablePlatfroms {
		get {
			return _AvaliablePlatfroms;
		}
	}


	//--------------------------------------
	// Event Handlers
	//--------------------------------------
	
	void HandleOnInterstitialFailedLoading () {
		_IsInterstitialReady = false;
		OnInterstitialLoadComplete(false);
	}
	
	void HandleOnInterstitialLoaded () {
		_IsInterstitialReady = true;
		OnInterstitialLoadComplete(true);
	}
	
	void HandleOnInterstitialClosed () {
		OnInterstitialFinished(true);
	}




}
