using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuControlsButton : MonoBehaviour, ISelectHandler
{
    public int playerId;
    public InputAction action;
    [HideInInspector] public string playerPrefsId;
    [HideInInspector] public KeyCode keyCode;

    [Header("Components")]
    public TextMeshProUGUI keyCodeText;
    [HideInInspector] public OptionsMenu optionsMenu;

    private void Awake()
    {
        if (playerId != 1 && playerId != 2)
        {
            Debug.LogError("MenuControlButton.Init(): Player id of " + Utils.GetFullName(transform)
            + " is set to " + playerId + ".");
            playerId = 1;
        }

        playerPrefsId = "P" + playerId + "_" + action.ToString();
    }

    public void Init(OptionsMenu optionsMenu)
    {
        this.optionsMenu = optionsMenu;
        UpdateButton();
        GetComponent<Button>().onClick.AddListener(delegate
        {
            optionsMenu.StartListeningKey(this);
        });
    }

    public void UpdateButton()
    {
        keyCode = playerId == 1 ? InputManager.Player1[0][action] : InputManager.Player2[0][action];
        keyCodeText.text = keyCode.ToString();
    }

    public void OnSelect(BaseEventData eventData)
    {
        optionsMenu.HandleSelectTrigger(GetComponent<Selectable>());
    }
}
