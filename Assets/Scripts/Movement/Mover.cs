using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        NavMeshAgent nav;
        Health health;

        [SerializeField] float maxSpeed = 6;
        void Start()
        {
            health = GetComponent<Health>();
            nav = GetComponent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            nav.enabled = !health.IsDead();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            nav.isStopped = false;
            nav.speed = maxSpeed * (Mathf.Clamp01(speedFraction));
            nav.destination = destination;
        }

        

        private void UpdateAnimator()
        {
            Vector3 velocity = nav.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity); // transforms the vector from a world space to local space
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        //IAction Interface
        public void Cancel()
        {
            nav.isStopped = true;
        }

        //ISaveable interface
        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            //probably should reset targets
            //cancel current actions
            this.GetComponent<NavMeshAgent>().enabled = false; //this simple just avoids some issues with the nav mesh agent
            this.GetComponent<NavMeshAgent>().enabled = true;
            this.GetComponent<ActionScheduler>().CancelCurrentAction();
            transform.position = position.getVector(); 
        }
    }

}


