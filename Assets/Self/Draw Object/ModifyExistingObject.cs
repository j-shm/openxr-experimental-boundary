using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(DrawObject))]
public class ModifyExistingObject : MonoBehaviour
{
    [SerializeField]
    private XRRayInteractor objectRayInteractor;
    private XRInteractionManager man;
    private XRRayInteractor rayInteractor;

    [SerializeField]
    private GameObject selectorToSpawn;
    private GameObject selector;

    private GameObject objSelected;
    private Transform objSelectedParent;

    private DrawObject resizer;

    private Vector3 dirToScale;
    void Start()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
        man = rayInteractor.interactionManager;
        rayInteractor.selectEntered.AddListener(selected);
        resizer = GetComponent<DrawObject>();
    }
    private void Select(SelectEnterEventArgs args)
    {
        if(objSelected != null || selector != null)
        {
            return;
        }
        Debug.Log(args.interactableObject.transform.gameObject);
        Debug.Log(args.interactableObject.transform.gameObject.GetComponent<ObjectData>());
        if (args.interactableObject.transform.gameObject.GetComponent<ObjectData>().placed)
        {
            Debug.Log("we're getting to the selected part!");
            objSelected = args.interactableObject.transform.gameObject;
            objSelectedParent = objSelected.transform.parent;
            objSelected.transform.SetParent(null, true);
            Main();
        }
    }
    private void Deselect(SelectExitEventArgs args)
    {
        resizer.ResizeObject(objSelected, FloatFromVector(objSelected.transform.localScale, dirToScale), dirToScale);
        resizer.ResizeObject(objSelected,
            FloatFromVector(objSelected.transform.position - selector.transform.position,
                GetAbsoluteDirection(dirToScale)),
            dirToScale);
        Destroy(selector);
        objSelected.transform.SetParent(objSelectedParent, true);
        objSelectedParent = null;
        objSelected = null;
        selector = null;
        
    }
    private void Update()
    {

        if (selector != null)
        {
            resizer.ResizeObject(objSelected, FloatFromVector(objSelected.transform.localScale, dirToScale), dirToScale);
            resizer.ResizeObject(objSelected,
                FloatFromVector(objSelected.transform.position - selector.transform.position,
                    GetAbsoluteDirection(dirToScale)),
                dirToScale);
        }
    }
    protected virtual void selected(SelectEnterEventArgs args) => Select(args);
    protected virtual void exited(SelectExitEventArgs args) => Deselect(args);
    private void Main()
    {
        RaycastHit res;
        if (rayInteractor.TryGetCurrent3DRaycastHit(out res))
        {
            selector = Instantiate(selectorToSpawn, res.point, Quaternion.identity);
            
            var spawnedSelectorGrabComp = selector.GetComponent<XRGrabInteractable>();
            man.SelectEnter((IXRSelectInteractor)objectRayInteractor, spawnedSelectorGrabComp);
            spawnedSelectorGrabComp.selectExited.AddListener(exited);
            selector.transform.SetParent(objSelected.transform, true);
            dirToScale = FindLargestDirection(selector.transform.localPosition);
            selector.transform.SetParent(null, true);
            Rigidbody selRB = selector.GetComponent<Rigidbody>();
            Debug.DrawRay(objSelected.transform.position, dirToScale*100);
            Debug.DrawRay(selector.transform.position, dirToScale*100);
            Debug.Log(dirToScale);
            if (dirToScale.x == 0)
            {
                selRB.constraints = RigidbodyConstraints.FreezePositionX;
            }
            if(dirToScale.y == 0)
            {
                selRB.constraints = RigidbodyConstraints.FreezePositionY;
            }
            if (dirToScale.z == 0)
            {
                selRB.constraints = RigidbodyConstraints.FreezePositionZ;
            }
        }
    }
    private Vector3 FindLargestDirection(Vector3 point)
    {
        Vector3 dir = Vector3.left;
        float largestPoint = Mathf.Abs(point.x);
        if (Mathf.Abs(point.y) > largestPoint)
        {
            largestPoint = Mathf.Abs(point.y);
            dir = Vector3.down;
        }
        if (Mathf.Abs(point.z) > largestPoint)
        {
            largestPoint = Mathf.Abs(point.z);
            dir = Vector3.back;
        }
        return Vector3.Scale(dir, new Vector3(Mathf.Sign(point.z), Mathf.Sign(point.y), Mathf.Sign(point.z)));
    }
    
    private float FloatFromVector(Vector3 point, Vector3 direction)
    {
        Vector3 multipliedPoint = Vector3.Scale(point , direction);
        if(multipliedPoint.x != 0)
        {
            return point.x;
        }
        if (multipliedPoint.y != 0)
        {
            return point.y;
        }
        if (multipliedPoint.z != 0)
        {
            return point.z;
        }
        return 0;
    }
    private Vector3 GetAbsoluteDirection(Vector3 direction)
    {
        return new Vector3(Mathf.Abs(direction.x), Mathf.Abs(direction.y), Mathf.Abs(direction.z));
    }
}
