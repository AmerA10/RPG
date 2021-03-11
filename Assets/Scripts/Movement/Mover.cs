using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        NavMeshAgent nav;
        Health health;
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

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destinatio)
        {
            nav.isStopped = false;
            nav.destination = destinatio;
        }

        public void Cancel()
        {
            nav.isStopped = true;
        }

  

        private void UpdateAnimator()
        {
            Vector3 velocity = nav.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity); // transforms the vector from a world space to local space
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

       
    }

}


