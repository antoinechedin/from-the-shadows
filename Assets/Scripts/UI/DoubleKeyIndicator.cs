using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoubleKeyIndicator : MonoBehaviour
{
    public InputAction action;

    private KeyIndicator keyboard;
    private KeyIndicator gamepad;

    private void Awake()
    {
        gamepad = transform.GetChild(0).GetComponent<KeyIndicator>();
        keyboard = transform.GetChild(1).GetComponent<KeyIndicator>();

        UpdateIndicators();
    }

    private void OnEnable()
    {
        GameManager.Instance.optionsUpdateDelegate += UpdateIndicators;
        GameManager.Instance.controllerChangeDelegate += UpdateIndicators;
    }

    public void UpdateIndicators()
    {
        if (
            GameManager.Instance.player1InputDevice == InputDevice.Keyboard
            && GameManager.Instance.player2InputDevice == InputDevice.Keyboard
        )
        {
            gamepad.gameObject.SetActive(false);
            keyboard.gameObject.SetActive(true);

            keyboard.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            keyboard.UpdateIndicator(InputDevice.Keyboard, action);

        }
        else
        {
            gamepad.gameObject.SetActive(true);
            gamepad.UpdateIndicator(InputDevice.Controller, action);

            if (GameManager.Instance.player1InputDevice != GameManager.Instance.player2InputDevice)
            {
                keyboard.gameObject.SetActive(true);
                keyboard.GetComponent<RectTransform>().anchoredPosition = Vector2.left * 85f;
                keyboard.UpdateIndicator(InputDevice.Keyboard, action);
            }
            else
            {
                keyboard.gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.optionsUpdateDelegate -= UpdateIndicators;
            GameManager.Instance.controllerChangeDelegate -= UpdateIndicators;
        }
    }
}
