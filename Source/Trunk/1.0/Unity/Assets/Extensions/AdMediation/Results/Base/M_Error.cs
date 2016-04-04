using UnityEngine;
using System.Collections;

public class M_Error  {

	protected int _Code;
	protected string _Description;
	
	
	public M_Error(int code, string description) {
		_Code = code;
		_Description = description;
	} 
	

	
	public int Code {
		get {
			return _Code;
		}
	}
	
	public string Description {
		get {
			return _Description;
		}
	}
}
