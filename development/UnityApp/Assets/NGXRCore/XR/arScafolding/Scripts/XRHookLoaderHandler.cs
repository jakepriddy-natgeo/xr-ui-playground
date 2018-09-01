using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.natgeo.xr {
	public class XRHookLoaderHandler : MonoBehaviour {

		static XRHookLoaderHandler _instance;

		public static XRHookLoaderHandler Instance
		{
			get
			{
				if (!_instance)
				{
					Debug.LogWarning("No XR Hook Loader Handler instance available. Creating one.");
					GameObject obj = new GameObject("XR Hook Loader Handler");
					_instance = obj.AddComponent<XRHookLoaderHandler>();
				}
				return _instance;
			}
		}

		[System.Serializable]
		public enum PlatformXRTarget
		{
			ARKit,
			ARCore
		}

		private struct PlatformXRMeta
		{
			public PlatformXRTarget platformXRTarget;
			public string sceneHandler;
            public string sceneName;
		}

		private List<PlatformXRMeta> availableXRPlatforms = new List<PlatformXRMeta> ();

        private PlatformXRMeta loadedPlatformMeta = new PlatformXRMeta();

		protected void Awake()
		{   
			// Prevent having more than one instance in a scene
			if (_instance)
			{ 
				Debug.LogWarning("There is already an instance of XRHookLoaderHandler.");
				Destroy(gameObject);
				return;
			}

			// The CoreLoaderHandler instance should be persistent between scenes
			//DontDestroyOnLoad(gameObject);

			_instance = this;


			//Manually Setting XR Platforms
			//ARKIT
			PlatformXRMeta arKit = new PlatformXRMeta ();
			arKit.platformXRTarget = PlatformXRTarget.ARKit;
			arKit.sceneHandler = "Projects/MainCore/xrhooks/ARKitHook";
            arKit.sceneName = "ARKitHook";
			availableXRPlatforms.Add (arKit);

			//ARCORE
			PlatformXRMeta arCore = new PlatformXRMeta ();
			arCore.platformXRTarget = PlatformXRTarget.ARCore;
			arCore.sceneHandler = "Projects/MainCore/xrhooks/ARCoreHook";
            arCore.sceneName = "ARCoreHook";
			availableXRPlatforms.Add (arCore);
		}

		public void LoadPlatformTargetHandler(PlatformXRTarget target) {
			foreach (PlatformXRMeta meta in availableXRPlatforms) {
				if (meta.platformXRTarget == target) {
                    loadedPlatformMeta = meta;
					StartCoroutine(LoadAsyncScene(meta.sceneHandler));
					return;
				}
			}

			Debug.Log ("Error, provided platform target not implemented");

		}

		IEnumerator LoadAsyncScene(string sceneHandlerPath)
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneHandlerPath, LoadSceneMode.Additive);

			// Wait until the asynchronous scene fully loads
			while (!asyncLoad.isDone)
			{
				yield return null;
			}
		}

		private void OnDestroy()
		{
            if(loadedPlatformMeta.sceneName != null) {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(loadedPlatformMeta.sceneName));
            }
		}
	}
}
