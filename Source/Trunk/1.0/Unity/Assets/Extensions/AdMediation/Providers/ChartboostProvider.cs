//#define CHARTBOOST_ENABLED

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


#if  CHARTBOOST_ENABLED
using ChartboostSDK;
#endif

public class ChartboostProvider  : AdProvider {

	public event Action<bool> OnInterstitialFinished 		= delegate {};
	public event Action<bool> OnInterstitialLoadComplete 	= delegate {};
	

	public event Action<bool> OnVideoFinished 		= delegate {};
	public event Action<bool> OnVideoLoadComplete 	= delegate {};

	#if  CHARTBOOST_ENABLED
	private bool _IsInitialized = false;
	#endif

	public void Init() {

		#if  CHARTBOOST_ENABLED


		if(_IsInitialized) {
			return;
		}

		_IsInitialized = true;
		// Listen to all impression-related events

		//Chartboost.Create();

		M_Logger.Log ("ChartboostProvider Init()");
		Chartboost.didFailToLoadInterstitial += didFailToLoadInterstitial;
		Chartboost.didDismissInterstitial += didDismissInterstitial;
		Chartboost.didCacheInterstitial += didCacheInterstitial;



		Chartboost.didFailToLoadRewardedVideo += didFailToLoadRewardedVideo;
		Chartboost.didDismissRewardedVideo += didDismissRewardedVideo;


		Chartboost.didCacheRewardedVideo += didCacheRewardedVideo;

		Chartboost.didCompleteRewardedVideo += didCompleteRewardedVideo;


		#endif
	}




	public void LoadInterstitial() {
		#if  CHARTBOOST_ENABLED

		if(!IsInterstitialReady) {
			M_Logger.Log ("Chartboost cacheInterstitial()");
			Chartboost.cacheInterstitial(CBLocation.Default);
		}
		#endif
	}

	public void ShowInterstitial() {
		#if  CHARTBOOST_ENABLED
		if(IsInterstitialReady) {
			Chartboost.showInterstitial(CBLocation.Default);
		}

		#endif
	}
	

	
	
	public void LoadVideo() {
		#if  CHARTBOOST_ENABLED
		if(!IsVideoReady) {
			M_Logger.Log ("Chartboost cacheRewardedVideo()");
			Chartboost.cacheRewardedVideo(CBLocation.Default);
		}
		#endif
	}


	public void ShowVideo() {
		#if  CHARTBOOST_ENABLED
		Chartboost.showRewardedVideo(CBLocation.Default);
		#endif
	}


	public bool IsInterstitialReady {
		get {
			#if  CHARTBOOST_ENABLED
			return Chartboost.hasInterstitial(CBLocation.Default);
			#else
			return false;
			#endif
		}
	}

	
	public bool IsVideoReady {
		get {
			#if  CHARTBOOST_ENABLED
			return Chartboost.hasRewardedVideo(CBLocation.Default);
			#else
			return false;
			#endif
		}
	} 


	public M_InterstitialProvider InterstitialProvider {
		get {
			return M_InterstitialProvider.Chartboost;
		}
	} 

	public M_VideoProvider VideoProvider {
		get {
			return M_VideoProvider.Chartboost;
		}
	}

	private List<RuntimePlatform> _AvaliablePlatfroms  = new List<RuntimePlatform>{RuntimePlatform.Android, RuntimePlatform.IPhonePlayer};
	public List<RuntimePlatform> AvaliablePlatfroms {
		get {
			return _AvaliablePlatfroms;
		}
	}


	//--------------------------------------
	// Event Handlers
	//--------------------------------------

	#if  CHARTBOOST_ENABLED

	void didCacheInterstitial (CBLocation location){
		OnInterstitialLoadComplete(true);
		M_Logger.Log("Chartboost: didCacheInterstitial");
	}	

	void didDismissInterstitial (CBLocation location) {
		OnInterstitialFinished(true);
		M_Logger.Log("Chartboost: didDismissInterstitial");
	}

	void didFailToLoadInterstitial (CBLocation location, CBImpressionError error) {
		OnInterstitialLoadComplete(false);
		M_Logger.Log(string.Format("Chartboost:  didFailToLoadInterstitial: {0} at location {1}", error, location));
	}

	



	void didDismissRewardedVideo (CBLocation location) {
		OnVideoFinished(true);
		M_Logger.Log("Chartboost: didDismissRewardedVideo");
	}
	
	void didCompleteRewardedVideo (CBLocation location, int arg2) {
		M_Logger.Log("Chartboost: didCompleteRewardedVideo");
	}



	void didFailToLoadRewardedVideo (CBLocation location, CBImpressionError error) {
		OnVideoLoadComplete(false);
		M_Logger.Log(string.Format("Chartboost: FailToLoad Rewarded Video: {0} at location {1}", error, location));
	}

	void didCacheRewardedVideo (CBLocation location	) {
		OnVideoLoadComplete(true);
		M_Logger.Log("Chartboost: didCacheRewardedVideo");
	}

	#endif
}
