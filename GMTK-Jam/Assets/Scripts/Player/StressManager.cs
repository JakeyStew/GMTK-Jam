using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StressManager : MonoBehaviour
{
    float currentStress;
    float maxStress = 100;
    public Image stressImage;
    float destressTimer = 0.2f;
    bool destressB;
    int enemyCount;
    private bool mayhemBool = false;
   

    private void Update()
    {        
        if (mayhemBool)
        {
            if (destressTimer > 0)
            {
                destressTimer -= Time.deltaTime;
            }
            else
            {
                destressOverTime();
                destressTimer = 0.025f;
            }

            if (getStressLevel() <= 0)
            {
                currentStress = 0;
                mayhemBool = false;
            }
            return;
        }

        if (destressB)
        {
            if (currentStress > 0)
            {
                if (destressTimer > 0)
                {
                    destressTimer -= Time.deltaTime;
                }
                else
                {
                    destressOverTime();
                    destressTimer = 0.2f;
                }
            }
        }
        else
        {
            if (destressTimer > 0)
            {
                destressTimer -= Time.deltaTime;
            }
            else
            {
                AddStress(enemyCount);
                destressTimer = 0.2f;
            }
        }
    }

    public void setEnemyCount(int change)
    {
        enemyCount += change;
        if (enemyCount < 0)
        {
            enemyCount = 0;
        }
        if (enemyCount == 0)
        {
            //destressB = true;
        }
        else
        {
            destressB = false;
        }
    }

    public void AddStress(int addedStress)
    {
        currentStress += addedStress;
        checkMaxStress();
        updateUi();
    }

    private void destressOverTime()
    {
        currentStress -= 1;
        updateUi();
    }

    private void updateUi()
    {
        float tempStress = currentStress / 100;
        stressImage.fillAmount = tempStress;
    }


    private void checkMaxStress()
    {
        if (currentStress > maxStress)
        {
            currentStress = maxStress;
        }
    }


    public float getStressLevel()
    {
        return currentStress;
    }


    public bool getMayhemBool()
    {
        if (getStressLevel() == 100)
        {
            mayhemBool = true;
        }
        else if (getStressLevel() < 100 && !mayhemBool)
        {
            mayhemBool = false;
        }
        return mayhemBool;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyBody")
        {
            GameObject enemyObject = other.gameObject;
            setEnemyCount(1);
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "EnemyBody")
        {
            setEnemyCount(-1);            
        }
    }
}
