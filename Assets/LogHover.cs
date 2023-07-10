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
        rayInteractor.hoverExited.AddListener(hoveredexied);
    }
    public void Hover(HoverEnterEventArgs args)
    {
        Debug.Log(args.interactableObject.transform.gameObject);
    }
    protected virtual void hovered(HoverEnterEventArgs args) => Hover(args);
    public void HoverExit(HoverExitEventArgs args)
    {
        RaycastHit res;
        if (args.interactorObject.transform.gameObject.GetComponent<XRRayInteractor>().TryGetCurrent3DRaycastHit(out res))
        {
            Debug.Log(res.collider.gameObject);
        }
    }
    protected virtual void hoveredexied(HoverExitEventArgs args) => HoverExit(args);
}
