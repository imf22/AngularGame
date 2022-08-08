using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockChildTransform : MonoBehaviour
{
    public Vector3 Offset = Vector3.zero;
    private Quaternion iniRot;
 
    void Start()
    {
        iniRot = transform.rotation;
    }

    void Update()
    {
        transform.rotation = iniRot;
        transform.position = this.transform.parent.transform.position + Offset;
    }
}
