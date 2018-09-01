using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

namespace com.natgeo.xr {
	public class StagePlacement : MonoBehaviour {
	    public Transform m_HitTransform;
	    private bool stagePlaced = false;

	    // Update is called once per frame
	    void Update()
	    {
	        if (!stagePlaced && Input.touchCount > 0 && m_HitTransform != null)
	        {
	            var touch = Input.GetTouch(0);
	            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
	            {
	                var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
	                ARPoint point = new ARPoint
	                {
	                    x = screenPosition.x,
	                    y = screenPosition.y
	                };

	                // prioritize reults types
	                ARHitTestResultType[] resultTypes = {
	                        ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
	                        // if you want to use infinite planes use this:
	                        //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
	                        ARHitTestResultType.ARHitTestResultTypeHorizontalPlane,
	                        ARHitTestResultType.ARHitTestResultTypeFeaturePoint
	                    };

	                foreach (ARHitTestResultType resultType in resultTypes)
	                {
	                    if (HitTestWithResultType(point, resultType))
	                    {
	                        return;
	                    }
	                }
	            }
	        }
	    }

	    bool HitTestWithResultType(ARPoint point, ARHitTestResultType resultTypes)
	    {
	        List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
	        if (hitResults.Count > 0)
	        {
	            foreach (var hitResult in hitResults)
	            {
	                Debug.Log("Got hit!");
	                m_HitTransform.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
	                m_HitTransform.rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
	                Debug.Log(string.Format("x:{0:0.######} y:{1:0.######} z:{2:0.######}", m_HitTransform.position.x, m_HitTransform.position.y, m_HitTransform.position.z));
	                stagePlaced = true;
	                return true;
	            }
	        }
	        return false;
	    }
	}
}

