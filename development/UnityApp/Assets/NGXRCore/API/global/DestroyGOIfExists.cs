using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace com.natgeo.xr {
	public class DestroyGOIfExists : MonoBehaviour {

		private static DestroyGOIfExists _instance ;

		void Awake() {
			//if we don't have an [_instance] set yet
			if(!_instance)
				_instance = this ;
			//otherwise, if we do, kill this thing
			else
				Destroy(this.gameObject) ;


			DontDestroyOnLoad(this.gameObject) ;

		}
	}
}
