using UnityEngine;
using System.Collections;

public class UnifiedAdsExample : MonoBehaviour {

	void Start () {
		AdMediation.Instance.OnVideoLoadComplete += HandleOnVideoLoadComplete;
		AdMediation.Instance.OnVideoFinished += HandleOnVideoFinished;
		AdMediation.Instance.OnVideoLeftApplication += HandleOnVideoLeftApplication;

		AdMediation.Instance.OnInterstitialLoadComplete += HandleOnInterstitialLoadComplete;
		AdMediation.Instance.OnInterstitialFinished += HandleOnInterstitialFinished;
		AdMediation.Instance.OnInterstitialLeftApplication += HandleOnInterstitialLeftApplication;

		AdMediation.Instance.Init ();
	}

	public void LoadVideoClick() {
		AdMediation.Instance.LoadVideo();
	}

	public void LoadInterstitialClick() {
		AdMediation.Instance.LoadInterstitial();
	}
	
	//--------------------------------------
	// Video Events
	//--------------------------------------
	

	private void HandleOnVideoFinished (M_VideoFinishResult res) {
		Debug.Log("Ad Mediation Example: Video Finished: " + res.AdProviderNetwork + " " + res.IsSucceeded);
	}
	
	private void HandleOnVideoLoadComplete (M_VideoLoadResult res) {
		Debug.Log("Ad Mediation Example: Video Loaded: " + res.AdProviderNetwork + " " + res.IsSucceeded);
		if(res.IsSucceeded) {
			AdMediation.Instance.ShowVideo();
		}
	}

	void HandleOnVideoLeftApplication (M_VideoLeftApplicationResult res)
	{
		Debug.Log("Ad Mediation Example: Video Left Application: " + res.AdProviderNetwork + " " + res.IsSucceeded);
	}

	//--------------------------------------
	// Interstitial Events
	//--------------------------------------

	private void HandleOnInterstitialFinished (M_InterstitialFinishResult res) {
		Debug.Log("Ad Mediation Example: Interstitial Finished: " + res.AdProviderNetwork + " " + res.IsSucceeded);

	}
	
	private void HandleOnInterstitialLoadComplete (M_InterstitialLoadResult res) {
		Debug.Log("Ad Mediation Example: Interstitial Loaded: " + res.AdProviderNetwork + " " + res.IsSucceeded);
		if(res.IsSucceeded) {
			AdMediation.Instance.ShowInterstitial();
		}
	}

	void HandleOnInterstitialLeftApplication (M_InterstitialLeftApplicationResult res)
	{
		Debug.Log("Ad Mediation Example: Interstitial Left Application: " + res.AdProviderNetwork + " " + res.IsSucceeded);
	}

}
