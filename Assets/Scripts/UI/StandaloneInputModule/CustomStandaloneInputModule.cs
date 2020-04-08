using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomStandaloneInputModule : StandaloneInputModule
{
    protected override void Awake() {
        m_InputOverride = gameObject.AddComponent<CustomBaseInput>();
    }
}
