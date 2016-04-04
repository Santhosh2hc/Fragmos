//#define CHARTBOOST_ENABLED

using UnityEngine;
using UnityEditor;
using System.Collections;

#if  CHARTBOOST_ENABLED
using ChartboostSDK;
#endif

public class M_CB_Settings  {




	public static void DrawSettings() {
		#if CHARTBOOST_ENABLED
		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		if(GUILayout.Button("Open Chartboost Settings",  GUILayout.Width(250))) {
			CBSettings.Edit();
		}
		EditorGUILayout.Space();
		EditorGUILayout.EndHorizontal();
		
		
		EditorGUILayout.Space();

	#endif
	}
}
