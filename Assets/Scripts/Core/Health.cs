using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
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
            CheckForDeath();
        }

        private void CheckForDeath()
        {
            if (health <= 0)
            {
                Die();

            }
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            
        }

        public object CaptureState()
        {
            return this.health;
        }

        public void RestoreState(object state)
        {
            this.health = (float)state;
            CheckForDeath();
        }

    }

}

