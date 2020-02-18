using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour, IResetable
{
    public enum Type { Light, Shadow };
    public Type type;
    public Material lightMaterial;
    public Material shadowMaterial;

    private GameObject child;
    [HideInInspector]
    public bool isValidated;
    [HideInInspector]
    public bool isPickedUp;

    // Start is called before the first frame update
    void Awake()
    {
        child = transform.Find("Child").gameObject;
        if (type == Type.Light && child != null)
            child.GetComponent<MeshRenderer>().material = lightMaterial;
        else if (type == Type.Shadow && child != null)
            child.GetComponent<MeshRenderer>().material = shadowMaterial;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isValidated)
        {
            int idPlayer = collision.gameObject.GetComponent<PlayerController>().input.id;
            if (type == Type.Light && idPlayer == 1)
            {
                PickUp();
            }
            else if (type == Type.Shadow && idPlayer == 2)
            {
                PickUp();
            }
        }
    }

    private void PickUp()
    {
        isPickedUp = true;
        child.SetActive(false);
    }

    public void Reset()
    {
        if (!isValidated)
        {
            isPickedUp = false;
            child.SetActive(true);
        }
    }

    public void UpdateState()
    {
        if (isValidated && child != null)
        {
            child.SetActive(false);
        }
    }
}
