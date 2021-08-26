using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {

        [SerializeField] Health healthComponent = null;
        [SerializeField] RectTransform healthBarRect = null;
        [SerializeField] Canvas rootCanvas = null;
        // Update is called once per frame
        void Update()
        {

            if(Mathf.Approximately(healthComponent.GetHealthFraction(), 0) 
            || Mathf.Approximately(healthComponent.GetHealthFraction(), 1)) 
            {
                rootCanvas.enabled = false;
            }
            rootCanvas.enabled = true;

            healthBarRect.localScale = new Vector3(healthComponent.GetHealthFraction(), 1, 1);
        }
    }

}

