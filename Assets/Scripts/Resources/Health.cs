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
        private float maxHealth;
        private bool isDead = false;
     
        GameObject instigator;
        private void Start()
        {
            health = GetComponent<BaseStats>().GetStat(Stat.Health);
            maxHealth = health;

        }

        public bool IsDead()
        {
            return this.isDead;
        }

       

        public void TakeDamage(GameObject instigator, float damage)
        {
           
            health = Mathf.Max(health - damage, 0); //if the health goes below zero then the health is zero 
            this.instigator = instigator;
            CheckForDeath();
            
        }

        public float GetHealth()
        {
            return this.health;
        }

        public float GetPercentage()
        {
            return (health / maxHealth) * 100;
        }
        private void CheckForDeath()
        {
            if (health <= 0)
            {
                Die();
                AwardExperience();

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

        private void AwardExperience()
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null)
            {
                return;
                
            }
            else 
            {
                experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
            }
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

