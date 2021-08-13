using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        // Start is called before the first frame update

        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5f;

 

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                Pickup(other.GetComponent<Fighter>());
            }
        }

        private void Pickup(Fighter fighter)
        {
            Debug.Log("picking up weapon");
            fighter.EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

       
        private void ShowPickup(bool shouldShow)
        {
            GetComponent<SphereCollider>().enabled = shouldShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);

            }
        }
        public bool HandleRaycast(PlayerController callingController)
        {
            if(Input.GetMouseButton(0))
            {
                Pickup(callingController.GetComponent<Fighter>());
            }
            return true;
        }
        public CursorType GetCursorType()
        {
            return CursorType.PickUp;
        }
    }

   
}

