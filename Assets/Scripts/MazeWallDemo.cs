using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeWallDemo : MazeWall {

    public override void SetAsPassage()
    {
        base.SetAsPassage();
        foreach(Renderer r in renderers)
        {
            r.material.color = Color.red;
        }
    }
}
