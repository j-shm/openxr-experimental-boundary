using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;
public class LimitDrawingToOneObject : MonoBehaviour
{
    private List<XRRayInteractor> rayInteractors = new List<XRRayInteractor>();
    private List<XRInteractorLineVisual> rayInteractorsVisuals = new List<XRInteractorLineVisual>();
    private List<GameObject> rayInteractorsGameObjects = new List<GameObject>();

    private void Start()
    {
        rayInteractors = transform.GetComponentsInChildren<XRRayInteractor>().ToList<XRRayInteractor>();
        foreach(XRRayInteractor rayInteractor in rayInteractors)
        {
            GameObject rayObject = rayInteractor.gameObject;
            rayInteractorsVisuals.Add(rayObject.GetComponent<XRInteractorLineVisual>());
            rayInteractorsGameObjects.Add(rayObject);
            if(rayObject.name != "Ground For Thingy") //this needs fixed to a tag or smth
            {
                rayInteractor.selectEntered.AddListener(selected);
                rayInteractor.selectExited.AddListener(exited);
            }

        }
    }
    protected virtual void selected(SelectEnterEventArgs args) => Select(args);
    protected virtual void exited(SelectExitEventArgs args) => Deselect(args);
    private void Select(SelectEnterEventArgs args)
    {
        for (int i =0; i < rayInteractorsVisuals.Count; i++)
        {
            rayInteractorsVisuals[i].enabled = false;
            Debug.Log(rayInteractorsVisuals[i].gameObject.name);
        }
        rayInteractorsVisuals[rayInteractorsGameObjects.IndexOf(args.interactorObject.transform.gameObject)].enabled = true;
    }
    private void Deselect(SelectExitEventArgs args)
    {
        for (int i = 0; i < rayInteractorsVisuals.Count; i++)
        {
            rayInteractorsVisuals[i].enabled = true;
        }
    }
}
