using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using RPG.Resources;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;
        Health health;
        Fighter fighter;
        private void Awake()
        {
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            //takes the first thing on the write, and put it in the curley praces and add a % sign
            // the first 0 is the first thing on the right of the comma, the second 0 is the number of decimal places
            
            if(fighter.GetTarget() == null)
            {
                text.text = "No Target";
                return;
            }
            
                health = fighter.GetTarget();
                text.text = String.Format("{0:0.0}%", health.GetPercentage());
            
        }
    }

}

