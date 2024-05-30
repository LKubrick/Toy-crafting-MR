using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class BoxMover : MonoBehaviour
{
    public GameObject toyInBox; 
    public GameObject toyOnTable; 
    public float riseSpeed = 1.0f; 
    public float delayBeforeMoving = 1.0f; 
    public float flipSpeed = 0.5f; 

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Quaternion endRotation;
    private Vector3 airPosition;

    public UnityEvent onToyChosed;
    private bool canChoseToy = true;
    
    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        endRotation = Quaternion.Euler(-180, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z); // Adjust for front flip

        // Calculate the air position based on the toy airplane's start position + 0.5 meters altitude
        airPosition = toyOnTable.transform.position + Vector3.up * 0.5f;

    }

    private void OnTriggerEnter(Collider other)
    {
        ToyInfo toyInfo = other.GetComponent<ToyInfo>();
        if (toyInfo != null && canChoseToy)
        {
            toyOnTable.gameObject.SetActive(false);
            
            // Retrieve the toy objects from the colliding object
            toyInBox = toyInfo.toyInBox;
            toyOnTable = toyInfo.toyOnTable;

            // Change the toys
            ChangeToys();
            onToyChosed.Invoke();
            canChoseToy = false;
        }
    }

    private void ChangeToys()
    {
        // Start the movement and flip coroutine if necessary
        StartCoroutine(StartMovingAfterDelay());
    }
    
    IEnumerator StartMovingAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeMoving); 
        StartCoroutine(RiseAndFlipBox());
    }

    IEnumerator RiseAndFlipBox()
    {
        float elapsedTime = 0;

        // Move the box to the air position
        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(initialPosition, airPosition, elapsedTime);
            transform.rotation = Quaternion.Slerp(initialRotation, endRotation, elapsedTime);
            elapsedTime += Time.deltaTime * riseSpeed;
            yield return null;
        }

        transform.position = airPosition;
        transform.rotation = endRotation;

        // Make the toy airplane on the table visible after the flip
        toyOnTable.SetActive(true);
        // Make the toy airplane in the box inactive
        toyInBox.SetActive(false);

        StartCoroutine(ReturnToInitialPosition());
    }

    IEnumerator ReturnToInitialPosition()
    {

        
        float elapsedTime = 0;

        // Move the box back to its initial position
        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(airPosition, initialPosition, elapsedTime);
            transform.rotation = Quaternion.Slerp(endRotation, initialRotation, elapsedTime);
            elapsedTime += Time.deltaTime * flipSpeed; // Slow down the flip back
            yield return null;
        }

        transform.position = initialPosition;
        transform.rotation = initialRotation;
        
        canChoseToy = true;
    }
}