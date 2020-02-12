using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActorControllerDebugCanvas : MonoBehaviour
{
    // private TextMeshProUGUI playerInfos;
    private Transform panel;
    private TextMeshProUGUI controllerInfos;
    private string controllerInfosTemplate;
    private TextMeshProUGUI collisionInfos;
    private string collisionInfosTemplate;

    private NewActorController actorController;

    private void Awake()
    {
        // playerInfos = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        panel = transform.GetChild(0);
        controllerInfos = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        if (controllerInfos != null) controllerInfosTemplate = controllerInfos.text;
        collisionInfos = transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
        if (collisionInfos != null) collisionInfosTemplate = collisionInfos.text;

        actorController = transform.parent.GetComponent<NewActorController>();
    }

    private void Update()
    {
        if (actorController != null && panel != null)
        {
            if (controllerInfos != null)
            {
                controllerInfos.text = string.Format(
                    controllerInfosTemplate,
                    actorController.collisions.move.x,
                    actorController.collisions.move.y,
                    actorController.collisions.moveOld.x,
                    actorController.collisions.moveOld.y,
                    actorController.collisions.climbingSlope ? "X" : " ",
                    actorController.collisions.descendingSlope ? "X" : " ",
                    actorController.collisions.slidingSlope ? "X" : " ",
                    actorController.collisions.slopeAngle,
                    actorController.collisions.slopeAngleOld
                );
            }
            if (collisionInfos != null)
            {
                collisionInfos.text = string.Format(
                    collisionInfosTemplate,
                    actorController.collisions.above ? "X" : "_",
                    actorController.collisions.bellow ? "X" : "_",
                    actorController.collisions.left ? "X" : "_",
                    actorController.collisions.right ? "X" : "_"
                );
            }
        }
    }
}
