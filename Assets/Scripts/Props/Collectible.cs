using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour, IResetable
{
    public enum Type { Light, Shadow };
    public Type type;
    public int index;

    private GameObject child;
    //[HideInInspector]
    public bool isValidated;
    [HideInInspector]
    public bool isPickedUp;

    // Start is called before the first frame update
    void Start()
    {
        child = transform.Find("Child").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Debug.Log("plop");
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
