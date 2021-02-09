using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class line
{
    public static Vector3[] bezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, int num)
    {
        var result = new Vector3[num + 1];
        var n = 1 / num;
        for (int i = 0; i < num + 1; i++)
        {
            float t = n * i;
            float t_ = 1 - t;
            result[i] = (t_ * a + t * b) * t_ + (t_ * c + t * d) * t;
        }
        return result;
    }
    public static Vector3[] rotation(Vector3[] val, float rad, int num)
    {
        var h = val.Length;
        var result = new Vector3[h * num + 1];
        var theta = rad / num;
        return result;
    }
}
