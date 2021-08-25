using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {

        [SerializeField] TextMeshProUGUI text;

    

        public void SetDamageText(string amount)
        {
            this.text.text = string.Format("{0:0}", amount);
        }

        public void DestroyText()
        {
            Destroy(gameObject);
        }
    }
}

