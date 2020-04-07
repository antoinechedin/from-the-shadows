using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuControlsButton : MonoBehaviour, ISelectHandler
{
    public string playerPrefsId;
    public KeyCode fallbackKeycode;
    [HideInInspector] public KeyCode keyCode;

    [Header("Components")]
    public TextMeshProUGUI keyCodeText;
    [HideInInspector] public OptionsMenu optionsMenu;

    public void Init(OptionsMenu optionsMenu)
    {
        this.optionsMenu = optionsMenu;
        UpdateButton();
        GetComponent<Button>().onClick.AddListener(delegate {
            optionsMenu.StartListeningKey(this);
        });
    }

    public void UpdateButton()
    {
        keyCode = (KeyCode)PlayerPrefs.GetInt(playerPrefsId, (int)fallbackKeycode);
        keyCodeText.text = keyCode.ToString();
    }

    public void OnSelect(BaseEventData eventData)
    {
        optionsMenu.HandleSelectTrigger(GetComponent<Selectable>());
    }
}
