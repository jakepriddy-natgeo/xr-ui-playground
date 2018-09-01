using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

namespace com.natgeo.xr {
	public static class ARKitCalibrationType {

		[System.Serializable]
		public enum ARHitType
		{
			FeaturePoint,
			NewHorizontalPlane,
			NewVerticalPlane,
			ExistingHorizontalPlane,
			ExistingHorizontalPlaneUsingExtent
		}

		public static List<float> GetARResultType(ARHitType[] arTypes) {
			List<float> types = new List<float> ();
			foreach (ARHitType arType in arTypes) {
				switch(arType) {
				case ARHitType.FeaturePoint:
					types.Add (0x1); //ARHitTestResultType.ARHitTestResultTypeFeaturePoint
					break;
				case ARHitType.NewHorizontalPlane:
					types.Add (0x2); //ARHitTestResultType.ARHitTestResultTypeHorizontalPlane
					break;
				case ARHitType.NewVerticalPlane:
					types.Add (0x4); //ARHitTestResultType.ARHitTestResultTypeVerticalPlane
					break;
				case ARHitType.ExistingHorizontalPlane:
					types.Add (0x8); //ARHitTestResultType.ARHitTestResultTypeExistingPlane
					break;
				case ARHitType.ExistingHorizontalPlaneUsingExtent:
					types.Add (0x10); //ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent
					break;

				}
			}

			return types;
		}
	}
}
