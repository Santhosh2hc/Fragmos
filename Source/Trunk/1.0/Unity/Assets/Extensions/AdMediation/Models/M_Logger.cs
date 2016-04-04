using UnityEngine;
using System.Collections;

public static class M_Logger  {

	private static string LOG_HEADER = "Ad Mediation: ";


	public static void Log(string msg) {

		if(!M_Settings.Instance.IsLogsEnabled) {
			return;
		}

		Debug.Log(LOG_HEADER + msg);
	}
}
