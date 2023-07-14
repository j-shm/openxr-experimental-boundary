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
    private DrawObject objectDrawer;


    private Vector3 dirToScale;

    private GameObject pivot;
    private MeshCollider objSelectedMeshCol;
    private GameObject selectorHolder;
    void Start()
    {
        rayInteractor = GetComponent<XRRayInteractor>();
        man = rayInteractor.interactionManager;
        rayInteractor.selectEntered.AddListener(selected);
        objectDrawer = GetComponent<DrawObject>();
    }
    private void Select(SelectEnterEventArgs args)
    {
        if(objSelected != null || selector != null)
        {
            return;
        }

        if (args.interactableObject.transform.gameObject.GetComponent<ObjectData>().placed)
        {
            objSelected = args.interactableObject.transform.gameObject;
            objSelectedParent = objSelected.transform.parent;
            objSelected.transform.SetParent(null, true);
            Main();
        }
    }
    private void Deselect(SelectExitEventArgs args)
    {
        Resize();
        Destroy(selector);
        objSelected.transform.SetParent(objSelectedParent, true);
        objSelected.transform.localScale = GetAbsoluteDirection(objSelected.transform.localScale);
        objSelectedParent = null;
        objSelected = null;
        selector = null;
        Destroy(pivot);
        pivot = null;
        objSelectedMeshCol = null;

        
    }
    private void Resize()
    {
        objectDrawer.ModifyResize(objSelected, selector, dirToScale);

    }
    private void Update()
    {

        if (selector != null)
        {
            Resize();
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
            Vector3 absDir = GetAbsoluteDirection(dirToScale);

            selectorHolder = new GameObject("selector holder");
            selectorHolder.transform.position = objSelected.transform.position;
            selectorHolder.transform.rotation = objSelected.transform.rotation;
            selector.transform.SetParent(selectorHolder.transform, true);
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
        return Vector3.Scale(dir, new Vector3(Mathf.Sign(point.x), Mathf.Sign(point.y), Mathf.Sign(point.z)));
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
    private Vector3 MakeVectorBaseOne(Vector3 vector)
    {
        if (vector.x != 0)
        {
            return new Vector3(vector.x, 1, 1);
        }
        if (vector.y != 0)
        {
            return new Vector3(1, vector.y, 1);
        }
        if (vector.z != 0)
        {
            return new Vector3(1, 1, vector.z);
        }
        return vector;
    }
}
