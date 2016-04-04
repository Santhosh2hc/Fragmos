using UnityEngine;
using UnityEditor;
using System.Collections;

static public class M_Menu {

	[MenuItem("Window/Stan's Assets/Ad Mediation/Edit Settings", false, 0)]
	public static void LoadManagerWindow() {
		Selection.activeObject = M_Settings.Instance;
	}

	[MenuItem("Window/Stan's Assets/Ad Mediation/Getting Started", false, 11)]
	public static void GettingStarted() {
		Application.OpenURL("https://goo.gl/MSX301");
	}


	[MenuItem("Window/Stan's Assets/Ad Mediation/Networks Setup/AdColony", false, 12)]
	public static void AdColony() {
		Application.OpenURL("https://goo.gl/y83QIn");
	}

	[MenuItem("Window/Stan's Assets/Ad Mediation/Networks Setup/Unity Ads", false, 13)]
	public static void UnityAds() {
		Application.OpenURL("https://goo.gl/EwG4pv");
	}


	[MenuItem("Window/Stan's Assets/Ad Mediation/Networks Setup/Vungle", false, 14)]
	public static void Vungle() {
		Application.OpenURL("https://goo.gl/UdQ3Xv");
	}

	[MenuItem("Window/Stan's Assets/Ad Mediation/Networks Setup/Chartboost", false, 15)]
	public static void Chartboost() {
		Application.OpenURL("https://goo.gl/7KoUDl");
	}

	[MenuItem("Window/Stan's Assets/Ad Mediation/Networks Setup/iAd", false, 16)]
	public static void iAd() {
		Application.OpenURL("https://goo.gl/sT44lR");
	}

	[MenuItem("Window/Stan's Assets/Ad Mediation/Networks Setup/AdMob", false, 17)]
	public static void AdMob() {
		Application.OpenURL("https://goo.gl/tEhYtm");
	}



	[MenuItem("Window/Stan's Assets/Ad Mediation/Release Notes", false, 18)]
	public static void ReleaseNotes() {
		Application.OpenURL("https://goo.gl/XJIZYa");
	}

	

	//--------------------------------------
	//  GENERAL
	//--------------------------------------



}
