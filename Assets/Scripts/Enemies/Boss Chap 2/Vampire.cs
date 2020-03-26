using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : MonoBehaviour, IResetable
{
    [Header("Déplacements")]
    public List<Transform> positions;
    public float speed;

    [Header("Attaque du boss")]
    public GameObject laserRotator;
    public float rotationSpeed;

    [Header("Actions")]
    public float minTimeAction;
    public float maxTimeAction;
    public GameObject explosivePrefab;

    [Header("Projectile explosif")]
    public float speedProjectile;
    public int minNbSubProjectile;
    public int maxNbSubProjectile;

    [Header("Sous projectiles")]
    public float speedSubProjectile;
    public float lengthSubProjectile;

    [Header("Stats")]
    public int maxLife;

    private float cptAction;
    private float rdmAction;

    private int life;

    private Animator animator;

    private bool firingLaser = false;
    private bool reset = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rdmAction = Random.Range(minTimeAction, maxTimeAction);
        life = maxLife;
    }

    // Update is called once per frame
    void Update()
    {
        if (!firingLaser)
        {
            cptAction += Time.deltaTime;

            if (cptAction >= rdmAction)
            {
                PickRandomAction();
            }
        }
    }
    public void PickRandomAction()
    {
        int rdm = Random.Range(0, 2); //rdm entre 0 et 1 compris

        switch (rdm)
        {
            case 0:
                Attack();
                break;
            case 1:
                Move();
                break;
            default:
                break;
        }

        cptAction = 0;
        rdmAction = Random.Range(minTimeAction, maxTimeAction);
    }

    public void Attack()
    {
        Debug.Log("Attack");
        if (life == 3)//phase 1 : fantôme
        {
            LaunchExplosive();
        }
        else if (life==2)//phase 2 fantôme + 1 explosif laser (joueur aléatoire)
        {
            LaunchExplosive();
        }
        else if (life == 1)//phase 3 + 2 explosif laser (chaque joueur)
        {
            LaunchExplosive();
        }
    }

    public void LaunchExplosive()
    {
        //pick a random target
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        GameObject targetPlayer = players[Random.Range(0, players.Length)].gameObject;
        Debug.Log(targetPlayer.name +" targeted");

        //launch attack
        GameObject spawned = Instantiate(explosivePrefab, transform.position, Quaternion.identity);
        spawned.GetComponent<VampireExplosive>().SetInfos(targetPlayer.transform.position + new Vector3(0, 2, 0),
            speedProjectile,
            minNbSubProjectile,
            maxNbSubProjectile, 
            speedSubProjectile,
            lengthSubProjectile);
    }

    public void Move()
    {
        Vector3 chosenPos = new Vector3();
        do
        {
            chosenPos = positions[Random.Range(0, positions.Count - 1)].position;
        } while (chosenPos == transform.position);

        StartCoroutine(MoveAsync(chosenPos));
    }

    public IEnumerator MoveAsync(Vector3 pos)
    {
        while (transform.position != pos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos, speed);
            yield return null;
        }
    }

    public IEnumerator SpiningLasers(float time)
    {
        firingLaser = true;
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        //on active le gros laser
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, FindObjectOfType<Prism>().transform.position);
        lineRenderer.enabled = true;

        float cpt = 0; 
        laserRotator.GetComponent<Animator>().SetTrigger("Activating");
        while (cpt < time && !reset)
        {
            cpt += Time.deltaTime;

            //rotation
            laserRotator.transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
            yield return null;
        }

        laserRotator.GetComponent<Animator>().SetTrigger("Disactivating");
        lineRenderer.enabled = false;
        firingLaser = false;
        reset = false;

        ShuffleReflectors();
    }

    /// <summary>
    /// Permet de mélanger les positions des reflectors de façon à ne pas pouvoir spam l'attaque sur le boss
    /// </summary>
    public void ShuffleReflectors()
    {
        Reflector[] reflectors = FindObjectsOfType<Reflector>();
        foreach (Reflector reflector in reflectors)
        {
            reflector.Shuffle();
        }
    }

    public void TakeDamage()
    {
        animator.SetTrigger("TakeDamage");
        life--;
        Debug.Log("Take damage : " + life);

        if (life <= 0)
        {
            Die();
        }
    }

    public void BigAttack()
    {
        StartCoroutine(SpiningLasers(5));
    }

    public void Die()
    {
        Debug.Log("DIE");
    }

    public void Reset()
    {
        reset = true;

        life = maxLife;
        transform.position = positions[1].position;
        animator.SetTrigger("Reset");
        firingLaser = false;
        cptAction  = 0;

        //on détruit tous les projectiles
        VampireExplosive[] explosives = FindObjectsOfType<VampireExplosive>();
        foreach (VampireExplosive explo in explosives)
        {
            Destroy(explo.gameObject);
        }

        LaserProjectile[] lasers = FindObjectsOfType<LaserProjectile>();
        foreach (LaserProjectile las in lasers)
        {
            Destroy(las.gameObject);
        }
    }
}
