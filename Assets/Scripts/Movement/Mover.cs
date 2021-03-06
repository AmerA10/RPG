using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {
        NavMeshAgent nav;
        Health health;

        [SerializeField] float maxSpeed = 6;
        [SerializeField] float maxNavPathLength = 40f;

        private void Awake()
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

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            //calculate the path between the player and the clicked position
            bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath)
            {
                //basically no path can be calculated to that place
                return false;
            }
            if (path.status != NavMeshPathStatus.PathComplete)
            {
                return false;
            }

            if (GetPathLength(path) > maxNavPathLength)
            {
                return false;
            }
            return true;
        }
        private float GetPathLength(NavMeshPath path)
        {
            Vector3[] corners = path.corners;
            float totalLength = 0;
            if (path.corners.Length < 2)
            {
                return 0;
            }
            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                totalLength += Vector3.Distance(path.corners[i], path.corners[i + 1]);
            }

            return 0f;
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
            SerializableVector3 position = (SerializableVector3) state;
            //probably should reset targets
            //cancel current actions
            this.GetComponent<NavMeshAgent>().enabled = false; //this simple just avoids some issues with the nav mesh agent
            transform.position = position.getVector();
            this.GetComponent<NavMeshAgent>().enabled = true;
            this.GetComponent<ActionScheduler>().CancelCurrentAction();
            
        }
    }


    //------------------
    //One way of returning multiple states
    /* public object CaptureState()
       {
           Dictionary<string, object> data = new Dictionary<string, object>();
           data["position"] = new SerializableVector3(transform.position);
           data["rotation"] = new SerializableVector3(transform.eulerAngles);
           return data;
       }

       public void RestoreState(object state)
       {
           Dictionary<string, object> data = (Dictionary<string, object> )state;
           //probably should reset targets
           //cancel current actions
           this.GetComponent<NavMeshAgent>().enabled = false; //this simple just avoids some issues with the nav mesh agent
           this.GetComponent<NavMeshAgent>().enabled = true;
           this.GetComponent<ActionScheduler>().CancelCurrentAction();
           transform.position = ((SerializableVector3)data["position"]).getVector(); 
           transform.eulerAngles = ((SerializableVector3)data["rotation"]).getVector();
       }
       */

    //---------------------
    //Another way is usig Structs
    /*
     * struct MoverSaveData
    {
        public SerializableVector3 position;
        public SerializableVector3 rotation;
    }

    //ISaveable interface
    public object CaptureState()
    {
        MoverSaveData data = new MoverSaveData();
        data.position = new SerializableVector3(transform.position);
        data.rotation = new SerializableVector3(transform.eulerAngles);
        return data;
    }

    public void RestoreState(object state)
    {
        MoverSaveData data = (MoverSaveData)state;
        //probably should reset targets
        //cancel current actions
        this.GetComponent<NavMeshAgent>().enabled = false; //this simple just avoids some issues with the nav mesh agent
        this.GetComponent<NavMeshAgent>().enabled = true;
        this.GetComponent<ActionScheduler>().CancelCurrentAction();
        transform.position = data.position.getVector();
        transform.eulerAngles = data.rotation.getVector();
    }
    */


}


