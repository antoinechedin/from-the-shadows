///<summary>
/// Change the switch state
///</summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField]
    GameObject switchOn; // Prefab of the switch on On

    [SerializeField]
    GameObject switchOff; // Prefab of the switch on Off

    public bool isOn = false;

    void Start()
    {
        // Switch the Lever to its Off sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = switchOff.GetComponent<SpriteRenderer>().sprite;
    }

    // Test the collision of the lever with the player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Switch the Lever to its On sprite using the E key when triggered
            //if (Input.GetButton("Interact"))
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = switchOn.GetComponent<SpriteRenderer>().sprite;

                isOn = true;
            }
        }
        
    }
}
