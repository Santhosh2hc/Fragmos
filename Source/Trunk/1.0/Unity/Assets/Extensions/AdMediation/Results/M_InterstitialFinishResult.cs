using UnityEngine;
using System.Collections;

public class M_InterstitialFinishResult : M_Result  {

	private M_InterstitialProvider _AdProviderNetwork;



	public M_InterstitialFinishResult(M_InterstitialProvider provider):base() {
		_AdProviderNetwork = provider;
	}

	public M_InterstitialFinishResult(M_InterstitialProvider provider, M_Error e):base(e) {
		_AdProviderNetwork = provider;
	}


	public M_InterstitialProvider AdProviderNetwork {
		get {
			return _AdProviderNetwork;
		}
	}
}
