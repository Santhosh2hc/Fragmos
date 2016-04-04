using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public interface AdProvider  {


	event Action<bool> OnInterstitialFinished;
	event Action<bool> OnInterstitialLoadComplete;


	event Action<bool> OnVideoFinished;
	event Action<bool> OnVideoLoadComplete;

	

	void Init();

	void LoadInterstitial();
	void ShowInterstitial();


	bool IsInterstitialReady {get;}


	void LoadVideo();
	void ShowVideo();

	bool IsVideoReady {get;} 

	M_InterstitialProvider InterstitialProvider {get;} 
	M_VideoProvider VideoProvider {get;} 
	List<RuntimePlatform> AvaliablePlatfroms {get;}
}
