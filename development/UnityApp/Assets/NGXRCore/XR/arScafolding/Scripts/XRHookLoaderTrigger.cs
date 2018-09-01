using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace com.natgeo.xr {
	public class XRHookLoaderTrigger : MonoBehaviour {
			
		[SerializeField]
		private XRHookLoaderHandler.PlatformXRTarget editorOverrideXRPlatform = XRHookLoaderHandler.PlatformXRTarget.ARKit;

		[SerializeField]
		private GameObject ARKitEditorLauncher;

		[SerializeField]
		private GameObject ARCoreEditorLauncher;

		public GameObject[] calibrationCoordinateReceiver;

		public UnityEvent OnStopCalibration;

		// Use this for initialization
		void Start () {
            StartCoroutine(WaitForEndOfFrame());
			
		}
        private IEnumerator WaitForEndOfFrame() {

            yield return new WaitForEndOfFrame();

            XRHookLoaderHandler.PlatformXRTarget targetXR = XRHookLoaderHandler.PlatformXRTarget.ARKit;
#if UNITY_EDITOR
            targetXR = editorOverrideXRPlatform;
#elif UNITY_IOS
            targetXR = XRHookLoaderHandler.PlatformXRTarget.ARKit;
#elif UNITY_ANDROID
            targetXR = XRHookLoaderHandler.PlatformXRTarget.ARCore;
#endif


            switch (targetXR)
            {
                case XRHookLoaderHandler.PlatformXRTarget.ARKit:
                    ARKitCalibrationHandler.Instance.calibrationStopEvent += ARKitCalibrationStopHandler;
                    ARKitCalibrationHandler.Instance.coordinateReadyEvent += ARKitCoordinateReadyHandler;
                    break;
                case XRHookLoaderHandler.PlatformXRTarget.ARCore:
                    //TODO: ARCORE
                    break;
            }

#if UNITY_EDITOR
            GameObject kit = null;
            switch (targetXR)
            {
                case XRHookLoaderHandler.PlatformXRTarget.ARKit:
                    kit = Instantiate(ARKitEditorLauncher);

                    break;
                case XRHookLoaderHandler.PlatformXRTarget.ARCore:
                    kit = Instantiate(ARCoreEditorLauncher);
                    break;
            }
            if (kit != null)
            {
                kit.transform.SetParent(transform);
            }
#elif UNITY_IOS || UNITY_ANDROID
            XRHookLoaderHandler.Instance.LoadPlatformTargetHandler (targetXR);
#endif

        }

		private void ARKitCalibrationStopHandler() {
			ARKitCalibrationHandler.Instance.calibrationStopEvent -= ARKitCalibrationStopHandler;

			if (OnStopCalibration != null)
				OnStopCalibration.Invoke ();
		}

		private void ARKitCoordinateReadyHandler(Vector3 position, Quaternion rotation) {
			if (calibrationCoordinateReceiver != null && calibrationCoordinateReceiver.Length > 0) {
				foreach (GameObject ccr in calibrationCoordinateReceiver) {
					ccr.SendMessage ("ProcessARAnchorPosition", position, SendMessageOptions.DontRequireReceiver);
					ccr.SendMessage ("ProcessARAnchorRotation", rotation, SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}
}