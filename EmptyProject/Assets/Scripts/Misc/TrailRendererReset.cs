using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailRendererReset : MonoBehaviour
{
    public TrailRenderer tr;

    public virtual void OnEnable()
    {
        tr.Clear();
    }

    public virtual void OnDisable()
    {
        tr.Clear();
    }
}
