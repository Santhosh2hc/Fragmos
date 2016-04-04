using UnityEngine;
using System.Collections;

public class M_Result  {

	protected bool _IsSucceeded = true;
	protected M_Error _Error = null;


	public M_Result() {
		_IsSucceeded = true;
	}
	
	public M_Result (M_Error e) {
		SetError(e);
	}

	
	//--------------------------------------
	// Get / Set
	//--------------------------------------
	
	
	public bool IsSucceeded {
		get {
			return _IsSucceeded;
		}
	}
	
	public bool IsFailed {
		get {
			return !_IsSucceeded;
		}
	}


	//--------------------------------------
	// Private Methods
	//--------------------------------------
	
	public void SetError(M_Error e) {
		_Error = e;
		_IsSucceeded = false;
	}
}
