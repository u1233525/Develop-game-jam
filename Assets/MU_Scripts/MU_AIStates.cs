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
    public NavMeshAgent MyNavmeshAgent;
    // Use this for initialization
    void Start()
    {
        state = "idle";
        MyNavmeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void changestatesbasedondistance()
    {
        if(Vector3.Distance(transform.position,GO_Target.transform.position)<AttackRange)
        {
            Bl_withinattackingdistance = true;
        }
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
        if(state=="Attack")
        {

        }
    }
}
