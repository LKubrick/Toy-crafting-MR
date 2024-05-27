using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToyCraftingManager : MonoBehaviour
{
    private Dictionary<ToyType, Toy> toyPartsRemaining;
    public AnimationManager _AnimationManager;

    private void Start()
    {
        toyPartsRemaining = new Dictionary<ToyType, Toy>();

        Toy[] toyTypes = FindObjectsOfType<Toy>();

        foreach (Toy toyType in toyTypes)
        {
            toyPartsRemaining[toyType.toyType] = toyType;
        }
    }

    private void OnEnable()
    {
        SnapInteractableExtended.onObjectSnapped.AddListener(OnToySnapped);
        SnapInteractableExtended.onObjectUnsnapped.AddListener(OnToyUnsnapped);
    }

    private void OnDisable()
    {
        SnapInteractableExtended.onObjectSnapped.RemoveListener(OnToySnapped);
        SnapInteractableExtended.onObjectUnsnapped.RemoveListener(OnToyUnsnapped);
    }

    private void OnToySnapped(Toy arg0)
    {
        if (toyPartsRemaining.ContainsKey(arg0.toyType))
        {
            toyPartsRemaining[arg0.toyType].partsToSnap--;
        }
        else
        {
            Debug.LogError("Toy not in dicionary");
        }
        
        Debug.Log($"Toy snapped: {arg0.toyType}. Parts left: {toyPartsRemaining[arg0.toyType].partsToSnap}");

        CheckToyCompletion(arg0);

    }
    private void OnToyUnsnapped(Toy arg0)
    {
        if (toyPartsRemaining.ContainsKey(arg0.toyType) && toyPartsRemaining[arg0.toyType] != null)
        {
            toyPartsRemaining[arg0.toyType].partsToSnap++;
        }
        Debug.Log($"Toy unsnapped: {arg0.toyType}. Parts left: {toyPartsRemaining[arg0.toyType]}");
    }

    private void CheckToyCompletion(Toy arg0)
    {
        if (toyPartsRemaining.ContainsKey(arg0.toyType) && toyPartsRemaining[arg0.toyType].partsToSnap <= 0)
        {
            OnToyComplete(arg0);
        }
    }

    private void OnToyComplete(Toy arg0)
    {
        switch (arg0.toyType)
        {
            case ToyType.Airplane:
                Debug.Log("Airplane assembly complete!");
                Debug.LogWarning("Done");
                //TODO: Additional logic
                _AnimationManager.AeroplaneAnim();
            break;
            case ToyType.Robot:
                Debug.Log("Robot assembly complete!");
                //TODO: Additional logic
            break;
            case ToyType.Lego:
                Debug.Log("Lego assembly complete!");
                //TODO: Additional logic
            break;
            default:
                Debug.Log("Unknown toy assembly complete!");
                break;
        }
    }
}