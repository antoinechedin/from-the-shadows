using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerDebugUI : MonoBehaviour
{
    public PlayerController player;
    public Text playerStateText;
    public Text playerInputText;
    public Text playerVelocityText;

    private void Update()
    {
        if (player != null)
        {
            playerStateText.text = "Player state: " + player.state;
            playerInputText.text = "Player input: [" + player.targetVelocity.x.ToString("0.00") + ", " + player.targetVelocity.y.ToString("0.00") + "]";
            playerVelocityText.text = "Player velocity: [" + player.velocity.x.ToString("0.00") + ", " + player.velocity.y.ToString("0.00") + "]";
        }
    }

}
