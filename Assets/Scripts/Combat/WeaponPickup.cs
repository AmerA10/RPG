using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Attributes;
namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        // Start is called before the first frame update

        [SerializeField] WeaponConfig weapon = null;
        [SerializeField] float healthToRestore = 0f;
        [SerializeField] float respawnTime = 5f;

 

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject)
        {
            Debug.Log("picking up weapon");
            if (weapon != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(weapon);
            }
            if(healthToRestore > 0)
            {
                subject.GetComponent<Health>().Heal(healthToRestore);
            }
            
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
                Pickup(callingController.gameObject);
            }
            return true;
        }
        public CursorType GetCursorType()
        {
            return CursorType.PickUp;
        }
    }

   
}

