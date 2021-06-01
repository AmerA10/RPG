using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;
        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

      public IEnumerator FadeOutIn()
        {
            yield return FadeOut(3f);
            Debug.Log("FadedOut");
            yield return FadeIn(1f);
            Debug.Log("FadedIn");
        }

       public IEnumerator FadeOut(float time)
        {
            //do this only for a limited amount of time
            while (canvasGroup.alpha < 1) //alpha is not 1, update it until it is 
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null; //null = 1 frame;
                //moveAlpha until it is 1 by the frame and time
            }
      
        }
       public IEnumerator FadeIn(float time)
        {
            //do this only for a limited amount of time
            while (canvasGroup.alpha > 0) //alpha is not 1, update it until it is 
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null; //null = 1 frame;
                //moveAlpha until it is 1 by the frame and time
            }
       
        }
        // Start is called before the first frame update
    }
}

