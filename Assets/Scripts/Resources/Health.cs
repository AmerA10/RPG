using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float health;
        private bool isDead = false;

        private void Start()
        {
            health = GetComponent<BaseStats>().GetHealth();

        }

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
            else
            {
                GetComponent<Animator>().ResetTrigger("die");
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

