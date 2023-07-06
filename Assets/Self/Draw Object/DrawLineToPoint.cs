using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawLineToPoint : MonoBehaviour
{
    private LineRenderer lineRend;
    [SerializeField]
    private Vector3 point;
    private void Start()
    {
        lineRend = GetComponent<LineRenderer>();
    }
    public void SetPoint(Vector3 point)
    {
        this.point = point;
    }
    private void Update()
    {
        Vector3[] pos = { transform.position, point };
        lineRend.SetPositions(pos);
    }
}
