using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SnapEvent : UnityEvent<Toy>
{
    
}
public class SnapInteractableExtended : SnapInteractable
{
    public static SnapEvent onObjectSnapped = new SnapEvent();
    public static SnapEvent onObjectUnsnapped = new SnapEvent();

    protected override void SelectingInteractorAdded(SnapInteractor interactor)
    {
        base.SelectingInteractorAdded(interactor);
        var toy = GetComponentInParent<Toy>();
        onObjectSnapped.Invoke(toy);
    }

    protected override void SelectingInteractorRemoved(SnapInteractor interactor)
    {
        base.SelectingInteractorRemoved(interactor);
        var toy = GetComponentInParent<Toy>();
        onObjectUnsnapped.Invoke(toy);
    }
}