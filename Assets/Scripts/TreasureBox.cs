using System.Collections;
using UnityEngine;

public class TreasureBox : MonoBehaviour
{
    public GameObject lid; // Reference to the lid game object
    public float openSpeed = 2.0f; // Speed of the opening animation
    private bool isOpening = false;
    private Quaternion initialRotation;
    private Quaternion openRotation;

    void Start()
    {
        initialRotation = lid.transform.localRotation;
        openRotation = initialRotation * Quaternion.Euler(-90, 0, 0); // Adjust the rotation as needed
        StartCoroutine(DelayedOpen());
    }

    IEnumerator DelayedOpen()
    {
        yield return new WaitForSeconds(10f); // Wait for 10 seconds
        StartCoroutine(OpenLid());
    }

    IEnumerator OpenLid()
    {
        isOpening = true;
        float elapsedTime = 0;

        while (elapsedTime < 1f)
        {
            lid.transform.localRotation = Quaternion.Slerp(initialRotation, openRotation, elapsedTime);
            elapsedTime += Time.deltaTime * openSpeed;
            yield return null;
        }

        lid.transform.localRotation = openRotation;
    }
}