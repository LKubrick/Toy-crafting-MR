using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
    
{
   
    public GameObject Propeller;
   
    public void AeroplaneAnim()
    {
       Propeller.SetActive(true);
        gameObject.GetComponent<MeshRenderer>().enabled = false;

    }
}
