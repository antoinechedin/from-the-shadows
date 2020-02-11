using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class testClass
{
    [SerializeField]
    private int nb;
    [SerializeField]
    public StringIntDictionary metaInt;
    [SerializeField]
    public StringFloatDictionary metaFloat;

    public testClass(int n, StringIntDictionary mInt, StringFloatDictionary mFloat)
    {
        nb = n;
        metaInt = mInt;
        metaFloat = mFloat;
    }
}
