using UnityEngine;
using System;

[Serializable]
public class WonnaTransformDatas
{
    public Vector3 position;
    public Vector3 scale;
    public Quaternion rotation;

    public WonnaTransformDatas(Vector3 position, Vector3 scale, Quaternion rotation)
    {
        this.position = position;
        this.scale = scale;
        this.rotation = rotation;
    }

    public WonnaTransformDatas(Transform transform)
    {
        position = transform.position;
        scale = transform.localScale;
        rotation = transform.rotation;
    }
}