using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using RPG.Saving;

namespace RPG.Stats {
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0f;

        //public delegate void ExperienceGainedDelegate();
        //Action is a predefined delegate that has no return type
        public event Action onExperienceGained;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained();
        }

        public object CaptureState()
        {
            return this.experiencePoints;
        }

        public void RestoreState(object state)
        {
           
            float experienceToLoad = (float)state;
            this.experiencePoints = experienceToLoad;
        }

        public float GetExperience()
        {
            return this.experiencePoints;
        }
    }
}


