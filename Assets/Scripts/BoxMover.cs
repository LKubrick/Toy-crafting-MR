using System.Collections;
using UnityEngine;

public class BoxMover : MonoBehaviour
{
    public GameObject toyAirplaneInBox; // Reference to the toy airplane game object inside the box
    public GameObject toyAirplaneOnTable; // Reference to the toy airplane game object on the table
    public float riseSpeed = 1.0f; // Speed of the box rising
    public float delayBeforeMoving = 5.0f; // Delay before the box starts moving
    public float flipSpeed = 0.5f; // Speed of the flip animation

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Quaternion endRotation;
    private Vector3 airPosition;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        endRotation = Quaternion.Euler(-180, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z); // Adjust for front flip

        // Calculate the air position based on the toy airplane's start position + 0.5 meters altitude
        airPosition = toyAirplaneOnTable.transform.position + Vector3.up * 0.5f;

        // Set the toy airplane on the table to inactive at the start
        toyAirplaneOnTable.SetActive(false);

        StartCoroutine(StartMovingAfterDelay());
    }

    IEnumerator StartMovingAfterDelay()
    {
        yield return new WaitForSeconds(15f); // 10 seconds for the lid to open, then 5 seconds delay
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
        toyAirplaneOnTable.SetActive(true);
        // Make the toy airplane in the box inactive
        toyAirplaneInBox.SetActive(false);

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
    }
}
