using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "progression", menuName = "Stats/NewProgression", order = 0)]
    public class Progression : ScriptableObject
    {

        [SerializeField] ProgressionCharacterClass[] characterClasses;
        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {

            foreach (ProgressionCharacterClass progressionClass in characterClasses) {

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

            return 0;
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

