using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.natgeo.xr {
    public class zoneEnter : MonoBehaviour
    {
        public UnityEvent onZoneEnter; 
        public UnityEvent onZoneStay;
        //public float stayEventInterval;
        public UnityEvent onZoneExit; 
   
        //public GameObject animationZone;

        private void OnTriggerEnter()
        {
            if (onZoneEnter != null)
            {
                onZoneEnter.Invoke();
            }
            //animationZone.SetActive(true);
        }

        private void OnTriggerStay()
        {
            if (onZoneStay != null)
            {
                onZoneStay.Invoke();
            }
        }

        private void OnTriggerExit()
        {
            if (onZoneExit != null)
            {
                onZoneExit.Invoke();
            }
            //animationZone.SetActive(false);
        }


    }
}
