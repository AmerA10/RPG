using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using RPG.Saving;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Weapon defaultWeapon = null;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon currentWeapon = null;
        [SerializeField] string defaultWeaponName = "Unarmed";


        Health target;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            //saves from the race condition with the Loading of the restore state
            if(currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
            
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

            if (target == null) { return; }
            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(this.rightHandTransform, this.leftHandTransform, target);
            }
            else
            {
                
                target.TakeDamage(currentWeapon.GetDamage());
            }
            //probably have to do a null check
            
        }

        //bow animation Event
        void Shoot()
        {
            Hit();
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

        public object CaptureState()
        {
            return currentWeapon.name;
        }

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon =  Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}

