using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDestruct : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ceiling (1)" || collision.gameObject.name == "Ceiling (2)" || collision.gameObject.name == "Ceiling (3)")
        {
            collision.gameObject.GetComponent<DestructiblePlatform>().Destruct();
        }
    }
}
