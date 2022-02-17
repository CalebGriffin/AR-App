using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infocard3D : Infocard
{
    [SerializeField] private ShapeInfo3D shapeInfo3D;

    protected override void TextSetUp()
    {
        if (textboxObjs.Length - 1 >= 0)
        {
            textboxObjs[0].text = $"This is a {shapeInfo3D.shapeName}";
        }
        if (textboxObjs.Length - 1 >= 1)
        {
            textboxObjs[1].text = $"It is a 3D shape";
        }
        if (textboxObjs.Length - 1 >= 2)
        {
            textboxObjs[2].text = $"It has {shapeInfo3D.noOfFaces} faces";
        }
        if (textboxObjs.Length - 1 >= 3)
        {
            textboxObjs[3].text = $"It has {shapeInfo3D.noOfEdges} edges";
        }
        if (textboxObjs.Length - 1 >= 4)
        {
            textboxObjs[4].text = $"It has {shapeInfo3D.noOfCorners} corners/vertices";
        }
        if (textboxObjs.Length - 1 >= 5)
        {
            textboxObjs[5].text = $"It's faces are {shapeInfo3D.faceShapes}";
        }
    }
}