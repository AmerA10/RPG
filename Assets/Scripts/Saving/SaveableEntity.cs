using UnityEngine;
using UnityEditor;
using System;



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
        public object CaptureState()
        {
            Debug.Log("Capturing state for: " + GetUniqueIdentifier());
            return null;
        }

        public void RestoreState(object state)
        {
            Debug.Log("Restoring state for: " + GetUniqueIdentifier());
        }

        private void Update()
        {
       
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
}
