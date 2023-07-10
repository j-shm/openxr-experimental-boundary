using Unity.XR.PXR;
using UnityEngine;
using System.Collections;

public class EnablePassthrough : MonoBehaviour
{
    private void Awake()
    {
        PXR_Boundary.EnableSeeThroughManual(true);
    }
    void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            PXR_Boundary.EnableSeeThroughManual(true);
        }
    }
}
