using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Weapon defaultWeapon = null;
        

        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon currentWeapon = null;


        Health target;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            EquipWeapon(defaultWeapon);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
          
            if (target == null) return;
            if (target.IsDead())
            {
                Debug.Log("Target is dead");
                Cancel();
                return;
            }

            if (!GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else
            {
                GetComponent<Mover>().Cancel(); //this cancel is used to stop moving for the range
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            //This will trigger the Hit() Event 
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                TriggerAttack(); //Trigger the attakc animation
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        //Animation Event
        void Hit()
        {
            //probably have to do a null check
            if(target == null) { return; }
            target.TakeDamage(currentWeapon.GetDamage());
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject target)
        {
            if (target == null) { return false; }
            Health targetToTest = target.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Cancel()
        {
            
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
            target = null;
            GetComponent<Mover>().Cancel();
        }


        public void EquipWeapon(Weapon weapon)
        {
            Debug.Log("changing weapons");
            currentWeapon = weapon; 
            Animator anim = GetComponent<Animator>();
            weapon.Spawn(this.rightHandTransform, this.leftHandTransform, anim);

        }
    }
}

