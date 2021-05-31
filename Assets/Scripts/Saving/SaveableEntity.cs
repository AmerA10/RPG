using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.AI;
using RPG.Core;

using System.Collections.Generic;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        //generates a GUID identifier and converst to String
        [SerializeField] string uniqueIdentifier = "";
        static Dictionary<string, SaveableEntity> globalLookUp = new Dictionary<string, SaveableEntity>();
        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }
        //we are returning object because it is the root of all, does not matter what type as long as it is a serializable
        //basically means you can return anything
        public object CaptureState()
        {

            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach(ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;
     
        }

        public void RestoreState(object state)
        {

            Dictionary<string,object> stateDict = (Dictionary<string, object>) state;
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string typeString = saveable.GetType().ToString();
                if (stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }
        }
#if UNITY_EDITOR
        private void Update()
        {
            /*
             * Quick Side Note
             * Since this code is only available in the unity editor
             * Afterall serialized objects only exists for the UNity editor
             * packaging the game will give an error as SerializedObject does not exist
             * In order to avoid this
             * Wrap the entire private void Update with a #if UNITY_EDITOR and then at the end add a #end if
             */
       
            if (Application.IsPlaying(gameObject)) return ;
            //prefabs do not have a path therefore it is possible to have no modifiers
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;


            SerializedObject serializedObject = new SerializedObject(this);
            //since the serialized property is genereic there is defined value type 
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");
            

            if(string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookUp[property.stringValue] = this;
 
        }

        private bool IsUnique(string candidate)
        {
            // 1. Check that the key exists in the dictionary
            // 2. Check that it does only point to it self

       

          

            if(!globalLookUp.ContainsKey(candidate))
            {
                return true;
            }
            if (globalLookUp[candidate] == this)
            {
                return true;
            }

            //One issue is that a static exists between scenes howeer the SaveableEntities objects dont therefore they become null
            //So when scenes get loaded again the Unique Identifiers in the dictionary are now pointing to a null object making it 'not unique'
            //So as a result the SaveableEntitiy generates another GUID
            //to fix this, simply do the following check

            if (globalLookUp[candidate] == null) //if its already deleted then its null, so remove it
            {
                globalLookUp.Remove(candidate);
                return true;
            }

            //if for some reasaon the dictionary has 2 different Guid pointing to the same entity then remove one
            if(globalLookUp[candidate].GetUniqueIdentifier() != candidate)
            {
                globalLookUp.Remove(candidate);
                return true;
            }

            return false;

        }


    }
#endif
}
