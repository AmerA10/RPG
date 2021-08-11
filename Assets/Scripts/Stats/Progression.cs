using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "progression", menuName = "Stats/NewProgression", order = 0)]
    public class Progression : ScriptableObject
    {

        [SerializeField] ProgressionCharacterClass[] characterClasses;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> LookupTable = null; 
        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {

            BuildLookUp();

            float[] levels = LookupTable[characterClass][stat];
            if (levels.Length < level)
            {
                return 0;
            }

            
            return levels[level - 1];

            /* foreach (ProgressionCharacterClass progressionClass in characterClasses) {

                 if(progressionClass.characterClass != characterClass)
                 {
                     continue;
                 }
                 foreach(ProgressionStat characterStat in progressionClass.stats)
                 {
                     if(characterStat.stat != stat)
                     {
                         continue;
                     }
                     if(characterStat.levels.Length < level)
                     {
                         continue;
                     }
                     return characterStat.levels[level - 1];
                 }
             }

             return 0;*/
        }

        public int GetLevels(Stat stat, CharacterClass characterClass)
        {
            BuildLookUp();

            float[] levels = LookupTable[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookUp()
        {
            if(LookupTable != null)
            {
                return;
            }
            LookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            foreach(ProgressionCharacterClass progressionClass in characterClasses)
            {
                Dictionary <Stat, float[]> statLookupTable = new Dictionary<Stat, float[]>(); 

                foreach (ProgressionStat characterStat in progressionClass.stats)
                {
                    statLookupTable[characterStat.stat] = characterStat.levels;
                }

                LookupTable[progressionClass.characterClass] = statLookupTable;
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
            
        }
        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;

        }




    }
}

