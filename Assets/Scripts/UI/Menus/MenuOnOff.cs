using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class MenuOnOff : MonoBehaviour, ISelectHandler
{
    public string playerPrefsId;

    public bool activated = true;

    public TextMeshProUGUI onText;
    public TextMeshProUGUI offText;

    public GameObject optionalText;

    private Animator animator;
    [HideInInspector] public OptionsMenu optionsMenu;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetValue(bool _activated)
    {
        bool changed = false;

        if (activated != _activated)
            changed = true;

        
        if(changed)
        {
            activated = _activated;

            if (activated)
            {
                onText.color = Color.white;
                offText.color = Color.gray;

                animator.SetTrigger("On");

                if (optionalText != null)
                    optionalText.SetActive(true);

                onText.text = "ON";
                offText.text = "Off";
            }
            else
            {
                onText.color = Color.gray;
                offText.color = Color.white;

                animator.SetTrigger("Off");

                if (optionalText != null)
                    optionalText.SetActive(false);

                onText.text = "On";
                offText.text = "OFF";
            }

            GetComponentInParent<AudioSource>().PlayOneShot(optionsMenu.menuManager.uiSelect);
            PlayerPrefs.SetInt(playerPrefsId, activated ? 1 : 0);
            GameManager.Instance.OnOptionUpdate();
        }
    }

    public void Init(int value, OptionsMenu optionsMenu)
    {
        this.optionsMenu = optionsMenu;

        if (value == 1)
        {
            activated = true;
            onText.color = Color.white;
            offText.color = Color.gray;

            if (optionalText != null)
                optionalText.SetActive(true);

            onText.text = "ON";
            offText.text = "Off";
        }
        else
        {
            activated = false;
            onText.color = Color.gray;
            offText.color = Color.white;

            if (optionalText != null)
                optionalText.SetActive(false);

            onText.text = "On";
            offText.text = "OFF";
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        optionsMenu.HandleSelectTrigger(GetComponent<Selectable>());
    }
}
