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
    public GameObject targettile;
    public GameObject GO_EnemyHomeBase;
    public GameObject tileclosesttoenemy;
    public GameObject[] Arrayoftiles;
    public List<GameObject> ListofRedEnemies = new List<GameObject>();
    public List<GameObject> ListofBlueEnemies = new List<GameObject>();
    
    public float Fl_MinDistaanceFromTile;


    // Use this for initialization
    void Start()
    {
        if (GO_Target == null)
        {
            GO_Target = ClosestBlueEnemy;
        }
        state = "idle";
        //take this out//
        Arrayoftiles = GameObject.FindGameObjectsWithTag("Tile");
        //unneeded/////
        MyNavmeshAgent = GetComponent<NavMeshAgent>();
        InvokeRepeating("shoot", Fl_TimeDelayforFirstShot, Fl_TimeDelayBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        if(state!="Attack")
        {
            Bl_withinattackingdistance = false;
        }
        FindClosestEnemy();
        FindClosestBlueEnemy();
        FindClosestRedEnemy();
        //closest red enemy is defined
        //closest blue is defined
        //check my distance from closest blue if im red and closest red if im blue
        //if this distance is less than attack range change state to attack
        //
        ////////////////this uses old algorithm, only works if initial target is assigned//////
        //changestatesbasedondistance();
        ////////////////////////////
        checkwhatteamimonandsetstates();
        ChangeTheStates();
        DifferentStateReactions();
    }
    void checkwhatteamimonandsetstates()
    {

        if (GetComponent<MU_ObjectProperties>().typeofenemy == "BlueEnemy")// if youre a blue enemy
        {
            if(state=="Attack")
            {
                if (Vector3.Distance(transform.position, ClosestRedEnemy.transform.position) > AttackRange && GetComponent<MU_ObjectProperties>().enemiesassembled == true)
                {
                    print("im coming home bbbbb");
                    state = "movingtohomebase";
                }
            }
            if (Vector3.Distance(transform.position, ClosestRedEnemy.transform.position) > AttackRange && GetComponent<MU_ObjectProperties>().enemiesassembled == false)
            {
                state = "movingintoposition";
            }
            if (Vector3.Distance(transform.position, ClosestRedEnemy.transform.position) > AttackRange && GetComponent<MU_ObjectProperties>().enemiesassembled == true)
            {
                print("im coming home");
                state = "movingtohomebase";
            }
            // check if youre in attack range//
            if (Vector3.Distance(transform.position, ClosestRedEnemy.transform.position) <= AttackRange)
            {
                Bl_withinattackingdistance = true;
            }
        }
        else if (GetComponent<MU_ObjectProperties>().typeofenemy == "RedEnemy")// if youre a red enemy
        {
            if (state == "Attack")
            {
                if (Vector3.Distance(transform.position, ClosestBlueEnemy.transform.position) > AttackRange && GetComponent<MU_ObjectProperties>().enemiesassembled == true)
                {
                    print("im coming home bbbbb");
                    state = "movingtohomebase";
                }
            }
            if (Vector3.Distance(transform.position, ClosestBlueEnemy.transform.position) > AttackRange && GetComponent<MU_ObjectProperties>().enemiesassembled==false)
            {
                state = "movingintoposition";
            }
            if(Vector3.Distance(transform.position, ClosestBlueEnemy.transform.position) > AttackRange && GetComponent<MU_ObjectProperties>().enemiesassembled == true)
            {
                state = "movingtohomebase";
            }
            //check if youre in attack range//
            if (Vector3.Distance(transform.position, ClosestBlueEnemy.transform.position) <= AttackRange)
            {
                Bl_withinattackingdistance = true;
            }
        }
    }
    void changestatesbasedondistance()
    {
        if (Vector3.Distance(transform.position, GO_Target.transform.position) < AttackRange)
        {
            print("Andy is right");
            Bl_withinattackingdistance = true;
        }
        if (Vector3.Distance(transform.position, GO_Target.transform.position) > AttackRange)
        {
            if (Bl_withinattackingdistance == true)
            {
                Bl_withinattackingdistance = false;
            }
        }
    }
    //void GetInStartingPositions()
    //{

    //}

    void ChangeTheStates()
    {
        if (Bl_withinattackingdistance && GetComponent<MU_ObjectProperties>().enemiesassembled != true)
        {
            state = "movingintoposition";
        }
        if (GetComponent<MU_ObjectProperties>().enemiesassembled == true && Bl_withinattackingdistance)
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
            else if (GetComponent<MU_ObjectProperties>().typeofenemy == "RedEnemy")
            {
                GO_Target = ClosestBlueEnemy;
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
        if (state == "movingintoposition")
        {
            MyNavmeshAgent.SetDestination(targettile.transform.position);
        }
        if(state=="movingtohomebase")
        {
            MyNavmeshAgent.SetDestination(GO_EnemyHomeBase.transform.position);
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
    //void FindClosestZonetoEnemy()
    //{

    //}
    void FindClosestEnemy()
    {
        arrayofenemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in arrayofenemies)
        {
            if (Vector3.Distance(go.transform.position,targettile.transform.position) <= Fl_MinDistaanceFromTile)
            {
                go.GetComponent<MU_ObjectProperties>().enemiesassembled = true;
            }
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
