using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.AI;
using RPG.Core;


namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        //generates a GUID identifier and converst to String
        [SerializeField] string uniqueIdentifier = "";
        public string GetUniqueIdentifier()
        {
            return uniqueIdentifier;
        }
        //we are returning object because it is the root of all, does not matter what type as long as it is a serializable
        //basically means you can return anything
        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3)state;
            //probably should reset targets
            //cancel current actions
            this.GetComponent<NavMeshAgent>().enabled = false; //this simple just avoids some issues with the nav mesh agent
            this.GetComponent<NavMeshAgent>().enabled = true;
            this.GetComponent<ActionScheduler>().CancelCurrentAction();
            transform.position = position.getVector();
            
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
            

            if(string.IsNullOrEmpty(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
        }


    }
#endif
}
