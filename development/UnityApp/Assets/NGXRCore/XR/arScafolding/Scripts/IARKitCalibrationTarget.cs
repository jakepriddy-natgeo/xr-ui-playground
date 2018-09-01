using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.natgeo.xr {
	public abstract class IARKitCalibrationTarget : MonoBehaviour {
		public abstract void ProcessARAnchorPosition(Vector3 position);
		public abstract void ProcessARAnchorRotation(Quaternion rotation);
	}
}