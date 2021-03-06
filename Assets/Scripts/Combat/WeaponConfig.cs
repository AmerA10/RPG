using UnityEngine;
using RPG.Attributes;
namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName ="Weapons/Make New Weapon", order = 0)]
    public class WeaponConfig : ScriptableObject {

        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] Weapon equippedPrefab = null;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponPercentageBonus = 0f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        const string weaponName = "Weapon";

        public Weapon Spawn(Transform rightHand, Transform leftHand,  Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);
            Weapon weapon = null;
            if (equippedPrefab != null)
            {
                Transform handTransform = GetHandTransform(rightHand, leftHand);
                weapon =  Instantiate(equippedPrefab, handTransform);
                weapon.transform.name = weaponName;

            }
            //this will be null if it is just the runtime animator controller not the animatorOverrideController
            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }

            /*if the weapon picked up doeos not contain an animatorOverride and the one picked 
            and the current player animator runTimeAnimatorController is an overrideController then 
            the animator runtimeAnimatorController should be the root of the overrideController, which then would be the default
            we dont want the animator to use ethe previous override controller, so
            if the player previous animator is an override, and the current override is null then revert to the root animator */


            else if (overrideController != null)
                
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
                
            }
            return weapon;
         
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(weaponName);
            if(oldWeapon == null)
            {
                oldWeapon = leftHand.Find(weaponName);
            }
            if(oldWeapon == null)
            {
                return;
            }

            oldWeapon.name = "DESTROYING";
            Destroy(oldWeapon.gameObject);
        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            Transform handTransform;
            if (isRightHanded)
            {
                handTransform = rightHand;
            }
            else
            {
                handTransform = leftHand;
            }

            return handTransform;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator , calculatedDamage);
        }

        public float GetDamage()
        {
            return weaponDamage;
        }

        public float GetPercentageBonus()
        {
            return weaponPercentageBonus;
        }

        public float GetRange()
        {
            return weaponRange;
        }

    }



}