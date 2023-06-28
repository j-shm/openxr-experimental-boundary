using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/*
 *  FIRST PLACE CORNER
 *  THEN PLACE OBJECT BY:
 *  SELECT POINT ON GROUND AND SPAWN SELECTOR
 *  MOVE SELECTOR TO WANTED HEIGHT
 *  SELECT LAST POINT ON GROUND
 */

public class HandleRayTable : MonoBehaviour
{
    //interactors
    [SerializeField]
    private XRRayInteractor objectRayInteractor;
    private XRInteractionManager man;
    private XRRayInteractor rayInteractor;

    private DrawObject objectDrawer;

    //the box selector
    [SerializeField]
    private GameObject selector;

    private GameObject spawnedSelector;
    private LineRenderer spawnedSelectorLineRenderer;

    //the actual object
    [SerializeField]
    private GameObject objToPlace;

    private GameObject placedObject;

    //the corner
    [SerializeField]
    private GameObject cornerToPlace;
    private GameObject corner;

    //other
    private Vector3 startingPoint;
    
    private void Start()
    {   
        rayInteractor = GetComponent<XRRayInteractor>();
        man = rayInteractor.interactionManager;
        objectDrawer = GetComponent<DrawObject>();
    }
    protected virtual void exited(SelectExitEventArgs args) => Deselect(args);


    //we have picked a height now we will spawn the object
    private void Deselect(SelectExitEventArgs args)
    { 
        placedObject = Instantiate(objToPlace, startingPoint, Quaternion.identity);
    }
    private void Update()
    {
        //resize the object dynamically to the raycast end point whilst the object is still hasn't been finalised.
        if (placedObject != null && rayInteractor.isHoverActive)
        {
            RaycastHit res;
            if (rayInteractor.TryGetCurrent3DRaycastHit(out res))
            {
                objectDrawer.ResizeObject(placedObject, startingPoint, spawnedSelector.transform.position, res.point);
            }
        }
    }
    public void Select()
    {
        //object ray is busy holding something else
        if (objectRayInteractor.interactablesSelected.Count != 0)
        {
            return;
        }

        RaycastHit res;
        if (rayInteractor.TryGetCurrent3DRaycastHit(out res))
        {
            if(corner == null)
            {
                corner = Instantiate(cornerToPlace, res.point, Quaternion.identity);
                return;
            }
            if (spawnedSelector == null)
            {
                startingPoint = res.point;

                spawnedSelector = Instantiate(selector, startingPoint, Quaternion.identity);
                var spawnedSelectorGrabComp = spawnedSelector.GetComponent<XRGrabInteractable>();

                /* 
                 * this is used to force the main ray interactor to pick up the spawnedSelecter that we use to find the height
                 * there should be a way to get IXRSelectInteractor instead of this obselete method of base interactors however i cannot find how to get the
                 * interactor portion only the interactable so this will have to do.
                 */
                man.SelectEnter(objectRayInteractor, spawnedSelectorGrabComp);

                //add listener for when the height has been selected by the user deselecting the item
                spawnedSelectorGrabComp.selectExited.AddListener(exited); 
                return;
            }
            /* clean up: remove all the object references and get ready for the next one*/
            objectDrawer.ResizeObject(placedObject, startingPoint, spawnedSelector.transform.position, res.point);
            placedObject.GetComponent<ObjectData>().placed = true;
            CleanUp();
        }
    }

    private void CleanUp()
    {
        if(spawnedSelector != null)
        {
            Destroy(spawnedSelector);
            spawnedSelector = null;
        }

        if(placedObject != null)
        {
            if(corner != null)
            {
                placedObject.transform.SetParent(corner.transform, true);
            }
            placedObject = null;
        }

    }

    //used for the loading system
    public void FullCleanUp()
    {
        if (corner != null)
        {
            Destroy(corner);
            corner = null;
        }
        CleanUp();
    }
    public void Setup(GameObject corner)
    {
        this.corner = corner;
    }
}
