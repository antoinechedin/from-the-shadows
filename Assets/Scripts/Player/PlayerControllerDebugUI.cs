using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerDebugUI : MonoBehaviour
{
    public PlayerController player;
    public Text playerInputText;
    public Text playerStateText;
    public Text playerVelocityText;

    private void Update()
    {
        if (player != null)
        {
            playerInputText.text = "Player input: [" + player.input.x.ToString("0.00") + ", " + player.input.y.ToString("0.00") + "]";
            playerStateText.text = "Player state: " + player.state;
            playerVelocityText.text = "Player velocity: [" + player.velocity.x.ToString("0.00") + ", " + player.velocity.y.ToString("0.00") + "]";
        }
    }

}
