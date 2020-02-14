using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

#if UNITY_EDITOR

public class ReadOnlyAttribute : PropertyAttribute
{

}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property,
                                            GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position,
                               SerializedProperty property,
                               GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}

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

    [ReadOnly] public Camera mainCam;
    [ReadOnly] public int countLevel;
    [ReadOnly] public int countCameraPoint;
    [ReadOnly] public int countLevelTrigger;
    [ReadOnly] public int countInvisibleWall;

    List<LevelManager> levels = new List<LevelManager>();
    List<Identifier<BoxCollider2D>> levelTrigger = new List<Identifier<BoxCollider2D>>();
    List<Identifier<BoxCollider2D>> invisibleWall = new List<Identifier<BoxCollider2D>>();

    /// <summary>
    /// Fill the list of Transform for CameraPoints / LevelTriggers / Invisible Walls
    /// </summary>
    public void LevelScriptInit()
    {
        // Clearing the Lists
        levelTrigger.Clear();
        invisibleWall.Clear();
        levels.Clear();

        // Get Reference to Camera
        mainCam = Camera.main;

        // Level Count
        GameObject[] levelsGo = GameObject.FindGameObjectsWithTag("Level");
        foreach (GameObject go in levelsGo)
        {
            levels.Add(go.GetComponent<LevelManager>());
        }

        // Get Level Triggers + Number of the Level Associated
        GameObject[] levelTriggers = GameObject.FindGameObjectsWithTag("Triggers");
        foreach (GameObject go in levelTriggers)
        {
            BoxCollider2D[] lvlTrigger = go.transform.GetComponentsInChildren<BoxCollider2D>();
            foreach (BoxCollider2D trigger in lvlTrigger)
                levelTrigger.Add(new Identifier<BoxCollider2D>(trigger, GetIdOf(go.transform.parent.name)));
        }

        // Get Invisible Walls + Number of the Level Associated
        GameObject[] invisibleWalls = GameObject.FindGameObjectsWithTag("InvisibleWalls");
        foreach (GameObject go in invisibleWalls)
        {
            BoxCollider2D[] invWalls = go.transform.GetComponentsInChildren<BoxCollider2D>();
            foreach (BoxCollider2D wall in invWalls)
                invisibleWall.Add(new Identifier<BoxCollider2D>(wall, GetIdOf(go.transform.parent.name)));
        }

        // Get Some Infos about the Datas collected (Count)
        countLevel = levels.Count;
        countCameraPoint = countLevel;
        countLevelTrigger = levelTrigger.Count;
        countInvisibleWall = invisibleWall.Count;
    }

    // Set the Main Camera to the Camera Point LB
    public void GoToCameraPointLB(int id)
    {
        LevelManager lm = levels[id].GetComponent<LevelManager>();
        mainCam.transform.position = lm.cameraLimitLB.position;
    }

    // Set the Main Camera to the Camera Point RT
    public void GoToCameraPointRT(int id)
    {
        LevelManager lm = levels[id].GetComponent<LevelManager>();
        mainCam.transform.position = lm.cameraLimitRT.position;
    }

    // Set the Camera Point from the Main Camera Position LB
    public void SetCameraPointLB(int id)
    {
        LevelManager lm = levels[id].GetComponent<LevelManager>();
        lm.cameraLimitLB.position = mainCam.transform.position;
    }

    // Set the Camera Point from the Main Camera Position RT
    public void SetCameraPointRT(int id)
    {
        LevelManager lm = levels[id].GetComponent<LevelManager>();
        lm.cameraLimitRT.position = mainCam.transform.position;
    }

    // Get Id of String, Need to Improve for 9+
    int GetIdOf(string s)
    {
        return int.Parse(s.Substring(s.Length - 1));
    }

    /// <summary>
    /// Display Custom Icons for CameraPoints & Colors for LevelTriggers / InvisibleWalls
    /// </summary>
    
    void OnDrawGizmos()
    {
        // CameraPoints Custom Icons + Number
        int i = 0;
        foreach(LevelManager lm in levels)
        {
            i++;
            GUI.color = Color.white;
            Vector3 lb = lm.cameraLimitLB.position;
            Vector3 rt = lm.cameraLimitRT.position;
            Vector3 mean = (lb + rt) / 2;
            Gizmos.DrawWireCube(mean, rt-lb);
            Handles.Label(mean + 0.6f*Vector3.right, i.ToString());
            Gizmos.DrawIcon(mean, "Camera2.png", true);
        }

        // LevelTrigger Red Collider + Number
        foreach (Identifier<BoxCollider2D> box in levelTrigger)
        {
            Gizmos.color = Color.red;
            BoxCollider2D boxCollider = box.item;

            GUI.color = Color.red;
            Handles.Label(boxCollider.bounds.center, box.id.ToString());

            Gizmos.DrawWireCube(boxCollider.bounds.center,
                                boxCollider.bounds.extents*2);
        }

        // InvisibleWalls Blue Collider + Number
        foreach (Identifier<BoxCollider2D> box in invisibleWall)
        {
            Gizmos.color = Color.blue;
            BoxCollider2D boxCollider = box.item;

            GUI.color = Color.blue;
            Handles.Label(boxCollider.bounds.center, box.id.ToString());        

            Gizmos.DrawWireCube(boxCollider.bounds.center,
                                boxCollider.bounds.extents * 2);
        }
    }   
}
#endif
