using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    private Transform[] points;
    [SerializeField]private float smoothness = 10;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    public void SetUpLine(Transform[] points)
    {
        lr.positionCount = points.Length;
        this.points = points;
    }
    private void Update()
    {
        for(int i = 0 ; i < points.Length; i++)
        {
            lr.SetPosition(i, points[i].position);
            Vector3 targetPos = points[i].position;
            Vector3 currentPos = lr.GetPosition(i);
            Vector3 smoothedPos = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * smoothness);

            lr.SetPosition(i, smoothedPos);
        }
    }
}
