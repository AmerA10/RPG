using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;
        Experience experience;
        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            //takes the first thing on the write, and put it in the curley praces and add a % sign
            // the first 0 is the first thing on the right of the comma, the second 0 is the number of decimal places
            text.text = experience.GetExperience().ToString();
        }
    }

}

