using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDebugPanel : MonoBehaviour
{
    private TextMeshProUGUI playerInfos;
    private string playerInfosTemplate;

    [HideInInspector] public PlayerController player;
    public int playerId;

    private void Awake()
    {
        // playerInfos = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        playerInfos = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (playerInfos != null) playerInfosTemplate = playerInfos.text;
        
        player = transform.parent.parent.GetComponent<PlayerController>();
    }

    private void OnEnable() {
        PlayerInput[] players = GameObject.FindObjectsOfType<PlayerInput>();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].id == playerId)
            {
                player = players[i].GetComponent<PlayerController>();
                break;
            }
        }
    }

    private void OnDisable() {
        player = null;
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
                    player.actor.collisions.bellow ? "X" : " ",
                    player.xVelocitySign < 0 ? "<-" :  player.xVelocitySign > 0 ? "->": "00",
                    player.state
                );
            }
        }
        else
        {
            playerInfos.text = "No player " + playerId;
        }
    }
}
