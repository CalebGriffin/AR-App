using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class inherits from the Infocard class and adds the relevant information to the infocard object
public class Infocard2D : Infocard
{
    [SerializeField] private ShapeInfo2D shapeInfo2D; // The scriptable object that has all of the shape information which will be on the infocard object

    // Set up the text for each text box on the infocard by first checking that the text box exists
    protected override void TextSetUp()
    {
        if (textboxObjs.Length - 1 >= 0)
        {
            textboxObjs[0].text = $"This is a {shapeInfo2D.shapeName}";
        }
        if (textboxObjs.Length - 1 >= 1)
        {
            textboxObjs[1].text = $"It is a 2D shape";
        }
        if (textboxObjs.Length - 1 >= 2)
        {
            textboxObjs[2].text = $"It has {shapeInfo2D.noOfSides} sides";
        }
        if (textboxObjs.Length - 1 >= 3)
        {
            textboxObjs[3].text = $"It has {shapeInfo2D.noOfCorners} corners";
        }
        if (textboxObjs.Length - 1 >= 4)
        {
            textboxObjs[4].text = $"It has {shapeInfo2D.angles} angles";
        }
        if (textboxObjs.Length - 1 >= 5)
        {
            textboxObjs[5].text = $"It has {shapeInfo2D.linesOfSymmetry} lines of symmetry";
        }
        if (textboxObjs.Length - 1 >= 6)
        {
            textboxObjs[6].text = $"It has {shapeInfo2D.noOfParallelLines} set{shapeInfo2D.s1} of parallel lines and {shapeInfo2D.noOfPerpendicularLines} set{shapeInfo2D.s2} of perpendicular lines";
        }
    }
}
