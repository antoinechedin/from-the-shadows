using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepLane : MonoBehaviour
{
    public Transform left;
    public Transform right;
    public GameObject prefab;
    public float speed;

    private GameObject go;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Spawn");
    }

    // Update is called once per frame
    void Update()
    {
        if (go != null)
            go.transform.position = Vector3.MoveTowards(go.transform.position, right.position, Time.deltaTime * speed);
        if (go != null && go.transform.position == right.position)
        {
            go = null;
            Destroy(go);
        }
    }

    public IEnumerator Spawn()
    {
        for(int i = 0; i < 5; i++)
        {
            if (left != null && right != null)
                go = Instantiate(prefab, left.position, Quaternion.identity);
            yield return new WaitForSeconds(5);
        }
        yield return null;
    }
}
