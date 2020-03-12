using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismRotator : MonoBehaviour
{
    public float rotationStep;
    public float rotationSpeed;

    private int currentPos = 0;//-1 gauche      0 centre       1 droite
    [SerializeField]
    private float targetRotation = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation != Quaternion.Euler(0, targetRotation, 0))
        {
            Debug.Log("rotation");
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0, targetRotation, 0)), rotationSpeed);
        }
    }

    public void Rotate(int sens)
    {
        if (sens == -1)//gauche
        {
            if (currentPos > -1)
            {
                currentPos += sens;
                targetRotation -= rotationStep;
            }
        }
        else if (sens == 1)//droite
        {
            if (currentPos < 1)
            {
                currentPos += sens;
                targetRotation += rotationStep;
            }
        }
    }
}
