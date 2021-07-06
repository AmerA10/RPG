using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RPG.Stats {
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpParticleEffect = null;
        [SerializeField] bool shouldUseModifiers = false;

        public event Action onLevelUp;
       
        int currentLevel = 0;

        private void Start()
        {
           
            currentLevel = CalculateLevel();
            Experience experience = GetComponent<Experience>();
            if(experience != null)
            {
                experience.onExperienceGained += UpdateLevel; 
            }
        }


        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel)
            {
                currentLevel = newLevel;
                LevelUpEffect();
                onLevelUp();
                
            }
        }

        private void LevelUpEffect()
        {
            GameObject levelUp = Instantiate(levelUpParticleEffect, transform);
        }



        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + (GetPercentageModifier(stat) / 100));
        }

       

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }


        //this will allow each class to return its own modifier depending on the stat passed in
        private float GetAdditiveModifier(Stat stat)
        {
            if(!shouldUseModifiers)
            {
                return 0;
            }
            float sum = 0;
            IModifierProvider[] modifierProviders = GetComponents<IModifierProvider>();
            foreach (IModifierProvider modifier in modifierProviders)
            {
                foreach(float modifiers in modifier.GetAdditiveModifiers(stat))
                {
                    sum += modifiers;
                }
            }
            return sum;
        }
        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers)
            {
                return 0;
            }
            float sumPercentage = 0;
            IModifierProvider[] modifierProviders = GetComponents<IModifierProvider>();
            foreach (IModifierProvider modifer in modifierProviders)
            {
                foreach (float percentage in modifer.GetPercentageModifiers(stat))
                {
                    sumPercentage += percentage;
                }
            }
            return sumPercentage;
        }

        public int GetLevel()
        {
            if(currentLevel < 1)
            {
               currentLevel = CalculateLevel();
            }
            return currentLevel;
        }

        public int CalculateLevel()
        {

            Experience experience = GetComponent<Experience>();
            if (experience == null)
            {
                return startingLevel;
            }
            
            float currentXp = experience.GetExperience();
            int PenUltimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);
            for (int level = 1; level <= PenUltimateLevel; level++)
            {
                float XpToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                if (XpToLevelUp > currentXp)
                {
                    return level;
                }
            }

            return PenUltimateLevel + 1;

            //this can also be another way to get it howeever the above way is better because it does not use a while loop


           /* float maxXp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, startingLevel);
            //int level = startingLevel;
            //if we have more xp then the amount at this level
            while (currentXp >= maxXp)
            {
                level = startingLevel + 1;
                maxXp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
            }
            startingLevel = level;
            return level;*/
            
        }
       
    }

}

