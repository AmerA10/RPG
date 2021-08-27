using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using System;
using GameDevTV.Utils;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] float aggroCoolDownTime = 5f;
        [SerializeField] PatrolPath patrolPath; //the patrol path, can be null
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwelTime = 2f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f; //.2 of what max speed is
        [SerializeField] float shoutDistance = 5f;
        //just try to connect the speed to the nav mesh speed
        //speeds will be managed based on the state of the character
        // Start is called before the first frame update
        GameObject player;
        Fighter fighter;
        Health health;
        Mover mover;
        LazyValue<Vector3> gaurdPosition;
        float timeSinceLastSawPlayer = Mathf.Infinity; //never seen the player basically
        int currentWaypointIndex = 0;
        float timeSinceLastWayPoint = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;


        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();
            gaurdPosition = new LazyValue<Vector3>(GetTransformPosition);

        }

        void Start()
        {

            gaurdPosition.ForceInit();

        }
        // Update is called once per frame
        void Update()
        {
            if (health.IsDead())
            {
                return;
            }

            if (IsAggrevated() && fighter.CanAttack(player))
            {
                
                AttackBehaviour();

            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();

            }
            UpdateTimers();
        }

        public void Aggrevate()
        {
            timeSinceAggrevated = 0f;
        }

        private Vector3 GetTransformPosition()
        {
            return transform.position;
        }
        private void UpdateTimers()
        {
            timeSinceAggrevated += Time.deltaTime;
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceLastWayPoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {

            Vector3 nexPositioon = gaurdPosition.value;
            
            if (patrolPath != null)
            {

                if(AtWayPoint())
                {
                    timeSinceLastWayPoint = 0;
                    CycleWaypoint();
                }
                nexPositioon = GetCurrentWayPoint();
            }
            if(timeSinceLastWayPoint > waypointDwelTime)
            {
                mover.StartMoveAction(nexPositioon, patrolSpeedFraction);
                
            }
            
        }

        private Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWayPoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWayPoint()); //get the distance from current location to waypoint
            return distanceToWaypoint < waypointTolerance; //check if it is less than the tolerance waypoint
        }

        private void SuspicionBehaviour()
        {
            
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            timeSinceLastSawPlayer = 0;
            fighter.Attack(player);

            AggrevateNearByEnemies();
        }

        public void AggrevateNearByEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach(RaycastHit hit in hits)
            {
                AIController ai = hit.transform.GetComponent<AIController>();
                if (ai == null)
                {
                    continue;
                }
                hit.transform.GetComponent<AIController>().Aggrevate();
            }
        }

        private bool IsAggrevated()
        {
            
            float distanceToPlayer = Vector3.Distance(player.transform.position, this.transform.position);
            return distanceToPlayer < chaseDistance ? true : false || timeSinceAggrevated < aggroCoolDownTime;
        }

        //called by unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, chaseDistance);
        }
    }
}

