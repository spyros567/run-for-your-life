using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    int currentHealth;
    int maxHealth=100;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentHealth<0)
        {
            currentHealth = 0;
            Debug.Log("PSOFISA");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        Debug.Log("Health:" + currentHealth);
    }
}
