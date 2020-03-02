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

    [ReadOnly] public int countLevel;
    [ReadOnly] public int countLevelTrigger;
    [ReadOnly] public int countInvisibleWall;

    List<LevelManager> levels = new List<LevelManager>();
    List<Identifier<BoxCollider2D>> levelTrigger = new List<Identifier<BoxCollider2D>>();
    List<Identifier<BoxCollider2D>> invisibleWall = new List<Identifier<BoxCollider2D>>();

    static int SortByName(LevelManager lm1, LevelManager lm2)
    {
        return lm1.id.CompareTo(lm2.id);
    }
    /// <summary>
    /// Fill the list of Transform for LevelTriggers / Invisible Walls
    /// </summary>
    public void LevelScriptInit()
    {
        // Clearing the Lists
        levelTrigger.Clear();
        invisibleWall.Clear();
        levels.Clear();

        // Level Count
        GameObject[] levelsGo = GameObject.FindGameObjectsWithTag("CU Level");
        foreach (GameObject go in levelsGo)
        {
            levels.Add(go.GetComponent<LevelManager>());
        }
        levels.Sort(SortByName);

        // Get Level Triggers + Number of the Level Associated
        GameObject[] levelTriggers = GameObject.FindGameObjectsWithTag("CU Triggers");
        foreach (GameObject go in levelTriggers)
        {
            BoxCollider2D[] lvlTrigger = go.transform.GetComponentsInChildren<BoxCollider2D>();
            foreach (BoxCollider2D trigger in lvlTrigger)
                levelTrigger.Add(new Identifier<BoxCollider2D>(trigger, go.transform.parent.GetComponent<LevelManager>().id));
        }

        // Get Invisible Walls + Number of the Level Associated
        GameObject[] invisibleWalls = GameObject.FindGameObjectsWithTag("CU InvisibleWalls");
        foreach (GameObject go in invisibleWalls)
        {
            BoxCollider2D[] invWalls = go.transform.GetComponentsInChildren<BoxCollider2D>();
            foreach (BoxCollider2D wall in invWalls)
                invisibleWall.Add(new Identifier<BoxCollider2D>(wall, go.transform.parent.GetComponent<LevelManager>().id));
        }

        // Get Some Infos about the Datas collected (Count)
        countLevel = levels.Count;
        countLevelTrigger = levelTrigger.Count;
        countInvisibleWall = invisibleWall.Count;
    }

    /// <summary>
    /// Display Custom Icons for Colors for LevelTriggers / InvisibleWalls
    /// </summary>
    
    void OnDrawGizmos()
    {
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
