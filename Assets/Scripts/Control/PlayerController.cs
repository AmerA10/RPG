using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Resources;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour
    {

        Health health;

        private void Start()
        {
            health = GetComponent<Health>();
        }

       
        void Update()
        {
            if(health.IsDead())
            {
                return;
            }
            if (InteractWithCombat()) return; //if we dont hit an enemy then just move on to the next method
            if(InteractWithMovement()) return;
        
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay()); //gives back a list of hits of everything that the ray hit
            foreach(RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;
                
                if (!GetComponent<Fighter>().CanAttack(target.gameObject))
                {
                    continue;
                }

                if(Input.GetMouseButton(0))
                {   

                 GetComponent<Fighter>().Attack(target.gameObject);

                }
                return true; //even if we are hovering its still true

            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit); //takes the ray and hit and stores info into the hit, the method is a bool return type
            if (hasHit)
            {
                if(Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

