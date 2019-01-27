using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ManagerBase<T> : MonoBehaviour where T : ManagerBase<T> {

	//static instance that shared for all namespace
	private static T _manager;

	public static T Instance {
		get {
			return _manager;
		}
	}

	protected virtual void Awake() {
		//create a new static instance when awake
		_manager = (T)this;
		//DontDestroyOnLoad(this);
	}

	public bool Exist() {
		return _manager != null;
	}
}