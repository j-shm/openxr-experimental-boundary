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
    private float initalScale;
    private GameObject pivot;
    private MeshCollider objSelectedMeshRend;
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
        Resize();
        Destroy(selector);
        objSelected.transform.SetParent(objSelectedParent, true);
        objSelected.transform.localScale = GetAbsoluteDirection(objSelected.transform.localScale);
        objSelectedParent = null;
        objSelected = null;
        selector = null;
        Destroy(pivot);
        pivot = null;
        objSelectedMeshRend = null;

        
    }
    private void Resize()
    {
        pivot.transform.localScale = MakeVectorBaseOne(Vector3.Scale(-dirToScale, selector.transform.position - pivot.transform.position) / FloatFromVector(objSelected.transform.localScale, dirToScale));
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
            var gkjfdgjkdfg = new GameObject("LOOK HERE!!");
            gkjfdgjkdfg.transform.position = res.point;
            gkjfdgjkdfg.transform.SetParent(objSelected.transform,true);
            selector.transform.SetParent(objSelected.transform, true);
            dirToScale = FindLargestDirection(selector.transform.localPosition);
            selector.transform.SetParent(null, true);
            Rigidbody selRB = selector.GetComponent<Rigidbody>();
            initalScale = Mathf.Sign(FloatFromVector(objSelected.transform.localScale, dirToScale));
            pivot = new GameObject("Pivot");
            objSelectedMeshRend = objSelected.GetComponent<MeshCollider>();
            Vector3 absDir = GetAbsoluteDirection(dirToScale);
            int dirSign = (int)Mathf.Sign(FloatFromVector(dirToScale, dirToScale));
            float extent = FloatFromVector(objSelectedMeshRend.bounds.extents, dirToScale);
            pivot.transform.position = objSelected.transform.position;
            objSelected.transform.SetParent(pivot.transform);
            pivot.transform.position += dirToScale * extent;
            objSelected.transform.localPosition += -dirToScale * extent;

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
