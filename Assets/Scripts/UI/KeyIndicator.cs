using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyIndicator : MonoBehaviour
{
    [Range(1, 2)] public int playerId = 1;
    [SerializeField] private Sprite[] gamepadKeySprites;
    
    [Header("Components")]
    public GameObject keycap;
    public TextMeshProUGUI keyText;
    public GameObject gamepadKey;

    private void Awake()
    {
        if (playerId != 1 && playerId != 2)
        {
            Debug.LogError("KeyIndicator.Awake(): Player id of " + Utils.GetFullName(transform)
             + " is invalid (" + playerId + "). Default value will be used.");
            playerId = 1;
        }
    }

    public void UpdateIndicator(InputDevice device, InputAction action)
    {
        if (device == InputDevice.Keyboard)
        {
            keycap.SetActive(true);
            gamepadKey.SetActive(false);

            KeyCode keyCode = playerId == 1 ? InputManager.Player1[0][action] : InputManager.Player2[0][action];
            keyText.text = keyCode.ToString();

            if (keyText.text.Length > 1)
            {
                keyText.fontSize = 40;
                keyText.fontStyle = FontStyles.Normal;
                keyText.alignment = TextAlignmentOptions.TopLeft;
            }
        }
        else
        {
            keycap.SetActive(false);
            gamepadKey.SetActive(true);

            gamepadKey.GetComponent<Image>().sprite = GetGamepadKeySprite(action);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(keycap.GetComponent<RectTransform>());
    }

    private Sprite GetGamepadKeySprite(InputAction action)
    {
        switch (action)
        {
            case InputAction.Jump:
            case InputAction.Select:
                return gamepadKeySprites[0];
            case InputAction.Return:
                return gamepadKeySprites[1];
            default:
                return gamepadKeySprites[14];
        }
    }
}
