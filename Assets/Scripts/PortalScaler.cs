using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortalScaler : MonoBehaviour
{
    [SerializeField] private Vector3 targetScale = new Vector3(75, 75, 75);
    [SerializeField] private float duration = 2.0f;

    private Vector3 initialScale;
    private float timeElapsed;

    public UnityEvent changePassthroughView;

    void Start()
    {
        initialScale = transform.localScale;
        StartCoroutine(ScaleToTarget());
    }

    private IEnumerator ScaleToTarget()
    {
        while (timeElapsed < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, targetScale, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
        changePassthroughView.Invoke();
    }
}
