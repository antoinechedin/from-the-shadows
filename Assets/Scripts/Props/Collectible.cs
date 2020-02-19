using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour, IResetable
{
    public enum Type { Light, Shadow };
    public Type type;
    public Material lightMaterial;
    public Material shadowMaterial;

    private List<GameObject> childs;
    [HideInInspector]
    public bool isValidated;
    [HideInInspector]
    public bool isPickedUp;

    // Start is called before the first frame update
    void Awake()
    {
        int nbChildren = transform.childCount;
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer renderer in renderers)
        {
            if (type == Type.Light)
                renderer.material = lightMaterial;
            if (type == Type.Shadow)
                renderer.material = shadowMaterial;
        }            
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
        isPickedUp = true;
        SetVisible(false);
    }

    public void SetVisible(bool isVisible)
    {
        int nbChildren = transform.childCount;
        for (int i = 0; i < nbChildren; i++)
        {
            transform.GetChild(i).gameObject.SetActive(isVisible);
        }
    }

    public void Reset()
    {
        if (!isValidated)
        {
            isPickedUp = false;
            SetVisible(true);
        }
    }

    public void UpdateState()
    {
        if (isValidated)
            SetVisible(false);
    }
}
