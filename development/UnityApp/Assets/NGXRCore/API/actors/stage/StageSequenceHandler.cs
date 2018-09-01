using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.natgeo.xr {
	public class StageSequenceHandler : MonoBehaviour {

		static StageSequenceHandler _instance;

		public static StageSequenceHandler Instance
		{
			get
			{
				if (!_instance)
				{
					Debug.LogWarning("No Stage Sequence Handler instance available. Creating one.");
					GameObject obj = new GameObject("Stage Sequence Handler");
					_instance = obj.AddComponent<StageSequenceHandler>();
				}
				return _instance;
			}
		}

		public enum StartStageMethod
		{
			CloseAll,
			Additively
		}
		public ItemStage[] stages;

		private List<int> queuedStages = new List<int>();
		private int closingCount = 0;
		private bool processing = false;


		private Dictionary<int,ItemStage> openStages = new Dictionary<int, ItemStage> ();

		// Use this for initialization
		void Start () {

		}
		
		// Update is called once per frame
		void Update () {

			#if UNITY_EDITOR

			if (Input.GetKeyUp("space")) {
				StartStage(0, StartStageMethod.CloseAll );
			}

			#endif


			if (queuedStages.Count > 0 && !processing) {
				int stageKey = GetNextQueuedStageId ();
				if (stageKey >= 0) {
					queuedStages.Remove (stageKey);
					InitStage(stageKey);
				}
			}
		}

		public void StartStage(int startStageId, StartStageMethod startMethod ) {
			if (openStages.ContainsKey(startStageId)) {
				Debug.Log ("Stage Already Started");
				return;
			}

			if (queuedStages.Contains(startStageId)) {
				Debug.Log ("Stage Already Queued To Start");
				return;
			}


			queuedStages.Add (startStageId);

			if (startMethod == StartStageMethod.CloseAll) {
				processing = true;
				CloseOpenStages ();
			}
			else if (startMethod == StartStageMethod.Additively) {
				processing = false;
			}
		}

		private ItemStage GetStageFromId(int stageId) {
			foreach (ItemStage stg in stages) {
				if (stg.StageID== stageId) {
					return stg;
				}
			}

			return null;
		}

		private int GetNextQueuedStageId() {
			int _key = -1;
			foreach (int key in queuedStages) {
				_key = key;
				break;
			}

			return _key;
		}
			
		private void CloseOpenStages() {
			closingCount = openStages.Count;
			if (closingCount == 0) {
				processing = false;
			}

			foreach(KeyValuePair<int, ItemStage> entry in openStages)
			{
				entry.Value.ExitStage (CleanupStage);
			}
		}

		private void CleanupStage(int id) {
			openStages.Remove (id);
			closingCount--;

			if (closingCount <= 0) {
				closingCount = 0;
				processing = false;
			}

		}

		public void InitStage(int stageKey) {
			ItemStage currentStage = GetStageFromId (stageKey);
			if ( currentStage != null) {
				currentStage.gameObject.SetActive (true);
				currentStage.StartStage ();
				openStages.Add (stageKey, currentStage);
			} else {
				Debug.LogError ("Provided Stage Key is Out of Bounds");
			}
		}


		public void CloseStage(int stageKey) {
			if (openStages.ContainsKey (stageKey)) {
				processing = true;
				ItemStage stage = openStages [stageKey];
				stage.ExitStage (CleanupStage);
			}
		}

		public void CloseAllStages() {
			queuedStages.Clear ();
			processing = true;
			CloseOpenStages ();
		}
			
	}
}