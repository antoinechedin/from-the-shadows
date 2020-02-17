using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUI : MonoBehaviour
{
    public Transform player;

    Image followImage;
    public Sprite displayedSprite;

    public Vector3 offSet;

    public bool drawDebugPosition;

    private void Start()
    {
        followImage = gameObject.AddComponent<Image>();
        followImage.sprite = displayedSprite;
        followImage.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    private void Update()
    {
        followImage.rectTransform.position = Camera.main.WorldToScreenPoint(player.position + offSet);
    }

    private void OnDrawGizmos()
    {
        if(drawDebugPosition)
            Gizmos.DrawWireSphere(player.position + offSet, 1);
    }
}
