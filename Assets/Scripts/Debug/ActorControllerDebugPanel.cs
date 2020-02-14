using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActorControllerDebugPanel : MonoBehaviour
{
    private TextMeshProUGUI controllerInfos;
    private string controllerInfosTemplate;
    private TextMeshProUGUI collisionInfos;
    private string collisionInfosTemplate;

    private NewActorController actorController;

    private void Awake()
    {
        // playerInfos = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        controllerInfos = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (controllerInfos != null) controllerInfosTemplate = controllerInfos.text;
        collisionInfos = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        if (collisionInfos != null) collisionInfosTemplate = collisionInfos.text;

        actorController = transform.parent.parent.GetComponent<NewActorController>();
    }

    private void Update()
    {
        if (actorController != null)
        {
            if (controllerInfos != null)
            {
                controllerInfos.text = string.Format(
                    controllerInfosTemplate,
                    actorController.collisions.move.x,
                    actorController.collisions.move.y,
                    actorController.collisionsPrevious.move.x,
                    actorController.collisionsPrevious.move.y,
                    actorController.collisions.groundNormal.x,
                    actorController.collisions.groundNormal.y,
                    actorController.collisions.climbingSlope ? "X" : " ",
                    actorController.collisions.descendingSlope ? "X" : " ",
                    actorController.collisions.slidingSlope ? "X" : " ",
                    actorController.collisions.slopeAngle,
                    actorController.collisionsPrevious.slopeAngle
                );
            }
            if (collisionInfos != null)
            {
                collisionInfos.text = string.Format(
                    collisionInfosTemplate,
                    actorController.collisions.above ? "X" : "_",
                    actorController.collisions.bellow ? "X" : "_",
                    actorController.collisions.left ? "X" : "_",
                    actorController.collisions.right ? "X" : "_",
                    actorController.collisionsPrevious.above ? "X" : "_",
                    actorController.collisionsPrevious.bellow ? "X" : "_",
                    actorController.collisionsPrevious.left ? "X" : "_",
                    actorController.collisionsPrevious.right ? "X" : "_"
                );
            }
        }
    }
}
