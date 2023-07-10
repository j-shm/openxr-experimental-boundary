using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LogHover : MonoBehaviour
{
    private void Start()
    {
        var rayInteractor = GetComponent<XRRayInteractor>();
        rayInteractor.hoverEntered.AddListener(hovered);
    }
    public void Hover(HoverEnterEventArgs args)
    {
        Debug.Log(args.interactableObject.transform.gameObject);
    }
    protected virtual void hovered(HoverEnterEventArgs args) => Hover(args);
}
