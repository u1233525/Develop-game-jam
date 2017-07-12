using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MU_Bullet : MonoBehaviour
{
    public float Fl_BulletSpeed;
    public float Fl_Destructiontime;
    public GameObject PCInstanceClone;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        BulletMove();
        KillBullet();
    }
    void BulletMove()
    {
        transform.Translate(Vector3.forward * Fl_BulletSpeed);
    }
    void KillBullet()
    {
        Destroy(gameObject, Fl_Destructiontime);
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == PCInstanceClone)
        {
            col.gameObject.GetComponent<MU_ObjectProperties>().Fl_Health -= 10;
            Destroy(gameObject);

        }
        if(col.gameObject!=PCInstanceClone)
        {
            col.gameObject.GetComponent<MU_ObjectProperties>().Fl_Health -= 10;
            Destroy(gameObject);

        }
    }
}
