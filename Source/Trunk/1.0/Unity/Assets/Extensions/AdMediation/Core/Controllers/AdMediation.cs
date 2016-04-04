using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class AdMediation : SA_Singleton<AdMediation> {

	public event Action<M_InterstitialFinishResult> 			OnInterstitialFinished 			= delegate {};
	public event Action<M_InterstitialLoadResult> 				OnInterstitialLoadComplete 		= delegate {};
	public event Action<M_InterstitialLeftApplicationResult> 	OnInterstitialLeftApplication 	= delegate{};

	public event Action<M_VideoFinishResult> 			OnVideoFinished 		= delegate {};
	public event Action<M_VideoLoadResult> 				OnVideoLoadComplete 	= delegate {};
	public event Action<M_VideoLeftApplicationResult> 	OnVideoLeftApplication 	= delegate{};

	private List<AdProvider> _VideoProviders =  new List<AdProvider>();
	private List<AdProvider> _InterstitialProviders =  new List<AdProvider>();

	private AdProvider _CurrentVideoProvider =  null;
	private AdProvider _CurrentInterstitialProvider = null;

	private M_AdLoadState _InterstitialLoadState = M_AdLoadState.Undefined;
	private M_AdLoadState _VideoLoadState = M_AdLoadState.Undefined;

	//--------------------------------------
	// Initialization
	//--------------------------------------

	void Awake() {
		DontDestroyOnLoad (gameObject);
	}

	public void Init() {
		M_MediationProfile p = null;
		foreach(M_MediationProfile profile in M_Settings.Instance.Profiles) {
			if(profile.Platform.Equals(Application.platform)) {
				p = profile;
				break;
			}
		}

		Init(p);
	}

	public void Init(string mediationProfileId) {
		M_MediationProfile p = null;

		foreach(M_MediationProfile profile in M_Settings.Instance.Profiles) {
			if(profile.Id.Equals(mediationProfileId)) {
				p = profile;
				break;
			}
		}

		Init(p);
	}

	private void Init(M_MediationProfile profile) {

		if(IsEditorTestingEnabled) {

			M_Logger.Log("Initialized with Editor Testing Profile");
			SA_EditorAd.Instance.SetFillRate(M_Settings.Instance.EditorFillRate);
			return;
		}


		if(profile == null) {
			Debug.LogWarning("Ad Mediation: initialization is failed with null profile");
			return;
		}

		foreach(M_InterstitialProvider p in profile.InterstitialProviders) {
			DefineInterstitialProvider(p);
		}

		foreach(M_VideoProvider p in profile.VideoProviders) {
			DefineVideoProvider(p);
		}

		if(_VideoProviders.Count > 0) {
			_CurrentVideoProvider = _VideoProviders[0];
		}

		if(_InterstitialProviders.Count > 0) {
			_CurrentInterstitialProvider = _InterstitialProviders[0];
		}


		M_Logger.Log(_VideoProviders.Count		 + " Video Ad Nextworks registred");
		M_Logger.Log(_InterstitialProviders.Count + " Interstitial Ad Nextworks registred");

	}

	//--------------------------------------
	// Public Method
	//--------------------------------------

	public void LoadInterstitial() {
		M_Logger.Log("_InterstitialLoadState " + _InterstitialLoadState);
		if(_InterstitialLoadState == M_AdLoadState.Undefined || _InterstitialLoadState == M_AdLoadState.Failed) {
			StartInterstitialRequiest();
		}
	}

	public void ShowInterstitial() {
		if(_InterstitialLoadState == M_AdLoadState.Loaded) {

			if(IsEditorTestingEnabled) {
				SA_EditorAd.OnInterstitialLeftApplication += HandleOnInterstitialLeftApplication_Editor;
				SA_EditorAd.OnInterstitialFinished += HandleOnInterstitialFinished_Editor;
				SA_EditorAd.Instance.ShowInterstitial();
			} else {
				_CurrentInterstitialProvider.OnInterstitialFinished += HandleOnInterstitialFinished;
				_CurrentInterstitialProvider.ShowInterstitial();
			}


		}
	}

	public void LoadVideo() {
		M_Logger.Log("_VideoLoadState " + _VideoLoadState);
		if(_VideoLoadState == M_AdLoadState.Undefined || _VideoLoadState == M_AdLoadState.Failed) {
			StartVideoRequest();
		}
	}

	public void ShowVideo() {
		M_Logger.Log ("_VideoLoadState " + _VideoLoadState);
		if(_VideoLoadState == M_AdLoadState.Loaded) {
			if (IsEditorTestingEnabled) {
				SA_EditorAd.OnVideoLeftApplication += HandleOnVideoLeftApplication_Editor;
				SA_EditorAd.OnVideoFinished += HandleOnVideoFinished_Editor;
				SA_EditorAd.Instance.ShowVideo();
			} else {
				_CurrentVideoProvider.OnVideoFinished += HandleOnVideoFinished;
				_CurrentVideoProvider.ShowVideo();
			}
		}
	}

	//--------------------------------------
	// Get / Set
	//--------------------------------------


	
	public bool IsInterstitialReady {
		get {
			return _InterstitialLoadState == M_AdLoadState.Loaded;
		}
	}

	
	public bool IsVideoReady  {
		get {
			return _VideoLoadState == M_AdLoadState.Loaded;
		}
	}

	public bool IsEditorTestingEnabled {
		get {
			return SA_EditorTesting.IsInsideEditor && M_Settings.Instance.IsEditorTestingEnabled;
		}
	}



	//--------------------------------------
	// Event Handlers
	//--------------------------------------

	void HandleOnVideoFinished_Editor (bool IsSucceeded)
	{
		SA_EditorAd.OnVideoLeftApplication -= HandleOnVideoLeftApplication_Editor;
		SA_EditorAd.OnVideoFinished -= HandleOnVideoFinished_Editor;
		_VideoLoadState = M_AdLoadState.Undefined;
		
		M_VideoFinishResult result;
		if(IsSucceeded) {
			result = new M_VideoFinishResult(M_VideoProvider.None);
		} else {
			M_Error error = new M_Error(0, "Internal error");
			result = new M_VideoFinishResult(M_VideoProvider.None, error);
		}
		
		OnVideoFinished(result);
	}

	void HandleOnVideoFinished (bool IsSucceeded) {
		_CurrentVideoProvider.OnVideoFinished -= HandleOnVideoFinished;
		_VideoLoadState = M_AdLoadState.Undefined;
		
		M_VideoFinishResult result;
		if(IsSucceeded) {
			result = new M_VideoFinishResult(_CurrentVideoProvider.VideoProvider);
		} else {
			M_Error error = new M_Error(0, "Internal error");
			result = new M_VideoFinishResult(_CurrentVideoProvider.VideoProvider, error);
		}
		
		OnVideoFinished(result);
	}

	void HandleOnVideoLoadTimeOut() {
		M_Logger.Log ("AdMediation Video Load TimeOut");
		if(_VideoLoadState == M_AdLoadState.InProgress) {
			HandleOnVideoLoadComplete(false);
		}

	}

	void HandleOnVideoLoadComplete(bool IsLoaded) {

		CancelInvoke("HandleOnVideoLoadTimeOut");

		_CurrentVideoProvider.OnVideoLoadComplete -= HandleOnVideoLoadComplete;
		M_VideoLoadResult result;

		M_Logger.Log ("AdMediation HandleOnVideoLoadComplete " + IsLoaded);
		if(IsLoaded) {
			_VideoLoadState = M_AdLoadState.Loaded;
			result = new M_VideoLoadResult(_CurrentVideoProvider.VideoProvider);
			OnVideoLoadComplete(result);
		} else {



			int index = _VideoProviders.IndexOf(_CurrentVideoProvider);
			index++;
			if(_VideoProviders.Count > index) {
				_CurrentVideoProvider = _VideoProviders[index];
				StartVideoRequest();
			} else {
				_VideoLoadState = M_AdLoadState.Failed;
				M_Error error = new M_Error(0, "Internal error");
				result =  new M_VideoLoadResult(_CurrentVideoProvider.VideoProvider, error);
				
				if(_VideoProviders.Count > 0) {
					_CurrentVideoProvider = _VideoProviders[0];
				}
				
				OnVideoLoadComplete(result);
			}
		}
	}

	void HandleOnVideoLeftApplication_Editor ()
	{
		M_VideoLeftApplicationResult result = new M_VideoLeftApplicationResult(M_VideoProvider.None);
		OnVideoLeftApplication(result);
	}

	void HandleOnVideoLoadComplete_Editor (bool IsLoaded)
	{
		M_VideoLoadResult result;
		SA_EditorAd.OnVideoLoadComplete -= HandleOnVideoLoadComplete_Editor;
		
		if(IsLoaded) {
			_VideoLoadState = M_AdLoadState.Loaded;
			result =  new M_VideoLoadResult(M_VideoProvider.None);
			
		} else {
			_VideoLoadState = M_AdLoadState.Failed;
			M_Error error = new M_Error(0, "Internal error");
			result =  new M_VideoLoadResult(M_VideoProvider.None, error);
		}
		
		OnVideoLoadComplete(result);
	}
	

	void HandleOnInterstitialFinished (bool IsSucceeded) {

		_CurrentInterstitialProvider.OnInterstitialFinished -= HandleOnInterstitialFinished;
		_InterstitialLoadState = M_AdLoadState.Undefined;

		M_InterstitialFinishResult result;
		if(IsSucceeded) {
			result =  new M_InterstitialFinishResult(_CurrentInterstitialProvider.InterstitialProvider);
		} else {
			M_Error error = new M_Error(0, "Internal error");
			result =  new M_InterstitialFinishResult(_CurrentInterstitialProvider.InterstitialProvider, error);
		}

		OnInterstitialFinished(result);
	}

	void HandleOnInterstitialLeftApplication_Editor ()
	{
		M_InterstitialLeftApplicationResult result = new M_InterstitialLeftApplicationResult(M_InterstitialProvider.None);
		OnInterstitialLeftApplication(result);
	}

	void HandleOnInterstitialFinished_Editor (bool IsSucceeded) {
		SA_EditorAd.OnInterstitialLeftApplication -= HandleOnInterstitialLeftApplication_Editor;
		SA_EditorAd.OnInterstitialFinished -= HandleOnInterstitialFinished_Editor;
		_InterstitialLoadState = M_AdLoadState.Undefined;

		M_InterstitialFinishResult result;
		if(IsSucceeded) {
			result =  new M_InterstitialFinishResult(M_InterstitialProvider.None);
		} else {
			M_Error error = new M_Error(0, "Internal error");
			result =  new M_InterstitialFinishResult(M_InterstitialProvider.None, error);
		}

		OnInterstitialFinished(result);
	}


	void HandleOnInterstitialLoadTimeOut() {
		M_Logger.Log ("AdMediation Interstitial Load TimeOut");
		if(_InterstitialLoadState == M_AdLoadState.InProgress) {
			HandleOnInterstitialLoadComplete(false);
		}
	}

	
	void HandleOnInterstitialLoadComplete (bool IsLoaded) 	{
		M_Logger.Log ("AdMediation Handle On Interstitial Load Complete " + IsLoaded);

		CancelInvoke("HandleOnInterstitialLoadTimeOut");

		_CurrentInterstitialProvider.OnInterstitialLoadComplete -= HandleOnInterstitialLoadComplete;
		M_InterstitialLoadResult result;

		if(IsLoaded) {
			_InterstitialLoadState = M_AdLoadState.Loaded;
			result =  new M_InterstitialLoadResult(_CurrentInterstitialProvider.InterstitialProvider);
			OnInterstitialLoadComplete(result);
		} else {

			int index = _InterstitialProviders.IndexOf(_CurrentInterstitialProvider);
			index++;
			if(_InterstitialProviders.Count > index) {
				_CurrentInterstitialProvider = _InterstitialProviders[index];
				StartInterstitialRequiest();

			} else {
				_InterstitialLoadState = M_AdLoadState.Failed;
				M_Error error = new M_Error(0, "Internal error");
				result =  new M_InterstitialLoadResult(_CurrentInterstitialProvider.InterstitialProvider, error);

				if(_InterstitialProviders.Count > 0) {
					_CurrentInterstitialProvider = _InterstitialProviders[0];
				}

				OnInterstitialLoadComplete(result);
			}

		}
	}

	void HandleOnInterstitialLoadComplete_Editor (bool res) {
		M_InterstitialLoadResult result;
		SA_EditorAd.OnInterstitialLoadComplete -= HandleOnInterstitialLoadComplete_Editor;

		if(res) {
			_InterstitialLoadState = M_AdLoadState.Loaded;
			result =  new M_InterstitialLoadResult(M_InterstitialProvider.None);

		} else {
			_InterstitialLoadState = M_AdLoadState.Failed;
			M_Error error = new M_Error(0, "Internal error");
			result =  new M_InterstitialLoadResult(M_InterstitialProvider.None, error);
		}

		OnInterstitialLoadComplete(result);
	}


	//--------------------------------------
	// Private Method
	//--------------------------------------

	private void StartVideoRequest() {
		_VideoLoadState = M_AdLoadState.InProgress;

		if (IsEditorTestingEnabled) {
			SA_EditorAd.OnVideoLoadComplete += HandleOnVideoLoadComplete_Editor;
			SA_EditorAd.Instance.LoadVideo();
			return;
		}
		
		if(_CurrentVideoProvider  != null) {
			if(_CurrentVideoProvider.AvaliablePlatfroms.Contains(Application.platform) && _CurrentVideoProvider.VideoProvider != M_VideoProvider.None) {
				M_Logger.Log ("Start Video Request with " + _CurrentVideoProvider.VideoProvider);

				_CurrentVideoProvider.OnVideoLoadComplete += HandleOnVideoLoadComplete;
				_CurrentVideoProvider.LoadVideo();


				Invoke("HandleOnVideoLoadTimeOut", M_Settings.Instance.VideoLoadTimeOut);

				return;
			}
		} else {
			M_Logger.Log("No Video Provider");
		}
	}
	
	private void StartInterstitialRequiest() {
		_InterstitialLoadState = M_AdLoadState.InProgress;

		if(IsEditorTestingEnabled) {
			SA_EditorAd.OnInterstitialLoadComplete += HandleOnInterstitialLoadComplete_Editor;
			SA_EditorAd.Instance.LoadInterstitial();
			return;
		}
		
		if(_CurrentInterstitialProvider  != null) {
			if(_CurrentInterstitialProvider.AvaliablePlatfroms.Contains(Application.platform) && _CurrentInterstitialProvider.InterstitialProvider != M_InterstitialProvider.None) {
				M_Logger.Log ("Start Interstitial Request with " + _CurrentInterstitialProvider.InterstitialProvider);

				_CurrentInterstitialProvider.OnInterstitialLoadComplete += HandleOnInterstitialLoadComplete;
				_CurrentInterstitialProvider.LoadInterstitial();


				Invoke("HandleOnInterstitialLoadTimeOut", M_Settings.Instance.InterstisialLoadTimeOut);

				return;
			}
		} else {
			M_Logger.Log("No Interstitial Provider");
		}
	}




	private void DefineInterstitialProvider(M_InterstitialProvider providerType) {
		AdProvider provider = null;

		switch(providerType) {
			case M_InterstitialProvider.AdMob:
				provider =  new AdMobProvider();
				break;
			case M_InterstitialProvider.Chartboost:
				provider =  new ChartboostProvider();
				break;
			case M_InterstitialProvider.iAd:
				provider =  new iAdProvider();
				break;
		}

		if(provider != null) {
			_InterstitialProviders.Add(provider);
			provider.Init();
		}
	}

	private void DefineVideoProvider(M_VideoProvider providerType) {
		AdProvider provider = null;

		switch(providerType) {
			case M_VideoProvider.UnityAds:
				provider = UnityAdsProvider.Create();
				break;
			case M_VideoProvider.AdColony:
				provider = new AdColonyProvider();
				break;
			case M_VideoProvider.Vungle:
				provider = new VungleAdProvider();
				break;
			case M_VideoProvider.Chartboost:
				provider =  new ChartboostProvider();
				break;
		}

		if(provider != null) {
			_VideoProviders.Add(provider);
			provider.Init();
		}
	}

}
