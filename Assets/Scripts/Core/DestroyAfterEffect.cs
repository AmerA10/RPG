using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {

        void Update()
        {
            //check weather or not the particle system will produce more particles. Basically : Is finished
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}

