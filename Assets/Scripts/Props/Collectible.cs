using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour, IResetable
{
    public enum Type { Light, Shadow };
    public Type type;
    [Range(0, 1)]
    public float animationOffset;
    public GameObject flash;
    public AudioClip pickUpSound;

    private AudioSource audioSource;
    private GameObject child;
    //[HideInInspector]
    public bool isValidated;
    //[HideInInspector]
    public bool isPickedUp;
    private bool isVisible;

    // Start is called before the first frame update

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Animator animator = GetComponent<Animator>();
        if (animator != null)
            animator.Play("Default", 0, animationOffset);
        child = transform.Find("Sphere").gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isValidated)
        {
            int idPlayer = collision.gameObject.GetComponent<PlayerInput>().id;
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
        if (child != null && !isPickedUp && !isValidated)
        {
            isPickedUp = true;
            if (audioSource != null)
                audioSource.PlayOneShot(pickUpSound);
            Instantiate(flash, child.transform.position, child.transform.rotation);
            isVisible = false;
            Invoke("UpdateVisible", 0.3f);
        }
    }

    public void UpdateVisible()
    {
        if (child != null)
            child.SetActive(isVisible);
    }

    public void Reset()
    {
        if (!isValidated)
        {
            isPickedUp = false;
            isVisible = true;
            UpdateVisible();
        }
    }

    public void UpdateState()
    {
        if (isValidated)
        {
            isVisible = false;
            UpdateVisible();
        }
    }
}
