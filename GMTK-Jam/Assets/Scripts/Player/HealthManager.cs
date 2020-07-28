using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    float maxHealth = 100;
    float currentHealth;
    public Image healthImage;
    public CapsuleCollider capsuleCollider;

    private void Start()
    {
        currentHealth = maxHealth;
        updateUI();
    }

    public void loseHealth(float lose)
    {
        if (currentHealth > 0)
        {
            currentHealth -= lose;
            updateUI();
        }
    }

    public void gainHealth(float gain)
    {
        currentHealth += gain;
        updateUI();
    }

    void updateUI()
    {
        float tempHealth = currentHealth / 100;
        healthImage.fillAmount = tempHealth;
    }

    private void OnTriggerEnter(Collider other)
    {       

        if (other.tag == "Enemy")
        {
            if (Vector3.Distance(transform.position, other.transform.position) < 3)
            {
                loseHealth(10);
            }
        }
    }



}
