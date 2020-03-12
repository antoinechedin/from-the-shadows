using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismRotator : MonoBehaviour
{
    public float rotationStep;

    public int currentPos = 0;//-1 gauche      0 centre       1 droite
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Rotate(int sens)
    {
        if (sens == -1)//gauche
        {
            if (currentPos > -1)
            {
                Debug.Log("ça tourne à gauche");
                currentPos += sens;
                transform.Rotate(0, -rotationStep, 0);
            }
        }
        else if (sens == 1)//droite
        {
            if (currentPos < 1)
            {
                Debug.Log("ça tourne à gauche");
                currentPos += sens;
                transform.Rotate(0, rotationStep, 0);
            }
        }
    }
}
