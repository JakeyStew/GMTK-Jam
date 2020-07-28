using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPerception : MonoBehaviour
{
    SphereCollider perceptionSphere;
    GameObject enemy;
    bool showLine = false;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.Find("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        if (showLine)
        {
            Debug.DrawRay(transform.position, enemy.transform.position - transform.position, Color.red);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            showLine = true;
        }
    }    
}
