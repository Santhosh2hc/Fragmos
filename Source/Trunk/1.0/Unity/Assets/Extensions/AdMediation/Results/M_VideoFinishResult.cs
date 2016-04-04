using UnityEngine;
using System.Collections;

public class M_VideoFinishResult : M_Result  {

	private M_VideoProvider _AdProviderNetwork;



	public M_VideoFinishResult(M_VideoProvider provider):base() {
		_AdProviderNetwork = provider;
	}

	public M_VideoFinishResult(M_VideoProvider provider, M_Error e):base(e) {
		_AdProviderNetwork = provider;
	}


	public M_VideoProvider AdProviderNetwork {
		get {
			return _AdProviderNetwork;
		}
	}
}
