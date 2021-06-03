using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        // Start is called before the first frame update

        [SerializeField] Weapon weapon = null;

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                Debug.Log("picking up weapon");
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                Destroy(this.gameObject, 0.02f);
            }
        }

    }
}

