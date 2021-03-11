using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        // Start is called before the first frame update
        GameObject player;
        Fighter fighter;
        Health health;
        void Start()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }
        // Update is called once per frame
        void Update()
        {
            if(health.IsDead())
            {
                return;
            }
            
            if (InAttackRange() && fighter.CanAttack(player))
            {
                Debug.Log(this.name + " Should give chase");
                fighter.Attack(player);
                
            }
            else
            {
                fighter.Cancel();
            }
        }

        private bool InAttackRange()
        {
            
            float distanceToPlayer = Vector3.Distance(player.transform.position, this.transform.position);
            return distanceToPlayer < chaseDistance ? true : false;
        }
    }
}

