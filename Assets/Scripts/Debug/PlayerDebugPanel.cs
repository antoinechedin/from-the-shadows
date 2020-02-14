using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDebugPanel : MonoBehaviour
{
    private TextMeshProUGUI playerInfos;
    private string playerInfosTemplate;

    private NewPlayerController player;

    private void Awake()
    {
        // playerInfos = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        playerInfos = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (playerInfos != null) playerInfosTemplate = playerInfos.text;
        

        player = transform.parent.parent.GetComponent<NewPlayerController>();
    }

    private void Update()
    {
        if (player != null)
        {
            if (playerInfos != null)
            {
                playerInfos.text = string.Format(
                    playerInfosTemplate,
                    player.velocity.x,
                    player.velocity.y,
                    player.targetVelocity.x,
                    player.targetVelocity.y,
                    player.controller.collisions.bellow ? "X" : "_",
                    player.state
                );
            }
        }
    }
}
