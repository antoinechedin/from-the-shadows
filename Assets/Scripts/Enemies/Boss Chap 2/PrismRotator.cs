using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismRotator : MonoBehaviour
{
    public float rotationStep;
    public float rotationSpeed;
    public AimCursor aimCursor;

    private int currentPos = 1;//0 gauche      1 centre       2 droite
    private float targetRotation = 0;

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation != Quaternion.Euler(0, targetRotation, 0))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, targetRotation, 0)), rotationSpeed);
        }
    }

    public void Rotate(int sens)
    {
        if (sens == -1)//gauche
        {
            if (currentPos > 0)
            {
                currentPos += sens;
                targetRotation -= rotationStep;
            }
        }
        else if (sens == 1)//droite
        {
            if (currentPos < 2)
            {
                currentPos += sens;
                targetRotation += rotationStep;
            }
        }

        aimCursor.SetTargetPos(currentPos);
    }
}
