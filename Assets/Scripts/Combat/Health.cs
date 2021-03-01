using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;
        private bool isDead = false;
        
        public bool IsDead()
        {
            return this.isDead;
        }
        
        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0); //if the health goes below zero then the health is zero
            Debug.Log("Enemy Health: " + health);
            if(health <= 0)
            {
                Die();
               
            }
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            
        }

        // Start is called before the first frame update
    
    }

}

