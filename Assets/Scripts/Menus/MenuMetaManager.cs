using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuMetaManager : MonoBehaviour
{
    public GameObject lightGuyValues;
    public GameObject shadowGuyValues;
    public GameObject totalValues;

    private List<TextMeshProUGUI> lightGuyToPopulate;
    private List<TextMeshProUGUI> shadowGuyToPopulate;
    private List<TextMeshProUGUI> totalToPopulate;
    // Start is called before the first frame update
    void Start()
    {
        lightGuyToPopulate = new List<TextMeshProUGUI>();
        shadowGuyToPopulate = new List<TextMeshProUGUI>();
        totalToPopulate = new List<TextMeshProUGUI>();

        for (int i = 0; i < lightGuyValues.transform.childCount; i++)
        {
            lightGuyToPopulate.Add(lightGuyValues.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>());
        }

        for (int i = 0; i < shadowGuyValues.transform.childCount; i++)
        {
            shadowGuyToPopulate.Add(shadowGuyValues.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>());
        }

        for (int i = 0; i < totalValues.transform.childCount; i++)
        {
            totalToPopulate.Add(totalValues.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(WaitForLoadAndPopulate());
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            GameManager.Instance.CurrentSave = 0;
        }
    }

    public IEnumerator WaitForLoadAndPopulate()
    {
        GameManager.Instance.LoadAllSaveFiles();
        GameManager.Instance.CurrentSave = 0;
        while (GameManager.Instance.Loading)
        {
            yield return null;
        }
        Populate();
    }

    public void Populate()
    {
        lightGuyToPopulate[0].text = GameManager.Instance.GetMetaInt("playerDeath1").ToString();
        lightGuyToPopulate[1].text = GameManager.Instance.GetMetaInt("jumpNumber1").ToString();
        lightGuyToPopulate[2].text = GameManager.Instance.GetMetaFloat("distance1").ToString();

        shadowGuyToPopulate[0].text = GameManager.Instance.GetMetaInt("playerDeath2").ToString();
        shadowGuyToPopulate[1].text = GameManager.Instance.GetMetaInt("jumpNumber2").ToString();
        shadowGuyToPopulate[2].text = GameManager.Instance.GetMetaFloat("distance2").ToString();

        totalToPopulate[0].text = GameManager.Instance.GetMetaFloat("totalTimePlayed").ToString();
    }
}