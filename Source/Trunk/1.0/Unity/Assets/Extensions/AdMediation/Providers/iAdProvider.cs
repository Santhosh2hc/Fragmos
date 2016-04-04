//#define iAd_ENABLED

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class iAdProvider  : AdProvider {

	public event Action<bool> OnInterstitialFinished = delegate {};
	public event Action<bool> OnInterstitialLoadComplete = delegate {};
	

	public event Action<bool> OnVideoFinished;
	public event Action<bool> OnVideoLoadComplete;

	private bool _IsInterstitialReady = false;



	public void Init() {
		#if  iAd_ENABLED

		iAdBannerController.InterstitialAdDidFinishAction += HandleOnInterstitialClosed;
		iAdBannerController.InterstitialAdDidLoadAction += HandleOnInterstitialLoaded;
		iAdBannerController.InterstitialDidFailWithErrorAction += HandleOnInterstitialFailedLoading;
		#endif
	}

	public void LoadInterstitial() {
		#if  iAd_ENABLED

		if(!_IsInterstitialReady) {
			iAdBannerController.Instance.LoadInterstitialAd();
		}
		#endif
	}

	public void ShowInterstitial() {
		#if  iAd_ENABLED
		if(_IsInterstitialReady) {
			_IsInterstitialReady = false;

			M_Logger.Log ("iAd ShowInterstitial");
			iAdBannerController.Instance.ShowInterstitialAd();
		}

		#endif
	}
	

	
	
	public void LoadVideo() {
		throw new NotImplementedException();
	}


	public void ShowVideo() {
		throw new NotImplementedException();
	}


	public bool IsInterstitialReady {
		get {
			return _IsInterstitialReady;
		}
	}
	
	public bool IsVideoReady {
		get {

			throw new NotImplementedException();
		}
	} 



	public M_InterstitialProvider InterstitialProvider {
		get {
			return M_InterstitialProvider.iAd;
		}
	} 

	public M_VideoProvider VideoProvider {
		get {
			return M_VideoProvider.None;
		}
	}



	private List<RuntimePlatform> _AvaliablePlatfroms  = new List<RuntimePlatform>{ RuntimePlatform.Android, RuntimePlatform.IPhonePlayer};
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
		M_Logger.Log("iAd: OnInterstitialLoaded");
		_IsInterstitialReady = true;
		OnInterstitialLoadComplete(true);
	}
	
	void HandleOnInterstitialClosed () {
		OnInterstitialFinished(true);
	}




}
