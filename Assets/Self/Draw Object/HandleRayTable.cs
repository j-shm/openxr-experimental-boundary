﻿using System.Collections;
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
    
    [SerializeField]
    private GameObject selectorPivot;
    private GameObject spawnedSelectorPivot;
    private GameObject spawnedSelector;

    //the actual object
    [SerializeField]
    private GameObject objToPlace;

    private GameObject placedObject;

    //the corner
    [SerializeField]
    private GameObject cornerToPlace;
    private GameObject corner;

    //other
    [SerializeField]
    private UserInterfaceHandler uiHandler;
    private Vector3 startingPoint;

    

    private void Start()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
        man = rayInteractor.interactionManager;
        objectDrawer = GetComponent<DrawObject>();
    }
    
    protected virtual void exitedHeightSelector(SelectExitEventArgs args) => DeselectHeight(args);
    protected virtual void exitedPivotSelector(SelectExitEventArgs args) => DeselectPivot(args);
    
    private void DeselectHeight(SelectExitEventArgs args)
    {
        uiHandler.NextStage();
        ((XRGrabInteractable)args.interactableObject).enabled = false;
        args.interactableObject.transform.gameObject.GetComponent<Collider>().enabled = false;
        spawnedSelectorPivot = Instantiate(selectorPivot, args.interactableObject.transform);
        var spawnedSelectorPivotGrabComp = spawnedSelectorPivot.GetComponent<XRGrabInteractable>();
        spawnedSelectorPivot.transform.SetParent(null,true);
        spawnedSelectorPivot.GetComponent<DrawLineToPoint>().SetPoint(spawnedSelector.transform.position);
        man.SelectEnter((IXRSelectInteractor)objectRayInteractor, spawnedSelectorPivotGrabComp);
        
        spawnedSelectorPivotGrabComp.selectExited.AddListener(exitedPivotSelector);
        StartCoroutine(TurnOffOthers());
    }
    //this is stupid: but im not sure a better way to handle LimitDrawingToOneObject
    //there is .1 second where you are able to break it.
    private IEnumerator TurnOffOthers()
    {
        var script = this.gameObject.transform.parent.GetComponent<LimitDrawingToOneObject>();
        script.SelectObject(objectRayInteractor.gameObject);
        yield return new WaitForSeconds(1f);
        script.SelectObject(objectRayInteractor.gameObject);
        yield return new WaitForSeconds(.1f);
        script.SelectObject(objectRayInteractor.gameObject);
    }
    private void DeselectPivot(SelectExitEventArgs args)
    {
        uiHandler.NextStage();
        placedObject = Instantiate(objToPlace, startingPoint, Quaternion.identity);
        placedObject.transform.LookAt(spawnedSelectorPivot.transform); 
        placedObject.transform.eulerAngles = Vector3.Scale(placedObject.transform.eulerAngles, Vector3.up);
        objectDrawer.FirstResize(placedObject,spawnedSelector.transform.position.y - startingPoint.y,spawnedSelectorPivot);
    }
    

    private void Update()
    {
        //resize the object dynamically to the raycast end point whilst the object is still hasn't been finalised.
        if (placedObject != null && rayInteractor.isHoverActive)
        {
            RaycastHit res;
            if (rayInteractor.TryGetCurrent3DRaycastHit(out res))
            {
                if (res.point.y < startingPoint.y) return;
                Resize(res.point);
            }
        }
    }
   
    private void Resize(Vector3 addPoint)
    {
        //again destorying a lot is bad
        GameObject addPointObj = new GameObject("addPoint");
        addPointObj.transform.position = addPoint; 
        objectDrawer.SecondResize(placedObject,addPointObj);
        addPointObj.transform.SetParent(placedObject.transform,true);
        Destroy(addPointObj);
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
                uiHandler.NextStage();
                return;
            }
            if (spawnedSelector == null)
            {
                startingPoint = res.point;

                spawnedSelector = Instantiate(selector, startingPoint, Quaternion.identity);
                var spawnedSelectorGrabComp = spawnedSelector.GetComponent<XRGrabInteractable>();
                spawnedSelector.GetComponent<DrawLineToPoint>().SetPoint(startingPoint);
                //force pickup
                man.SelectEnter((IXRSelectInteractor)objectRayInteractor, spawnedSelectorGrabComp);

                //add listener for when the height has been selected by the user deselecting the item
                spawnedSelectorGrabComp.selectExited.AddListener(exitedHeightSelector);
                uiHandler.NextStage();
                return;
            }
            if (res.point.y < startingPoint.y) return;
            /* clean up: remove all the object references and get ready for the next one*/
            Resize(res.point);
            ObjectData objData = placedObject.GetComponent<ObjectData>();
            objData.placed = true;
            objData.objectType = uiHandler.GetObjectType();
            CleanUp();
        }
    }

    private void CleanUp()
    {
        uiHandler.ResetStage();
        if (spawnedSelector != null)
        {
            Destroy(spawnedSelector);
            spawnedSelector = null;
        }
        if(spawnedSelectorPivot != null)
        {
            Destroy(spawnedSelectorPivot);
            spawnedSelectorPivot = null;
        }

        if(placedObject != null)
        {
            placedObject.transform.localScale = GetAbsoluteVector(placedObject.transform.localScale);
            placedObject.GetComponent<Collider>().enabled = true;
            if(corner != null)
            {
                placedObject.transform.SetParent(corner.transform, true);
            }
            placedObject = null;
        }

    }
    private Vector3 GetAbsoluteVector(Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
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
