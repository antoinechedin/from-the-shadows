using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuMetaManager : MonoBehaviour
{
    public GameObject dataPrefab;
    public ContentSizeFitter shadowCharContent;
    public ContentSizeFitter lightCharContent;
    public ContentSizeFitter generalContent;

    void Start()
    {
        Populate();
    }

    public void Populate()
    {
        GameObject newData;

        // PLAYER 1 (Shadow character)

        newData = Instantiate(dataPrefab, shadowCharContent.transform);
        newData.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = "Morts";
        newData.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = GameManager.Instance.GetMetaInt("playerDeath1").ToString();

        newData = Instantiate(dataPrefab, shadowCharContent.transform);
        newData.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = "Sauts";
        newData.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = GameManager.Instance.GetMetaInt("jumpNumber1").ToString();

        newData = Instantiate(dataPrefab, shadowCharContent.transform);
        newData.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = "Distance";
        newData.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = System.Math.Round(GameManager.Instance.GetMetaFloat("distance1"), 2).ToString();

        // PLAYER 2 (Light character)

        newData = Instantiate(dataPrefab, lightCharContent.transform);
        newData.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = "Morts";
        newData.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = GameManager.Instance.GetMetaInt("playerDeath2").ToString();

        newData = Instantiate(dataPrefab, lightCharContent.transform);
        newData.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = "Sauts";
        newData.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = GameManager.Instance.GetMetaInt("jumpNumber2").ToString();

        newData = Instantiate(dataPrefab, lightCharContent.transform);
        newData.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = "Distance";
        newData.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = System.Math.Round(GameManager.Instance.GetMetaFloat("distance2"), 2).ToString();

        // GENERAL

        newData = Instantiate(dataPrefab, generalContent.transform);
        newData.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = "Temps joué";
        newData.transform.Find("Value").GetComponent<TextMeshProUGUI>().text = TimeSpan.FromSeconds(GameManager.Instance.GetMetaFloat("totalTimePlayed")).ToString(@"hh\:mm\:ss");
    }
}