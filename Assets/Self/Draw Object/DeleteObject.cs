using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DeleteObject : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<XRRayInteractor>().selectEntered.AddListener(selected);
    }
    private void Select(SelectEnterEventArgs args)
    {
        Debug.Log("why am i not working please explain? i know why: placed hasnt been set!");
        if(args.interactableObject.transform.gameObject.tag != "Corner"
            && args.interactableObject.transform.gameObject.GetComponent<ObjectData>().placed)
        {
            Destroy(args.interactableObject.transform.gameObject);
        }
    }
    protected virtual void selected(SelectEnterEventArgs args) => Select(args);
}
