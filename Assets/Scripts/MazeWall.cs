using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeWall : MonoBehaviour {

    [HideInInspector]
    public bool passage = false;
    protected Renderer[] renderers;
    protected Collider[] colliders;



    public virtual void Initialize()
    {
        transform.LookAt(Vector3.zero); //look at world center
        transform.LookAt(-transform.forward); // and reverse it
        transform.localPosition += transform.forward * 0.05f; //slightly lift up from ground
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();
    }

    public virtual void SetAsPassage()
    {
        passage = true;
    }

    public bool isPassage()
    {
        return passage;
    }

    public virtual void Disable()
    {
        foreach(Renderer r in renderers)
        {
            r.enabled = false;
        }

        foreach (Collider c in colliders)
        {
            c.enabled = false;
        }
    }


}
