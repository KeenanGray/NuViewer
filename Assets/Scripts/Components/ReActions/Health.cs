using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int MaxHealth = 5;
    int currentHealth;
    
     void Start() {
        currentHealth=MaxHealth;
    }
     void Update(){
        if(currentHealth<=0)
            Destroy(this.gameObject);
    }

    public void TakeDamage(int amt){
        Debug.Log("took " + amt + " damage");
        currentHealth-=amt;
    }
    public void HealDamage(int amt){   
        Debug.Log("healed " + amt + " damage");
        currentHealth+=amt;
    }
}
