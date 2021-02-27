using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] Transform target;
        void Start()
        {

        }

        // Update is called once per frame
        void LateUpdate() //happens after normal update in the execution, this will make sure that the camera will move after the animations
        {
            this.transform.position = target.position;
        }
    }

}

