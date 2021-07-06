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
        float health = -1f;
        private float maxHealth;
        private bool isDead = false;

        [SerializeField] float regenerationPercentage = 70f;

        GameObject instigator;
        private void Awake()
        {
            GetComponent<BaseStats>().onLevelUp += LevelUpHealthUpdate;
            if (health < 0)
            {
                health = GetComponent<BaseStats>().GetStat(Stat.Health);
                maxHealth = health;
            }
           
            
        }

        public bool IsDead()
        {
            return this.isDead;
        }

        private void LevelUpHealthUpdate()
        {
            float currentPercentage = GetPercentage();
            this.maxHealth = GetComponent<BaseStats>().GetStat(Stat.Health);
            this.health = (Mathf.Max(currentPercentage, regenerationPercentage) / 100) * maxHealth;
      
        }
        
       

        public void TakeDamage(GameObject instigator, float damage)
        {
            Debug.Log(this.gameObject.name + " Took damage: " + damage);
            health = Mathf.Max(health - damage, 0); //if the health goes below zero then the health is zero 
            this.instigator = instigator;
            CheckForDeath();
            
        }

        public float GetHealth()
        {
            return this.health;
        }
        public float GetMaxHealth()
        {
            return this.maxHealth;
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
            AwardExperience();
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
           

            
        }

        private void AwardExperience()
        {
            if(instigator == null)
            {
                return;
            }
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
            Debug.Log("health at load is: " + health);
            CheckForDeath();
        }

    }

}

