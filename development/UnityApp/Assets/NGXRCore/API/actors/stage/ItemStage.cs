using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.natgeo.xr {
	public abstract class ItemStage : MonoBehaviour {
		public delegate void CallbackHandler(int id);
	    public abstract void StartStage();
		public abstract void ExitStage(CallbackHandler onFinished);
		public int StageID;
	}
}
