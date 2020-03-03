using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private HandAttack handAttack;

    // Start is called before the first frame update
    void Start()
    {
        handAttack = transform.parent.gameObject.GetComponent<HandAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "HurtBoss")
        {
            handAttack.GetHurt();
            //handAttack.to = transform.position;
        }
    }
}
