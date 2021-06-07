using UnityEngine;
using System.Collections;
using RPG.Core;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName ="Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject {

        [SerializeField] AnimatorOverrideController animatorOverride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        public void Spawn(Transform rightHand, Transform leftHand,  Animator animator)
        {
            if(equippedPrefab != null)
            {
                Transform handTransform = GetHandTransform(rightHand, leftHand);
                Instantiate(equippedPrefab, handTransform);


            }
            if (animatorOverride != null)
            {
                animator.runtimeAnimatorController = animatorOverride;
            }
         
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

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, this.weaponDamage);
        }

        public float GetDamage()
        {
            return weaponDamage;
        }
        public float GetRange()
        {
            return weaponRange;
        }

    }



}