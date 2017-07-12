using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MU_AIStates : MonoBehaviour
{
    public string state;
    public bool Bl_withinattackingdistance=false;
    public bool Bl_withinchasingdistance = false;
    public GameObject GO_Target;
    public float ChaseRange;
    public float AttackRange;
    public Vector3  CriticalDistanceOffset;
    public float StoppingDistance;
    public NavMeshAgent MyNavmeshAgent;
    public GameObject GO_Bullet;
    public GameObject GO_BulletSpawner;
    public bool Bl_canshoot;
    public float Fl_TimeDelayforFirstShot;
    public float Fl_TimeDelayBetweenShots;
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
    }
    void changestatesbasedondistance()
    {
        if(Vector3.Distance(transform.position,GO_Target.transform.position)<AttackRange)
        {
            Bl_withinattackingdistance = true;
        }
    }
    void GetInStartingPositions()
    {

    }
    void ChangeTheStates()
    {
        if(Bl_withinattackingdistance)
        {
            state = "Attack";
        }
    }
    void DifferentStateReactions()
    {
        if (state == "Attack")
        {
            transform.LookAt(GO_Target.transform);
            MyNavmeshAgent.SetDestination(GO_Target.transform.position);
            if (Vector3.Distance(transform.position, GO_Target.transform.position) <= StoppingDistance)
            {
                MyNavmeshAgent.SetDestination(transform.position);
                state = "Shooting";
            }
        }
       if(state=="Shooting")
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
            GameObject.Instantiate(GO_Bullet, GO_BulletSpawner.transform.position,transform.rotation);
        }
    }
    void FindClosestZonetoEnemy()
    {

    }
    void FindClosestEnemy()
    {

    }
}
