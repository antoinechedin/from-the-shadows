using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopulateMetadata : MonoBehaviour
{
    public List<TextMeshProUGUI> listToPopulate;
    private List<string> numberPopulate;
    // Start is called before the first frame update
    void Start()
    {
        Populate();
    }

    private void Populate()
    {
        numberPopulate = new List<string>();

        numberPopulate.Add("Nombre de saut :\t\t\t" +PlayerPrefs.GetInt("jumpNumber", 0));

        for (int i = 0; i < listToPopulate.Count; i++)
        {
            listToPopulate[i].text = numberPopulate[i];
        }
    }
}
