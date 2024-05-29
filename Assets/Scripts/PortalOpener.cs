using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalOpener : MonoBehaviour
{
    [SerializeField] private GameObject portalGameObject;
    public void OpenPortal()
    {
        portalGameObject.SetActive(true);
    }
}
