//#define CODE_DISABLED

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;

public class M_PostProcess  {
	

	[PostProcessBuild(2)]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {


		M_Settings.Instance.SetAppversion(PlayerSettings.bundleVersion);
	}

}
