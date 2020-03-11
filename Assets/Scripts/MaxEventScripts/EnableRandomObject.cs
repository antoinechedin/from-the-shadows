using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableRandomObject : MonoBehaviour
{
    public List<GameObject> gameObjectList = new List<GameObject>();

    public float cooldown = 5f;
    public float overlapTime = 2f;

    private int lastIndex = 0;
    private int randomIndex = 0;

    private GameObject newActivated;
    private GameObject oldActivated;

    private void OnEnable()
    {
        if (newActivated != null)
            newActivated.SetActive(false);
        StartCoroutine(RandomObjectLoop());
    }

    private IEnumerator RandomObjectLoop()
    {
        while (randomIndex == lastIndex)
            randomIndex = Random.Range(0, (gameObjectList.Count - 1));
        lastIndex = randomIndex;


        newActivated = gameObjectList[randomIndex];
        newActivated.SetActive(true);

        yield return new WaitForSeconds(overlapTime);

        if (oldActivated != null)
            oldActivated.SetActive(false);

        oldActivated = newActivated;

        yield return new WaitForSeconds(cooldown - overlapTime);

        StartCoroutine(RandomObjectLoop());
    }
}
