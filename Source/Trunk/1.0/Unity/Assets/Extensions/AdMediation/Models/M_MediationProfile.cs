using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class M_MediationProfile  {


	//for Editor only
	public bool IsOpen = true;
	public bool IsVideoNetworksOpen = true;
	public bool IsInterstisialNetworksOpen = true;



	public string Id =  "New Profile";

	public RuntimePlatform Platform 	=  RuntimePlatform.Android;

	public List<M_VideoProvider> VideoProviders =  new List<M_VideoProvider>();
	public List<M_InterstitialProvider> InterstitialProviders =  new List<M_InterstitialProvider>();
}
