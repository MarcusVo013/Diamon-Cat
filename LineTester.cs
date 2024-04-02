using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTester : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private LineController lines;
    void Start()
    {
        lines.SetUpLine(points);
    }
}
