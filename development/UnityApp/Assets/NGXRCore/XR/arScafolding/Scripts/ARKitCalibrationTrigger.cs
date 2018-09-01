using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.Events;


namespace com.natgeo.xr {
	public class ARKitCalibrationTrigger : MonoBehaviour {

		public ARKitCalibrationType.ARHitType[] hitResultType;
		public bool calibrateOnStart = false;
		public GameObject[] calibrationCoordinateReceiver;

		public UnityEvent OnStartCalibration;
		public UnityEvent OnFirstTargetAvailable;
		public UnityEvent OnTargetAvailable;
		public UnityEvent OnStopCalibration;

		// Use this for initialization
		void Start () {
			if (calibrateOnStart) {
				SubscribeHandlerEvents ();
				StartCalibration ();
			}
		}

		private void SubscribeHandlerEvents() {
			ARKitCalibrationHandler.Instance.calibrationStartedEvent += CalibrationStartedHandler;
			ARKitCalibrationHandler.Instance.firstCoordinateReadyEvent += FirstCoordinateReadyHandler;
			ARKitCalibrationHandler.Instance.coordinateReadyEvent += CoordinateReadyHandler;
			ARKitCalibrationHandler.Instance.calibrationStopEvent += CalibrationStopHandler;
		}

		public void StartCalibration() {
			ARKitCalibrationHandler.Instance.StartCalibration (ARKitCalibrationType.GetARResultType (hitResultType));
		}

		public void StopCalibration() {
			ARKitCalibrationHandler.Instance.StopCalibration ();
		}


		private void CalibrationStartedHandler() {
			ARKitCalibrationHandler.Instance.calibrationStartedEvent -= CalibrationStartedHandler;
			if (OnStartCalibration != null)
				OnStartCalibration.Invoke ();
		}

		private void FirstCoordinateReadyHandler(Vector3 position, Quaternion rotation) {
			ARKitCalibrationHandler.Instance.firstCoordinateReadyEvent -= FirstCoordinateReadyHandler;
			if (OnFirstTargetAvailable != null)
				OnFirstTargetAvailable.Invoke ();

			CoordinateReadyHandler (position, rotation);
		}

		private void CoordinateReadyHandler(Vector3 position, Quaternion rotation) {
			if (OnTargetAvailable != null)
				OnTargetAvailable.Invoke ();

			if (calibrationCoordinateReceiver != null && calibrationCoordinateReceiver.Length > 0) {
				foreach (GameObject ccr in calibrationCoordinateReceiver) {
					ccr.SendMessage ("ProcessARAnchorPosition", position, SendMessageOptions.DontRequireReceiver);
					ccr.SendMessage ("ProcessARAnchorRotation", rotation, SendMessageOptions.DontRequireReceiver);
				}
			}
		}

		private void CalibrationStopHandler() {
			ARKitCalibrationHandler.Instance.coordinateReadyEvent -= CoordinateReadyHandler;
			ARKitCalibrationHandler.Instance.calibrationStopEvent -= CalibrationStopHandler;

			if (OnStopCalibration != null)
				OnStopCalibration.Invoke ();
		}

		
		// Update is called once per frame
		void Update () {
			
		}
	}
}
