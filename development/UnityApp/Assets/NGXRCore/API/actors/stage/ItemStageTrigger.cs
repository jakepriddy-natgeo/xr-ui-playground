using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.natgeo.xr {
	public class ItemStageTrigger : ItemStage {
		public UnityEvent OnStageStarted;
		public UnityEvent OnStageExited;

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		#region implemented abstract members of ItemStage

		public override void StartStage ()
		{
			if (OnStageStarted != null)
				OnStageStarted.Invoke ();
		}

		public override void ExitStage (CallbackHandler onFinished)
		{
			if (OnStageExited != null)
				OnStageExited.Invoke ();

			if (onFinished != null)
				onFinished.Invoke(StageID);
		}

		#endregion
	}
}