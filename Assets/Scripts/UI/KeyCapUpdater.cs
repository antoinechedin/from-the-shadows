using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyCapUpdater : MonoBehaviour
{
    public InputAction action;
    [Header("Components")]
    public TextMeshProUGUI keyText;

    public void UpdateKeyText(int playerId)
    {
        KeyCode keyCode = playerId == 1 ? InputManager.Player1[0][action] : InputManager.Player2[0][action];
        keyText.text = keyCode.ToString();
        UpdateSize();
    }

    [ContextMenu("Update size")]
    public void UpdateSize()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(46 + keyText.text.Length * 18, 64);
    }
}
