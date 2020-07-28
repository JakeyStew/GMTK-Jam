using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStageTransition : MonoBehaviour
{
    GameObject player;
    private void Start()
    {
        player = GameObject.Find("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Vector3.Distance(player.transform.position, transform.position) < 5)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
