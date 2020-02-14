using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utils is meant to be used only with static calls. It's a collection of 
/// useful functions that aren't provided by Unity.
/// </summary>
public class Utils
{
    /// <summary>
    /// Return the full name of a gameobjet in the scene hierachy.
    /// For example:
    /// |A
    /// |--B
    /// |--C
    /// |----D
    /// GetFullName(D) will return "A/C/D".
    /// This is mainly use for debug purpose.
    /// </summary>
    /// <param name="obj">the gameobject you want the full name</param>
    /// <returns></returns>
    public static string GetFullName(Transform obj)
    {
        if (obj.parent == null)
            return obj.name;
        else
            return GetFullName(obj.parent) + "/" + obj.name;
    }
}
