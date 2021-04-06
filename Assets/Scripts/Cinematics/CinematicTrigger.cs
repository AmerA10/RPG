using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables; //relating to things such as playable director

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool isPlayed = false;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.transform.tag.Equals("Player") && !isPlayed)
            {
                GetComponent<PlayableDirector>().Play();
                isPlayed = true;
            }
           
        }
    }
}

