using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyIndicator : MonoBehaviour
{
    public bool shouldListenOnControllerChange;
    [Range(1, 2)] public int playerId = 1;

    [Header("Components")]
    public KeyCapUpdater[] keycaps;
    public GamepadKeyUpdater[] gamepadKeys;

    private void OnEnable()
    {
        if (shouldListenOnControllerChange)
        {
            GameManager.Instance.controllerChangeDelegate += UpdateIndicator;
            GameManager.Instance.optionsUpdateDelegate += UpdateIndicator;
            UpdateIndicator();
        }
    }

    private void Awake()
    {
        if (playerId != 1 && playerId != 2)
        {
            Debug.LogError("KeyIndicator.Awake(): Player id of " + Utils.GetFullName(transform)
             + " is invalid (" + playerId + "). Default value will be used.");
            playerId = 1;
        }
    }

    public void UpdateIndicator()
    {
        InputDevice device =
            playerId == 1 ? GameManager.Instance.player1InputDevice : GameManager.Instance.player2InputDevice;
        UpdateIndicator(device);
    }

    public void UpdateIndicator(InputDevice device)
    {
        if (device == InputDevice.Keyboard)
        {
            foreach (var keycap in keycaps)
            {
                keycap.gameObject.SetActive(true);
                keycap.UpdateKeyText(playerId);
            }
            foreach (var gamepadKey in gamepadKeys)
            {
                gamepadKey.gameObject.SetActive(false);
            }

        }
        else
        {
            foreach (var keycap in keycaps)
            {
                keycap.gameObject.SetActive(false);
            }
            foreach (var gamepadKey in gamepadKeys)
            {
                gamepadKey.gameObject.SetActive(true);
                gamepadKey.UpdateGamepadKeyImage();
            }
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            if (shouldListenOnControllerChange)
            {
                GameManager.Instance.controllerChangeDelegate -= UpdateIndicator;
                GameManager.Instance.optionsUpdateDelegate -= UpdateIndicator;
            }
        }
    }
}
