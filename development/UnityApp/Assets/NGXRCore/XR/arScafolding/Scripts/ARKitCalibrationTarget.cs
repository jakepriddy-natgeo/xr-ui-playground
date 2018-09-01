using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.natgeo.xr {
	public class ARKitCalibrationTarget : IARKitCalibrationTarget {

		private Vector3 globalPosition = Vector3.zero;
		private Quaternion globalRotation = Quaternion.identity;
        public bool lookAtCameraRotation = true;


		public override void ProcessARAnchorPosition(Vector3 position) {
			globalPosition.x = position.x;
			globalPosition.y = position.y;
			globalPosition.z = position.z;
          
		}

		public override void ProcessARAnchorRotation(Quaternion rotation) {
			if (lookAtCameraRotation == false) {
				globalRotation.w = rotation.w;
				globalRotation.x = rotation.x;
				globalRotation.y = rotation.y;
				globalRotation.z = rotation.z;
			} else {
                Vector3 targetPostition  = new Vector3( Camera.main.transform.position.x, this.transform.position.y, Camera.main.transform.position.z);
                transform.LookAt(targetPostition);
			}
		}

		void Update() {
			transform.position = globalPosition;
			if (lookAtCameraRotation == false) {
				transform.rotation = globalRotation;
			}
		}
	}
}
