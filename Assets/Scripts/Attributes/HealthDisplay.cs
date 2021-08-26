using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
namespace RPG.Attributes
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;
        Health health;
        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
      
        }

        private void Update()
        {
            //takes the first thing on the write, and put it in the curley praces and add a % sign
            // the first 0 is the first thing on the right of the comma, the second 0 is the number of decimal places
            text.text = String.Format("{0:0}/{1:0}", health.GetHealth(), health.GetMaxHealth());
        }
    }

}

