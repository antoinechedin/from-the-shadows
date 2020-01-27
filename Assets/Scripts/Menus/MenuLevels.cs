using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuLevels : MonoBehaviour
{
    public UnityEngine.UI.GridLayoutGroup buttonsGroup;
    public GameObject buttonPrefab;

    public void SetMenuLevels(int completedLevels, int totalLevels)
    {
        foreach (Transform child in buttonsGroup.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < totalLevels; i++)
        {
            int levelNumber = i;
            GameObject button = Instantiate(buttonPrefab, buttonsGroup.transform);
            button.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "" + (i + 1);
            button.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate { LevelButtonClicked(levelNumber); });

            if (i == 0) EventSystem.current.SetSelectedGameObject(button.gameObject);
        }
    }

    private static void LevelButtonClicked(int number)
    {
        // TODO: Launch the selected level
        Debug.Log("You have clicked on level " + number);
    }

}
