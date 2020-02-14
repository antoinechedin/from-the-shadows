using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class testClass
{
    [SerializeField]
    private SerializableDate date;

    public testClass(SerializableDate d)
    {
        date = d;
    }
}
