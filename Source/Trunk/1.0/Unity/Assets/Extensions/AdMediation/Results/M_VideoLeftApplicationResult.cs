using UnityEngine;
using System.Collections;

public class M_VideoLeftApplicationResult : M_Result {

	private M_VideoProvider _AdProviderNetwork;
	
	public M_VideoLeftApplicationResult(M_VideoProvider provider):base() {
		_AdProviderNetwork = provider;
	}
	
	public M_VideoLeftApplicationResult(M_VideoProvider provider, M_Error e):base(e) {
		_AdProviderNetwork = provider;
	}
	
	
	public M_VideoProvider AdProviderNetwork {
		get {
			return _AdProviderNetwork;
		}
	}
}
