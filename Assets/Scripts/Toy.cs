using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum ToyType
{
    Airplane,
    Robot,
    Lego
}
public class Toy : MonoBehaviour
{
    public ToyType toyType;
    public int partsToSnap;

    private void Start()
    {
        gameObject.SetActive(false);
    }
}
