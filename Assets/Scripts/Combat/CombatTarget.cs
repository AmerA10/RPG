
using UnityEngine;
using RPG.Resources;
using RPG.Control;
namespace RPG.Combat {
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController callingController)
        {

            if (!callingController.GetComponent<Fighter>().CanAttack(this.gameObject))
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {

                callingController.GetComponent<Fighter>().Attack(this.gameObject);

            }
            return true; //even if we are hovering its still true

        }
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }
    }

}


