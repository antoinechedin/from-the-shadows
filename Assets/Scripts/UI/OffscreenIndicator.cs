using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class OffscreenIndicator : MonoBehaviour
{
	public Texture2D playerIcon;
	public Texture2D arrowIcon;
	public float iconSize = 20f;
	[HideInInspector]
	public GUIStyle gui;

	// Adapt icon size to screen size
	private float resolution = Screen.width / 500;
	private Camera camera;
	private Vector2 indRange;
	private bool isVisible = true;
	private float offsetPlayerHeight= 1.5f;

	void Start()
	{
		isVisible = GetComponent<SpriteRenderer>().isVisible;
		camera = Camera.main;

		indRange.x = Screen.width;
		indRange.y = Screen.height;
		indRange *= 1.1f;
	}

	void OnGUI()
	{
		if (!isVisible)
		{
			Vector3 playerPosition = transform.position;
			playerPosition.y += offsetPlayerHeight;

			Vector2 playerIconPosition;
			Vector2 arrowIconPosition;

			playerIconPosition.x = Mathf.Min(camera.WorldToScreenPoint(playerPosition).x, Screen.width - iconSize * resolution * 2);
			playerIconPosition.x = Mathf.Max(playerIconPosition.x, iconSize * resolution);
			playerIconPosition.y = Mathf.Min(Screen.height - camera.WorldToScreenPoint(playerPosition).y, Screen.height - iconSize * resolution * 2);
			playerIconPosition.y = Mathf.Max(playerIconPosition.y, iconSize * resolution);

			GUI.Box(new Rect(playerIconPosition.x, playerIconPosition.y, resolution * iconSize, resolution * iconSize),
				    playerIcon, gui);

			Vector2 iconToPlayer = playerPosition - camera.ScreenToWorldPoint(playerIconPosition);
			float angle = - Vector2.SignedAngle(Vector2.down, iconToPlayer);

			float offset = (iconSize * resolution / 2);

			Vector2 pivot = new Vector2(playerIconPosition.x + (iconSize * resolution / 2), playerIconPosition.y + (iconSize * resolution / 2));

			GUIUtility.RotateAroundPivot(angle + 45, pivot);
			GUI.Box(new Rect(pivot.x, pivot.y, resolution * iconSize, resolution * iconSize),
					arrowIcon, gui);
		}
	}

	void OnBecameInvisible()
	{
		isVisible = false;
	}

	void OnBecameVisible()
	{
		isVisible = true;
	}
}