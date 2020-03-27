using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class OffscreenIndicator : MonoBehaviour
{
	public Texture2D icon;
	public float iconSize = 20f;
	[HideInInspector]
	public GUIStyle gui;	

	// Adapt icon size to screen size
	private float resolution = Screen.width / 500; 
	private Camera camera;
	private Vector2 indRange;
	private bool isVisible = true;

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
			Vector3 direction = transform.position - camera.transform.position;
			direction = Vector3.Normalize(direction);
			direction.y *= -1f; 

			Vector2 indPos = new Vector2(indRange.x * direction.x, indRange.y * direction.y);
			indPos = new Vector2((Screen.width / 2) + indPos.x, (Screen.height / 2) + indPos.y);		

			Vector3 pointScreen = transform.position - camera.ScreenToWorldPoint(new Vector3(indPos.x, indPos.y,
																					transform.position.z));
			pointScreen = Vector3.Normalize(pointScreen);

			float angle = Mathf.Atan2(pointScreen.x, pointScreen.y) * Mathf.Rad2Deg;

			GUIUtility.RotateAroundPivot(angle, indPos);
			GUI.Box(new Rect(indPos.x, indPos.y, resolution * iconSize, resolution * iconSize), icon, gui);			
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