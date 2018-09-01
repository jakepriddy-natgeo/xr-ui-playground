using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.natgeo.xr
{
    public class FollowMainCameraTransform : FollowTransform
    {
        
        // Use this for initialization
        void Update()
        {
			
			if (Camera.main != null && target == null) 
            {
                target = Camera.main.transform;

            }
        }

    }
}
