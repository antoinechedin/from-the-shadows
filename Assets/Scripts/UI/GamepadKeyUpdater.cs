using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamepadKeyUpdater : MonoBehaviour
{
    public InputAction action;
    public Sprite[] gamepadKeySprites;
    [Header("Components")]
    public Image gamepadKeyImage;

    public void UpdateGamepadKeyImage()
    {
        gamepadKeyImage.sprite = GetGamepadKeySprite(action);
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
            case InputAction.Up:
            case InputAction.Down:
            case InputAction.Left:
            case InputAction.Right:
                return gamepadKeySprites[8];
            case InputAction.Interact:
                return gamepadKeySprites[15];
            case InputAction.Switch:
                return gamepadKeySprites[7];
            default:
                return gamepadKeySprites[14];
        }
    }
}
