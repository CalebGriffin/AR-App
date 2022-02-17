using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New 3D ShapeInfo", menuName = "3D Shape Info", order = 51)]
public class ShapeInfo3D : ScriptableObject
{
    public string shapeName;
    public string shapeDimension = "3D";
    public string noOfFaces;
    public string noOfEdges;
    public string noOfCorners;
    public string faceShapes;
}