using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MU_AIStates : MonoBehaviour
{
    public string state;
    public bool Bl_withinattackingdistance = false;
    public bool Bl_withinchasingdistance = false;
    public GameObject GO_Target;
    public float ChaseRange;
    public float AttackRange;
    public Vector3 CriticalDistanceOffset;
    public float StoppingDistance;
    public NavMeshAgent MyNavmeshAgent;
    public GameObject GO_Bullet;
    public GameObject GO_BulletSpawner;
    public bool Bl_canshoot;
    public float Fl_TimeDelayforFirstShot;
    public float Fl_TimeDelayBetweenShots;
    public GameObject ClosestRedEnemy;
    public GameObject ClosestBlueEnemy;
    public GameObject[] arrayofenemies;
    public List<GameObject> ListofRedEnemies=new List<GameObject>();
    public List<GameObject> ListofBlueEnemies = new List<GameObject>();

    // Use this for initialization
    void Start()
    {

        state = "idle";
        MyNavmeshAgent = GetComponent<NavMeshAgent>();
        InvokeRepeating("shoot", Fl_TimeDelayforFirstShot, Fl_TimeDelayBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        changestatesbasedondistance();
        ChangeTheStates();
        DifferentStateReactions();
        FindClosestEnemy();
        FindClosestBlueEnemy();
        FindClosestRedEnemy();
    }
    void changestatesbasedondistance()
    {
        if (Vector3.Distance(transform.position, GO_Target.transform.position) < AttackRange)
        {
            Bl_withinattackingdistance = true;
        }
    }
    void GetInStartingPositions()
    {

    }
    void ChangeTheStates()
    {
        if (Bl_withinattackingdistance)
        {
            state = "Attack";
        }
    }
    void DifferentStateReactions()
    {
        if (state == "Attack")
        {
            if (GetComponent<MU_ObjectProperties>().typeofenemy == "BlueEnemy")
            {
                GO_Target = ClosestRedEnemy;
                transform.LookAt(GO_Target.transform);
                MyNavmeshAgent.SetDestination(GO_Target.transform.position);
                if (Vector3.Distance(transform.position, GO_Target.transform.position) <= StoppingDistance)
                {
                    MyNavmeshAgent.SetDestination(transform.position);
                    state = "Shooting";
                }
            }
        }
        if (state == "Shooting")
        {
            Bl_canshoot = true;
        }
        // move towards an enemy set as the target
        // look at the enemy
        // stop moving if within a certain distance
        // shoot at them afterwards
    }
    void shoot()
    {
        if (Bl_canshoot)
        {
            GameObject.Instantiate(GO_Bullet, GO_BulletSpawner.transform.position, transform.rotation);
        }
    }
    void FindClosestZonetoEnemy()
    {

    }
    void FindClosestEnemy()
    {
        arrayofenemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in arrayofenemies)
        {
            if (go.GetComponent<MU_ObjectProperties>().typeofenemy == "RedEnemy")
            {
                if (!ListofRedEnemies.Contains(go))
                {
                    ListofRedEnemies.Add(go);
                }
            }
            else if (go.GetComponent<MU_ObjectProperties>().typeofenemy == "BlueEnemy")
            {
                print("kkkk");
                if (!ListofBlueEnemies.Contains(go))
                {
                    ListofBlueEnemies.Add(go); 
                }
            }
        }
    }
    void FindClosestRedEnemy()
    {
        float distance = Mathf.Infinity;
        foreach (GameObject go in ListofRedEnemies)
        {
            Vector3 v2 = go.transform.position - transform.position;
            float currentdistance = v2.sqrMagnitude;
            if (currentdistance < distance)
            {
                ClosestRedEnemy = go;
                distance = currentdistance;
            }
        }
    }
    void FindClosestBlueEnemy()
    {
        float distance = Mathf.Infinity;
        foreach (GameObject go in ListofBlueEnemies)
        {
            Vector3 v2 = go.transform.position - transform.position;
            float currentdistance = v2.sqrMagnitude;
            if (currentdistance < distance)
            {
                ClosestBlueEnemy = go;
                distance = currentdistance;
            }
        }
    }
}
