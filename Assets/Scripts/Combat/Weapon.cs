using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace RPG.Combat
{ 
    public class Weapon : MonoBehaviour
    {
        [SerializeField] UnityEvent onHit;
        public void OnWeaponHit()
        {
            onHit.Invoke();
            Debug.Log("The weapon: " + gameObject.name + " is hitting");
        }

    }
}

