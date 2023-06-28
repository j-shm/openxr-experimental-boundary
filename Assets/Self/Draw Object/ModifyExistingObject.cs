using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ModifyExistingObject : MonoBehaviour
{
    [SerializeField]
    private XRRayInteractor objectRayInteractor;
    private XRInteractionManager man;
    private XRRayInteractor rayInteractor;

    [SerializeField]
    private GameObject selectorToSpawn;
    private GameObject selector;
    void Start()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
        man = rayInteractor.interactionManager;
        rayInteractor.selectEntered.AddListener(selected);
    }
    private void Select(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.gameObject.tag != "Corner"
            && args.interactableObject.transform.gameObject.GetComponent<ObjectData>().placed)
        {
            Main();
        }
    }
    protected virtual void selected(SelectEnterEventArgs args) => Select(args);

    private void Main()
    {
        RaycastHit res;
        if (rayInteractor.TryGetCurrent3DRaycastHit(out res))
        {
            selector = Instantiate(selectorToSpawn, res.point, Quaternion.identity);

            var spawnedSelectorGrabComp = selector.GetComponent<XRGrabInteractable>();
            man.SelectEnter(objectRayInteractor, spawnedSelectorGrabComp);

            //spawnedSelectorGrabComp.selectExited.AddListener(exited);
        }
    }
}
