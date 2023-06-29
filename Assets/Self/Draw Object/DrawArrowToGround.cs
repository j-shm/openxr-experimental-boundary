using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawArrowToGround : MonoBehaviour
{
    private LineRenderer lineRend;
    private void Start()
    {
        lineRend = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        Vector3[] pos = {transform.position, Vector3.Scale(transform.position, (Vector3.right + Vector3.forward))};
        lineRend.SetPositions(pos);
    }
}
