using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDate : ISerializationCallbackReceiver
{
    public string dateString;
    public DateTime date;

    public SerializableDate(DateTime d)
    {
        date = d;
    }

    public void OnAfterDeserialize()
    {
        date = DateTime.Parse(dateString);
    }

    public void OnBeforeSerialize()
    {
        dateString = date.ToString();
    }

    public string Print()
    {
        return date.ToString();
    }

    override
    public string ToString()
    {
        return date.ToString();
    }
}
