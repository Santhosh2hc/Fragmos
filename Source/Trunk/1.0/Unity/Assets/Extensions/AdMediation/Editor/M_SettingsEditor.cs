using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;


[CustomEditor(typeof(M_Settings))]
public class M_SettingsEditor : Editor {

	GUIContent SdkVersion   = new GUIContent("Plugin Version [?]", "This is Plugin version.  If you have problems or compliments please include this so we know exactly what version to look out for.");

	private static int[] rates = new int[]{0, 20, 50, 80, 100};

	private static string AdClony_Path 			= "Extensions/AdMediation/Providers/AdColonyProvider.cs";
	private static string UnityAds_Path 		= "Extensions/AdMediation/Providers/UnityAdsProvider.cs";
	private static string GoogleMobileAd_Path 	= "Extensions/AdMediation/Providers/AdMobProvider.cs";
	private static string M_AdMobEditor_Path 	= "Extensions/AdMediation/Editor/M_AdMobEditor.cs";

	private static string Vungle_Path 			= "Extensions/AdMediation/Providers/VungleAdProvider.cs";
	private static string iAdPorvider_Path 			= "Extensions/AdMediation/Providers/iAdProvider.cs";

	private static string CBProvider_Path 			= "Extensions/AdMediation/Providers/ChartboostProvider.cs";
	private static string CBSettings_Path 			= "Extensions/AdMediation/Editor/M_CB_Settings.cs";


	private string[] ToolbarStrings = new string[] {"Profiles", "Ad Networks", "Settings"};

	void Awake() {
		#if !UNITY_WEBPLAYER
		UpdatePluginSettings();
		#endif
	}

	public override void OnInspectorGUI () {
		

		GUI.changed = false;
		
		
		M_Settings.Instance.ToolbarIndex = GUILayout.Toolbar(M_Settings.Instance.ToolbarIndex, ToolbarStrings);
		
		
		switch(M_Settings.Instance.ToolbarIndex) {
		case 0:
			Profiles();
			AboutGUI();
			break;
		case 1:
			AdNetworks();
			break;
		case 2:
			Settings();
			break;
		}
		
		if(GUI.changed) {
			DirtyEditor();
		}
		
	}

	
	public static void UpdatePluginSettings() {
	
		SA_EditorTool.ChnageDefineState(AdClony_Path, 			"ADCLONY_ENABLE", 			M_Settings.Instance.AdColonyEnabled);
		SA_EditorTool.ChnageDefineState(UnityAds_Path, 			"UNITY_ADS_ENABLE", 		M_Settings.Instance.UnityAdsEnabled);

		SA_EditorTool.ChnageDefineState(GoogleMobileAd_Path, 		"ADMOB_ENABLED", 			M_Settings.Instance.GoogleMobileAdEnabled);
		SA_EditorTool.ChnageDefineState(M_AdMobEditor_Path, 		"ADMOB_ENABLED", 			M_Settings.Instance.GoogleMobileAdEnabled);

		SA_EditorTool.ChnageDefineState(Vungle_Path, 				"VUNGLE_ENABLE", 			M_Settings.Instance.VungleEnabled);
		SA_EditorTool.ChnageDefineState(iAdPorvider_Path, 		"iAd_ENABLED", 				M_Settings.Instance.iAdEnabled);


		SA_EditorTool.ChnageDefineState(CBProvider_Path, 			"CHARTBOOST_ENABLED", 		M_Settings.Instance.ChartboostEnabled);
		SA_EditorTool.ChnageDefineState(CBSettings_Path, 			"CHARTBOOST_ENABLED", 		M_Settings.Instance.ChartboostEnabled);
	}
	
	


	private string[] FillRateToolbarStrings = new string[] {"0%", "20%", "50%", "80%", "100%"};
	private void Settings() {
		SA_EditorTool.BlockHeader("Plugin Settings");


		M_Settings.Instance.IsLogsEnabled = SA_EditorTool.ToggleFiled("Ad Mediation Logs", M_Settings.Instance.IsLogsEnabled);

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Video Ad Timeout");
		M_Settings.Instance.VideoLoadTimeOut = EditorGUILayout.FloatField(M_Settings.Instance.VideoLoadTimeOut);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Interstisial Ad Timeout");
		M_Settings.Instance.InterstisialLoadTimeOut = EditorGUILayout.FloatField(M_Settings.Instance.InterstisialLoadTimeOut);
		EditorGUILayout.EndHorizontal();


		EditorGUILayout.Space();
		SA_EditorTool.BlockHeader("Editor Testing Settings");

		M_Settings.Instance.IsEditorTestingEnabled = SA_EditorTool.ToggleFiled("Editor Testing", M_Settings.Instance.IsEditorTestingEnabled);

		GUI.enabled = M_Settings.Instance.IsEditorTestingEnabled;

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Fill Rate:");
		M_Settings.Instance.EditorFillRateIndex = GUILayout.Toolbar(M_Settings.Instance.EditorFillRateIndex, FillRateToolbarStrings, EditorStyles.radioButton);
		M_Settings.Instance.EditorFillRate = rates[M_Settings.Instance.EditorFillRateIndex];
		EditorGUILayout.Space();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("");
		EditorGUILayout.LabelField("0% - Always Error");
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("");
		EditorGUILayout.LabelField("100% - Always Provide Ad");
		EditorGUILayout.EndHorizontal();

		GUI.enabled = true;


		EditorGUILayout.Space();
		SA_EditorTool.BlockHeader("More Actions");

		EditorGUILayout.BeginHorizontal();


		if(GUILayout.Button("Open Documentation", GUILayout.Width(150))) {
			Application.OpenURL("https://goo.gl/SWoA9C");
		}

		if(GUILayout.Button("Contact Support", GUILayout.Width(150))) {
			SA_EditorTool.ContactSupportWithSubject("Ad Mediation Plugin");
		}

		if(GUILayout.Button("Load Example Settings", GUILayout.Width(150))) {
			FillExampleSettings();
		}
		EditorGUILayout.EndHorizontal();
	}

	private void FillExampleSettings() {
		M_Settings.Instance.AdColony_Android_ID = "app0c76d14a00174ba9b1";
		M_Settings.Instance.AdColonyZone_Android = "vzbe002fb30130433cb0";


	}

	private void AdNetworks() {
	

		SA_EditorTool.BlockHeader("Ad Networks");

		bool lastValue;


		EditorGUI.BeginChangeCheck();
		EditorGUI.indentLevel++; {

		
			EditorGUILayout.BeginVertical (GUI.skin.box);
			EditorGUILayout.BeginHorizontal();

			lastValue = M_Settings.Instance.AdColonyEnabled;
			M_Settings.Instance.AdColonyEnabled   = EditorGUILayout.Toggle(M_Settings.Instance.AdColonyEnabled, GUILayout.Width(30));
			if(lastValue != M_Settings.Instance.AdColonyEnabled && M_Settings.Instance.AdColonyEnabled) {
				M_Settings.Instance.AdColonyEnabled = ApproveProviderEnabling(M_VideoProvider.AdColony);
			}

			EditorGUILayout.LabelField("AdColony");
			EditorGUILayout.EndHorizontal();

			if(M_Settings.Instance.AdColonyEnabled) {
				EditorGUILayout.Space();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("IOS Id:");
				M_Settings.Instance.AdColony_IOS_ID   = EditorGUILayout.TextField(M_Settings.Instance.AdColony_IOS_ID);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("IOS Zone:");
				M_Settings.Instance.AdColonyZone_IOS   = EditorGUILayout.TextField(M_Settings.Instance.AdColonyZone_IOS);
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Android Id:");
				M_Settings.Instance.AdColony_Android_ID   = EditorGUILayout.TextField(M_Settings.Instance.AdColony_Android_ID);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Android Zone:");
				M_Settings.Instance.AdColonyZone_Android   = EditorGUILayout.TextField(M_Settings.Instance.AdColonyZone_Android);
				EditorGUILayout.EndHorizontal();
				
			
				EditorGUILayout.Space();
			}

			EditorGUILayout.EndVertical();




			EditorGUILayout.BeginVertical (GUI.skin.box);
			EditorGUILayout.BeginHorizontal();

	
			M_Settings.Instance.UnityAdsEnabled   = EditorGUILayout.Toggle(M_Settings.Instance.UnityAdsEnabled, GUILayout.Width(30));
			EditorGUILayout.LabelField("Unity Ads");

			EditorGUILayout.EndHorizontal();

			if(M_Settings.Instance.UnityAdsEnabled) {
				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("IOS Id:");
				M_Settings.Instance.UnityAds_IOS_ID   = EditorGUILayout.TextField(M_Settings.Instance.UnityAds_IOS_ID);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Android Id:");
				M_Settings.Instance.UnityAds_Android_ID   = EditorGUILayout.TextField(M_Settings.Instance.UnityAds_Android_ID);
				EditorGUILayout.EndHorizontal();
				

				EditorGUILayout.Space();
			}
			EditorGUILayout.EndVertical();





			EditorGUILayout.BeginVertical (GUI.skin.box);
			EditorGUILayout.BeginHorizontal();
			M_Settings.Instance.VungleEnabled   = EditorGUILayout.Toggle(M_Settings.Instance.VungleEnabled, GUILayout.Width(30));
			EditorGUILayout.LabelField("Vungle");
			EditorGUILayout.EndHorizontal();

			if(M_Settings.Instance.VungleEnabled) {
				EditorGUILayout.Space();
					
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("IOS Id:");
				M_Settings.Instance.Vungle_IOS_ID   = EditorGUILayout.TextField(M_Settings.Instance.Vungle_IOS_ID);
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("Android Id:");
				M_Settings.Instance.Vungle_Android_ID   = EditorGUILayout.TextField(M_Settings.Instance.Vungle_Android_ID);
				EditorGUILayout.EndHorizontal();
					
			
				EditorGUILayout.Space();
			}
			EditorGUILayout.EndVertical();


			EditorGUILayout.BeginVertical (GUI.skin.box);
			EditorGUILayout.BeginHorizontal();
			M_Settings.Instance.ChartboostEnabled   = EditorGUILayout.Toggle(M_Settings.Instance.ChartboostEnabled, GUILayout.Width(30));
			EditorGUILayout.LabelField("Chartboost");
			EditorGUILayout.EndHorizontal();
			
			if(M_Settings.Instance.ChartboostEnabled) {
				M_CB_Settings.DrawSettings();
			}
			EditorGUILayout.EndVertical();


			EditorGUILayout.BeginVertical (GUI.skin.box);
			EditorGUILayout.BeginHorizontal();
			M_Settings.Instance.iAdEnabled   = EditorGUILayout.Toggle(M_Settings.Instance.iAdEnabled, GUILayout.Width(30));
			EditorGUILayout.LabelField("iAd (IOS Native Plugin)");
			EditorGUILayout.EndHorizontal();

			
			EditorGUILayout.EndVertical();






			EditorGUILayout.BeginVertical (GUI.skin.box);
			EditorGUILayout.BeginHorizontal();
			M_Settings.Instance.GoogleMobileAdEnabled   = EditorGUILayout.Toggle(M_Settings.Instance.GoogleMobileAdEnabled, GUILayout.Width(30));
			EditorGUILayout.LabelField("AdMob (Google Mobile Ad Plugin)");
			EditorGUILayout.EndHorizontal();
			if(M_Settings.Instance.GoogleMobileAdEnabled) {

				M_AdMobEditor.DrawGADSettings();
			}

			EditorGUILayout.EndVertical();

		}EditorGUI.indentLevel--;

		if(EditorGUI.EndChangeCheck()) {
			UpdatePluginSettings();
		}
	}




	private GUIStyle _ProfileMiniButton = null;

	public GUIStyle ProfileMiniButton {
		get {
			if(_ProfileMiniButton == null) {
				_ProfileMiniButton =  new GUIStyle(EditorStyles.miniButton);
				_ProfileMiniButton.padding =  new RectOffset(0, 1, 1, 1);;
			}
			return _ProfileMiniButton;
		}
	}

	private void Profiles() {

		SA_EditorTool.BlockHeader("Mediation Profiles");

		EditorGUI.indentLevel++; {

			foreach(M_MediationProfile profile in M_Settings.Instance.Profiles) {
				EditorGUILayout.BeginVertical (GUI.skin.box);


				EditorGUILayout.BeginHorizontal(); {
					profile.IsOpen = EditorGUILayout.Foldout(profile.IsOpen, profile.Id);
					EditorGUILayout.Space();
					if(GUILayout.Button("X", ProfileMiniButton, GUILayout.Width(20))) {
						M_Settings.Instance.Profiles.Remove(profile);
						return;
					}
				} EditorGUILayout.EndHorizontal();

				if(profile.IsOpen) {
					EditorGUILayout.BeginHorizontal(); {
						EditorGUILayout.LabelField("Profile Id");
						profile.Id	 	= EditorGUILayout.TextField(profile.Id);
					} EditorGUILayout.EndHorizontal();

					EditorGUILayout.BeginHorizontal(); {
						EditorGUILayout.LabelField("Platfrom ");
						profile.Platform	 	= (RuntimePlatform) EditorGUILayout.EnumPopup(profile.Platform);
					} EditorGUILayout.EndHorizontal();



					EditorGUI.indentLevel++; {

						EditorGUILayout.Space();
						EditorGUILayout.BeginHorizontal(); {
							profile.IsVideoNetworksOpen = EditorGUILayout.Foldout(profile.IsVideoNetworksOpen, "Video Ad Networks");

							if(GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(20))) {
								profile.VideoProviders.Add(M_VideoProvider.None);
							}

							GUI.enabled = false;
							if(profile.VideoProviders.Count > 0) {
								GUI.enabled = true;
							}
							if(GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(20))) {
								profile.VideoProviders.RemoveAt(profile.VideoProviders.Count - 1);
							}

							GUI.enabled = true;

						} EditorGUILayout.EndHorizontal();



						bool HorizontalStarted = false;

						if(profile.IsVideoNetworksOpen) {
							for(int i = 0; i < profile.VideoProviders.Count; i ++) {
								
								if( i % 3 == 0) {
									HorizontalStarted = true;
									EditorGUILayout.BeginHorizontal();
									EditorGUILayout.Space();
									
								}
								profile.VideoProviders[i] = (M_VideoProvider) EditorGUILayout.EnumPopup(profile.VideoProviders[i], GUILayout.Width(150));
								
								if((i+1) % 3 == 0) {
									HorizontalStarted = false;
									EditorGUILayout.EndHorizontal();
								}
							}
							
							
							if(HorizontalStarted) {
								EditorGUILayout.EndHorizontal();
							}
							EditorGUILayout.Space();
						}







						EditorGUILayout.BeginHorizontal(); {
							profile.IsInterstisialNetworksOpen = EditorGUILayout.Foldout(profile.IsInterstisialNetworksOpen, "Interstitial Ad Networks");
							
							if(GUILayout.Button("+", EditorStyles.miniButtonLeft, GUILayout.Width(20))) {
								profile.InterstitialProviders.Add(M_InterstitialProvider.None);
							}
							
							GUI.enabled = false;
							if(profile.VideoProviders.Count > 0) {
								GUI.enabled = true;
							}
							if(GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(20))) {
								profile.InterstitialProviders.RemoveAt(profile.InterstitialProviders.Count - 1);
							}
							
							GUI.enabled = true;
							
						} EditorGUILayout.EndHorizontal();



						HorizontalStarted = false;
						if(profile.IsInterstisialNetworksOpen) {
							for(int i = 0; i < profile.InterstitialProviders.Count; i ++) {
								
								if( i % 3 == 0) {
									HorizontalStarted = true;
									EditorGUILayout.BeginHorizontal();
									EditorGUILayout.Space();
									
								}
								profile.InterstitialProviders[i] = (M_InterstitialProvider) EditorGUILayout.EnumPopup(profile.InterstitialProviders[i], GUILayout.Width(150));
								
								if((i+1) % 3 == 0) {
									HorizontalStarted = false;
									EditorGUILayout.EndHorizontal();
								}
							}
							
							
							if(HorizontalStarted) {
								EditorGUILayout.EndHorizontal();
							}

						}




						EditorGUILayout.Space();
					} EditorGUI.indentLevel--;


				}

				CheckProfile(profile);

				EditorGUILayout.EndVertical();

			}
		} EditorGUI.indentLevel--;

		
		EditorGUILayout.BeginHorizontal();

		EditorGUILayout.Space();
		if(GUILayout.Button("New Mediation Profile",  GUILayout.Width(180))) {
			M_MediationProfile p = new M_MediationProfile();
			M_Settings.Instance.Profiles.Add(p);
		}

		EditorGUILayout.EndHorizontal();




	

	}


	private bool ApproveProviderEnabling(M_VideoProvider provider) {
		return true;
	}

	private bool ProviderEnabled(M_InterstitialProvider provider) {
		return true;
	}




	private void AboutGUI() {

		SA_EditorTool.BlockHeader("About the Plugin");
		
		SA_EditorTool.SelectableLabelField(SdkVersion, M_Settings.VERSION_NUMBER);
		SA_EditorTool.SupportMail();
		
		SA_EditorTool.DrawSALogo();
	}



	private static void CheckProfile(M_MediationProfile profile) {
		if(HasDuplicates(profile.VideoProviders)) {
			profile.IsOpen = true;
			EditorGUILayout.HelpBox("Video Ad Providers Has Duplicates Netwroks", MessageType.Error);
		}

		if(HasDuplicates(profile.InterstitialProviders)) {
			profile.IsOpen = true;
			EditorGUILayout.HelpBox("Interstitial Ad Providers Has Duplicates Netwroks", MessageType.Error);
		}

		foreach(M_VideoProvider prov in profile.VideoProviders) {
			switch(prov) {
			case M_VideoProvider.AdColony:
				if(!M_Settings.Instance.AdColonyEnabled) {
					profile.IsOpen = true;
					DisplayNontenabledVideoNetworkWarning(M_VideoProvider.AdColony);
				}
				break;

			case M_VideoProvider.Chartboost:
				if(!M_Settings.Instance.ChartboostEnabled) {
					profile.IsOpen = true;
					DisplayNontenabledVideoNetworkWarning(M_VideoProvider.Chartboost);
				}
				break;

			case M_VideoProvider.UnityAds:
				if(!M_Settings.Instance.UnityAdsEnabled) {
					profile.IsOpen = true;
					DisplayNontenabledVideoNetworkWarning(M_VideoProvider.UnityAds);
				}
				break;

			case M_VideoProvider.Vungle:
				if(!M_Settings.Instance.VungleEnabled) {
					profile.IsOpen = true;
					DisplayNontenabledVideoNetworkWarning(M_VideoProvider.Vungle);
				}
				break;
			}
		}

		foreach(M_InterstitialProvider prov in profile.InterstitialProviders) {
			switch(prov) {
			case M_InterstitialProvider.AdMob:
				if(!M_Settings.Instance.GoogleMobileAdEnabled) {
					profile.IsOpen = true;
					DisplayNontenabledInterstitialNetworkWarning(M_InterstitialProvider.AdMob);
				}
				break;
				
			case M_InterstitialProvider.Chartboost:
				if(!M_Settings.Instance.ChartboostEnabled) {
					profile.IsOpen = true;
					DisplayNontenabledInterstitialNetworkWarning(M_InterstitialProvider.Chartboost);
				}
				break;
				
			case M_InterstitialProvider.iAd:
				if(!M_Settings.Instance.iAdEnabled) {
					profile.IsOpen = true;
					DisplayNontenabledInterstitialNetworkWarning(M_InterstitialProvider.iAd);
				}
				break;
			}
		}
	}

	private static void DisplayNontenabledInterstitialNetworkWarning(M_InterstitialProvider p) {
		EditorGUILayout.HelpBox("You have used " + p.ToString() + " Network in the mediation settings, but it wasn't enabled under the 'Networks' tab", MessageType.Error);
	}

	private static void DisplayNontenabledVideoNetworkWarning(M_VideoProvider p) {
		EditorGUILayout.HelpBox("You have used " + p.ToString() + " Network in the mediation settings, but it wasn't enabled under the 'Networks' tab", MessageType.Error);
	}

	private static bool HasDuplicates<T>(List<T> myList) {
		var hs = new HashSet<T>();
		
		for (var i = 0; i < myList.Count; ++i) {
			if (!hs.Add(myList[i])) return true;
		}
		return false;
	}


	
	private static void DirtyEditor() {
		#if UNITY_EDITOR
		EditorUtility.SetDirty(M_Settings.Instance);
		#endif
	}
}
