using System;
using UnityEngine;

public class ParticleSystemHelper
{
    public static void SetEmmisionActive(ParticleSystem ps, bool active)
    {
        if (ps == null)
        {
            return;
        }
        var temp = ps.emission;

        temp.enabled = active;
    }

    public static void SetEmmisionActiveRecursive(ParticleSystem ps, bool active)
    {
        if (ps == null)
        {
            return;
        }
        ParticleSystemHelper.SetEmmisionActive(ps, active);
        Transform transform = ps.transform;
        for (int i = 0; i < transform.childCount; i++)
        {
            ParticleSystemHelper.SetEmmisionActiveRecursive(transform.GetChild(i).GetComponent<ParticleSystem>(), active);
        }
    }
}
