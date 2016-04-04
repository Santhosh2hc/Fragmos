//#define ADMOB_ENABLED

using UnityEngine;
using UnityEditor;
using System.Collections;

public static class M_AdMobEditor  {

	public static void DrawGADSettings() {

		#if ADMOB_ENABLED


		EditorGUILayout.Space();
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		if(GUILayout.Button("Open AdMob Settings",  GUILayout.Width(250))) {
			Selection.activeObject = GoogleMobileAdSettings.Instance;;
		}
		EditorGUILayout.Space();
		EditorGUILayout.EndHorizontal();
		
		
		EditorGUILayout.Space();


		#endif
	}

	 
}
