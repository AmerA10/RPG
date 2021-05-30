using UnityEngine;
using UnityEditor;
using System;

namespace RPG.Saving
{
    public class SaveableEntity : MonoBehaviour
    {
        public string GetUniqueIdentifier()
        {
            return "";
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



    }
}
