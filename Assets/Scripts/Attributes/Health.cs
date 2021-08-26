using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using GameDevTV.Utils;
using UnityEngine.Events;
namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        LazyValue<float> health ;
        LazyValue<float> maxHealth;
        private bool isDead = false;

        [SerializeField] float regenerationPercentage = 70f;
        [SerializeField] TakeDamageEvent takeDamage;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
        }

        GameObject instigator;
        private void Awake()
        {
            //uses the lazyvalue class as a wrapper the value
            //Insures initialization is called before first use
            health = new LazyValue<float>(GetInitialHealth);
            maxHealth = new LazyValue<float>(GetMaxHealth);


        }

        private void Start()
        {
            //A method that forces initialization of the lazyValue
            health.ForceInit();
        }

        private float GetInitialHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health); 
        }

        private void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += LevelUpHealthUpdate;
        }

        private void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= LevelUpHealthUpdate;
        }


        public bool IsDead()
        {
            return this.isDead;
        }

        private void LevelUpHealthUpdate()
        {
            float currentPercentage = GetPercentage();
            this.maxHealth.value = GetComponent<BaseStats>().GetStat(Stat.Health);
            this.health.value = (Mathf.Max(currentPercentage, regenerationPercentage) / 100) * maxHealth.value;
      
        }
        
       

        public void TakeDamage(GameObject instigator, float damage)
        {
            Debug.Log(this.gameObject.name + " Took damage: " + damage);
            health.value = Mathf.Max(health.value - damage, 0); //if the health goes below zero then the health is zero 
            this.instigator = instigator;
            takeDamage.Invoke(damage);
            
            CheckForDeath();
           
            
        }

        public float GetHealth()
        {
            return this.health.value;
        }
        public float GetMaxHealth()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
          
        }

        public float GetPercentage()
        {
    
            return GetHealthFraction() * 100;
        }
        public float GetHealthFraction()
        {

            return (health.value / maxHealth.value);
        }
        private void CheckForDeath()
        {
            if (health.value <= 0)
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
            return this.health.value;
        }

        public void RestoreState(object state)
        {
            this.health.value = (float)state;
            Debug.Log("health at load is: " + health);
            CheckForDeath();
        }

    }

}

