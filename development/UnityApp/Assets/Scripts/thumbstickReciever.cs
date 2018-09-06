using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class thumbstickReciever : MonoBehaviour
{

    public UnityEvent onThumbstickEnter;
    public UnityEvent onThumbstickStay;
    public UnityEvent onThumbstickExit;
    public UnityEvent onThumbsitckRelease;

	private void Start()
	{
		
	}

    private void OnCollisionStay2D(Collision2D col)
    {

        if (col.collider.gameObject.tag == "thumbstick")
        {
            col.collider.GetComponent<thumbstickControl>().collision = true;
            if (!col.collider.GetComponent<thumbstickControl>().dragging)
            {
                col.collider.gameObject.transform.position = transform.position;
                //do something when the thumbstick is dragged onto this object

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




}


/*
 * 
 * For enterprise   ************
onThumbstickExit.Invoke();


    private void OnTriggerEnter()
{
    if (onThumbstickEnter != null)
    {
        onThumbstickEnter.Invoke();
    }
    //animationZone.SetActive(true);
}
private void OnTriggerEnter()
{
    if (onThumbstickStay != null)
    {
        onThumbstickStay.Invoke();
    }
    //animationZone.SetActive(true);
}
private void OnTriggerEnter()
{
    if (onThumbstickExit != null)
    {
        onThumbstickExit.Invoke();
    }
    //animationZone.SetActive(true);
}
private void OnTriggerEnter()
{
    if (onThumbsitckRelease != null)
    {
        onThumbsitckRelease.Invoke();
    }
    //animationZone.SetActive(true);
}

*/

