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


[Serializable]
public class WonnRangeDatas
{
    public float min = 0;
    public float max = 0;
}


[Serializable]
public class WonnaTimeDatas
{
    [Range(0, 59f)] public float minute;
    [Range(0, 59f)] public float seconds;


    public WonnaTimeDatas(float minute, float seconds)
    {
        this.minute = minute;
        this.seconds = seconds;
    }

    public WonnaTimeDatas()
    {
        this.minute = 0;
        this.seconds = 0;
    }
}