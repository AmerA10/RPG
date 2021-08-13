using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Resources;
using System;
using UnityEngine.EventSystems;

namespace RPG.Control {
    public class PlayerController : MonoBehaviour
    {

       
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType cursorType;
            public Texture2D cursorTexture;
            public Vector2 hotSpot;
        }

        
        [SerializeField] CursorMapping[] cursorMapping = null;

        Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
        }
        void Update()
        {
            if (InteractWithComponent()) return;
            if (InteractWithUI()) return;
            if(health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            } //if we dont hit an enemy then just move on to the next method
            if(InteractWithMovement()) return;
            SetCursor(CursorType.None);
        
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted(); //gives back a list of hits of everything that the ray hit
            foreach (RaycastHit hit in hits)
            {
               IRaycastable[] rayCastables =  hit.transform.GetComponents<IRaycastable>();
               foreach(IRaycastable raycastable in rayCastables)
                {
                    if(raycastable.HandleRaycast(this))
                    {
                        Debug.Log("Mouse hitting: " + raycastable);
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        RaycastHit[] RaycastAllSorted()
        {
            //Get All hits
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            
     
            //Sort BY distance
            //Build array distances
            float[] distances = new float[hits.Length];

            //Sort the hits
            Array.Sort(distances, hits);
            for(int i = 0; i < distances.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            //Return
            return hits;
        }

        private bool InteractWithUI()
        {
            //returns weather or not the cursor over a UI gameobject
            if(EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
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
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }


        private void SetCursor(CursorType type)
        {

            CursorMapping mapping = GetCursorMapping(type);
           Cursor.SetCursor(mapping.cursorTexture, mapping.hotSpot,CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach(CursorMapping cursorMap in cursorMapping)
            {
                if(cursorMap.cursorType == type)
                {
                    return cursorMap;
                }
                
            }
            return cursorMapping[0];
            
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}

