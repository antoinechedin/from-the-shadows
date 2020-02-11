using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class ChapterUtility : MonoBehaviour
{
    /// <summary>
    /// Stock something with an identifier (ex Camera Point = Transform and Level 1)
    /// </summary>
    /// <typeparam name="T">The object you need to identify</typeparam>
    struct Identifier<T>
    {
        public T item;
        public int id;
        public Identifier(T item2, int id2)
        {
            item = item2;
            id = id2;
        }
    }

    [SerializeField] Camera mainCam;
    [SerializeField] private int countLevel;
    [SerializeField] public int countCameraPoint;
    [SerializeField] private int countLevelTrigger;
    [SerializeField] private int countInvisibleWall;

    List<GameObject> cameraPoint = new List<GameObject>();
    List<Identifier<BoxCollider2D>> levelTrigger = new List<Identifier<BoxCollider2D>>();
    List<Identifier<BoxCollider2D>> invisibleWall = new List<Identifier<BoxCollider2D>>();

    /// <summary>
    /// Fill the list of Transform for CameraPoints / LevelTriggers / Invisible Walls
    /// </summary>
    public void LevelScriptInit()
    {
        // Clearing the Lists
        cameraPoint.Clear();
        levelTrigger.Clear();
        invisibleWall.Clear();

        // Get Reference to Camera
        mainCam = Camera.main;
        // Level Count
        GameObject[] levels = GameObject.FindGameObjectsWithTag("Level");

        // Get Camera Points and Order them
        GameObject[] cameraPoints = GameObject.FindGameObjectsWithTag("CameraPoint");
        for(int i = 1; i <= levels.Length; i++)
        {
            foreach(GameObject go in cameraPoints)
            {
                if (GetIdOf(go.name) == i)
                {
                    cameraPoint.Add(go);
                    break;
                }
            }
        }

        // Get Level Triggers + Number of the Level Associated
        GameObject[] levelTriggers = GameObject.FindGameObjectsWithTag("Triggers");
        foreach (GameObject go in levelTriggers)
        {
            BoxCollider2D[] lvlTrigger = go.transform.GetComponentsInChildren<BoxCollider2D>();
            foreach (BoxCollider2D trigger in lvlTrigger)
                levelTrigger.Add(new Identifier<BoxCollider2D>(trigger, GetIdOf(go.name)));
        }

        // Get Invisible Walls + Number of the Level Associated
        GameObject[] invisibleWalls = GameObject.FindGameObjectsWithTag("InvisibleWalls");
        foreach (GameObject go in invisibleWalls)
        {
            BoxCollider2D[] invWalls = go.transform.GetComponentsInChildren<BoxCollider2D>();
            foreach (BoxCollider2D wall in invWalls)
                invisibleWall.Add(new Identifier<BoxCollider2D>(wall, GetIdOf(go.name)));
        }

        // Get Some Infos about the Datas collected (Count)
        countLevel = levels.Length;
        countCameraPoint = cameraPoint.Count;
        countLevelTrigger = levelTrigger.Count;
        countInvisibleWall = invisibleWall.Count;
    }

    // Set the Main Camera to the Camera Point
    public void GoToCameraPoint(int id)
    {
        mainCam.transform.position = cameraPoint[id-1].transform.position;
    }

    // Set the Camera Point from the Main Camera Position
    public void SetCameraPoint(int id)
    {
        cameraPoint[id-1].transform.position = mainCam.transform.position;
    }

    // Get Id of String, Need to Improve for 9+
    int GetIdOf(string s)
    {
        return Int16.Parse(s.Substring(s.Length - 1));
    }

    /// <summary>
    /// Display Custom Icons for CameraPoints & Colors for LevelTriggers / InvisibleWalls
    /// </summary>
    /*void OnDrawGizmos()
    {
        // CameraPoints Custom Icons + Number
        int i = 0;
        foreach(GameObject go in cameraPoint)
        {
            i++;
            GUI.color = Color.black;
            Handles.Label((Vector2)go.transform.position + 0.6f*Vector2.right, i.ToString());
            Gizmos.DrawIcon((Vector2)go.transform.position, "Camera.png", true);
        }

        // LevelTrigger Red Collider + Number
        foreach (Identifier<BoxCollider2D> box in levelTrigger)
        {
            Gizmos.color = Color.red;
            BoxCollider2D boxCollider = box.item;

            GUI.color = Color.red;
            Handles.Label(boxCollider.bounds.center, box.id.ToString());

            Gizmos.DrawWireCube((Vector2)boxCollider.bounds.center,
                                (Vector2)boxCollider.bounds.extents*2);
        }

        // InvisibleWalls Blue Collider + Number
        foreach (Identifier<BoxCollider2D> box in invisibleWall)
        {
            Gizmos.color = Color.blue;
            BoxCollider2D boxCollider = box.item;

            GUI.color = Color.blue;
            Handles.Label(boxCollider.bounds.center, box.id.ToString());        

            Gizmos.DrawWireCube((Vector2)boxCollider.bounds.center,
                                (Vector2)boxCollider.bounds.extents * 2);
        }
    }*/
}
