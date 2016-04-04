using UnityEngine;
using System.IO;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif
public class M_Settings : ScriptableObject {

	public const string VERSION_NUMBER = "1.4";

	[SerializeField]
	public List<M_MediationProfile> Profiles =  new List<M_MediationProfile>();



	public bool GoogleMobileAdEnabled = false;


	public bool iAdEnabled = false;

	public bool UnityAdsEnabled = false;
	public bool UnityAdsOpen = false;
	public string UnityAds_IOS_ID 		= string.Empty;
	public string UnityAds_Android_ID 	= string.Empty;


	public bool VungleEnabled = false;
	public string Vungle_IOS_ID 		= string.Empty;
	public string Vungle_Android_ID 	= string.Empty;


	public bool AdColonyEnabled = false;
	public string AdColony_IOS_ID 		= string.Empty;
	public string AdColony_Android_ID 	= string.Empty;

	public string AdColonyZone_IOS 		= string.Empty;
	public string AdColonyZone_Android 	= string.Empty;

	private string _AppVersion = string.Empty;



	public bool ChartboostEnabled = false;


	public float VideoLoadTimeOut = 10f;
	public float InterstisialLoadTimeOut = 10f;

	public bool IsEditorTestingEnabled = true;
	public bool IsLogsEnabled = true;
	public int EditorFillRateIndex = 4;
	public int EditorFillRate = 50;

	public int ToolbarIndex = 0;


	
	private const string MAssetName = "AdMediationSetting";
	private const string MAssetExtension = ".asset";
	
	
	private static M_Settings _instance;
	
	public static M_Settings Instance {
		get {
			if(_instance == null) {
				_instance = Resources.Load(MAssetName) as M_Settings;
				if(_instance == null) {
					_instance = CreateInstance<M_Settings>();

					#if UNITY_EDITOR
					FileStaticAPI.CreateFolder(SA_Config.SettingsPath);
					string fullPath = Path.Combine(Path.Combine("Assets", SA_Config.SettingsPath), MAssetName + MAssetExtension );
					AssetDatabase.CreateAsset(_instance, fullPath);
					#endif
					
				}
			}
			return _instance;
		}
	}

	public void SetAppversion(string version) {
		_AppVersion = version;
	}


	public string AppVersion {
		get {
			return _AppVersion;
		}
	}
}
