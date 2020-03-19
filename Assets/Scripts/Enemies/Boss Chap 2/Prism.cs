﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prism : Receptor
{
    public int requireNbLaserTouching;
    public Material notActivated;
    public Material activated;
    public List<GameObject> indicators;

    [Header("Attaque du boss")]
    public GameObject laserRotator;
    public float rotationSpeed;

    private bool firing = false;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        laserRotator.SetActive(false);
    }

    public override void AddLaser(GameObject addedlaser) 
    {
        base.AddLaser(addedlaser);
        TestNbLaserTouching();
    }

    public override void RemoveLaser(GameObject removedLaser)
    {
        base.RemoveLaser(removedLaser);
        TestNbLaserTouching();
    }


    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 100);
    }

    /// <summary>
    /// Test to see if the number of lasers touching the prism is equal to the required number. If it's true, fire a big laser.
    /// </summary>
    public void TestNbLaserTouching()
    {
        //reset de la couleur des faces
        Material[] activatedArray = new Material[] { activated, activated, activated, activated, activated, activated };
        Material[] notActivatedArray = new Material[] { notActivated, notActivated, notActivated, notActivated, notActivated, notActivated };
        foreach (GameObject gameObject in indicators)
        {
            gameObject.GetComponent<MeshRenderer>().materials = notActivatedArray;
        }

        for (int i = 0; i < lasersTouching.Count; i++)
        {
            indicators[i].GetComponent<MeshRenderer>().materials = activatedArray;
        }


        Debug.Log(gameObject.name + " : " + lasersTouching.Count);
        if (lasersTouching.Count >= requireNbLaserTouching)
        {
            //FIRE A BIG LASER
            if (!firing)
            {
                StartCoroutine(FireLaser());
            }
        }
    }

    public IEnumerator FireLaser()
    {
        firing = true;

        //On tir un rayon pour chercher la collision avec le boss
        Vector3 aimPoint = transform.position + transform.forward * 100;
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, aimPoint, out hit, 100, ~LayerMask.NameToLayer("BossLayer")))
        {
            hit.transform.GetComponent<Vampire>().TakeDamage();
        }

        //on tir le rayon
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, aimPoint);

        yield return new WaitForSeconds(2);

        //on enlève le rayon
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
        firing = false;

        //TODO : reset les reflector
    }

    public IEnumerator SpiningLasers(float time)
    {
        float cpt = 0;

        while (cpt < time)
        {
            cpt += Time.deltaTime;

            //rotation
            laserRotator.SetActive(true);
            laserRotator.transform.Rotate(new Vector3(0, 0, rotationSpeed));
            yield return null;
        }
        laserRotator.SetActive(false);
    }
}