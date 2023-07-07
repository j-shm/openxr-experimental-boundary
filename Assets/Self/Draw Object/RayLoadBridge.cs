using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayLoadBridge : MonoBehaviour
{
    private HandleRayTable rayTable;
    [SerializeField]
    private LoadObjects loadObjects;

    public void Start()
    {
        rayTable = GetComponent<HandleRayTable>();
        loadObjects.onLoad += HandleLoading;
    }
    private void HandleLoading(GameObject corner)
    {
        rayTable.FullCleanUp();
        rayTable.Setup(corner);
    }
}
