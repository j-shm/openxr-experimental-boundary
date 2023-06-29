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
            if(rayObject.name != "Ground For Thingy" && rayObject.name != "Expand Ray Interactor") //this needs fixed to a tag or smth
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
        GameObject obj = args.interactorObject.transform.gameObject;
        for (int i =0; i < rayInteractorsVisuals.Count; i++)
        {
            if(rayInteractorsVisuals[i].gameObject != obj)
            {
                rayInteractorsVisuals[i].enabled = false;
                rayInteractors[i].allowSelect = false;
            }
        }
    }
    private void Deselect(SelectExitEventArgs args)
    {
        StartCoroutine(DoDeselect());
    }
    private IEnumerator DoDeselect()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < rayInteractorsVisuals.Count; i++)
        {
            rayInteractorsVisuals[i].enabled = true;
            rayInteractors[i].allowSelect = true;
        }
    }
}
