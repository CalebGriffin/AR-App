using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New 2D ShapeInfo", menuName = "2D Shape Info", order = 51)]
public class ShapeInfo2D : ScriptableObject
{
    public string shapeName;
    public string shapeDimension = "2D";
    public string noOfSides;
    public string noOfCorners;
    public string angles;
    public string linesOfSymmetry;
    public string noOfParallelLines;
    public string s1;
    public string noOfPerpendicularLines;
    public string s2;
}
