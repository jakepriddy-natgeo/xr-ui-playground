﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class thumbstickReciever : MonoBehaviour
{

    public UnityEvent onThumbstickEnter;
    public UnityEvent onThumbstickStay;
    public UnityEvent onThumbstickExit;
    //public UnityEvent onThumbsitckSelect;

	private void Start()
	{
		
	}

    private void OnCollisionStay2D(Collision2D col)
    {

        if (col.collider.gameObject.tag == "thumbstick")
        {
            col.collider.GetComponent<thumbstickControl>().collision = true;
            if (col.collider.GetComponent<thumbstickControl>().dragging)
            {
                col.collider.gameObject.transform.position = transform.position;
                //do something when the thumbstick is dragged onto this object
                Debug.Log("Staying");
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col)
	{

        col.collider.GetComponent<thumbstickControl>().collision = false;
        Debug.Log("Exited");

	}

	private void OnCollisionEnter2D(Collision2D col)
	{
        
        col.collider.GetComponent<thumbstickControl>().collision = true;
        Debug.Log("Entered");

	}


    /*private void OnCollisionStay2D(Collision2D col)
    {

        if (col.collider.gameObject.tag == "thumbstick")
        {
            col.collider.GetComponent<thumbstickControl>().collision = true;
            if (!col.collider.GetComponent<thumbstickControl>().dragging)
            {
                col.collider.gameObject.transform.position = transform.position;
                //do something when the thumbstick is dragged onto this object
                Debug.Log("Staying");
            }
        }
    }
*/
}


