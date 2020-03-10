using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructiblePlatform : MonoBehaviour, IResetable
{
    public void Destruct()
    {
        FindObjectOfType<ChapterManager>().ShakeFor(1f, 1f, 1f);
        gameObject.SetActive(false);

        for (int i = 0; i < 5; i++)
        {
            float rdmX = Random.Range(GetComponent<Collider2D>().bounds.min.x, GetComponent<Collider2D>().bounds.max.x);
            float rdmY = Random.Range(GetComponent<Collider2D>().bounds.min.y, GetComponent<Collider2D>().bounds.max.y);
            Vector3 rdmPos = new Vector3(rdmX, rdmY, gameObject.transform.position.z);
            Instantiate(Resources.Load("DestroyPlatform"), rdmPos, Quaternion.identity);
        }
    }

    public void Reset()
    {
        gameObject.SetActive(true);
    }
}