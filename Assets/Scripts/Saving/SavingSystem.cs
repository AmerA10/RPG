using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace RPG.Saving {
    public class SavingSystem : MonoBehaviour
    {
        //there are better and more automatic ways of optomizing this type of saving system.
        //c# has a built in system for serialization
        //fundamentally it is all the same as the two serilaize and deserialization methods


        private void Start()
        {
            Debug.Log("SavePath: " + Application.persistentDataPath);
            Debug.Log("Debug on: " +gameObject.name, gameObject);
        }
        public void Save(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            using (FileStream stream = File.Open(path, FileMode.Create)) //creates an automic close when exiting the using statement
            {
   
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, CaptureState());

            };
    
         
        }

       

        public void Load(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);     
            using (FileStream stream = File.Open(path, FileMode.Open)) //creates an automatically closing stream when exiting the using statement
            {
            
     
                BinaryFormatter formatter = new BinaryFormatter();
                RestoreState(formatter.Deserialize(stream));

          
                
                //maybe change the target of the player to nothings
            };
        }

        

        private object CaptureState()
        {
            //the string will be the unique identifier
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>()) //gets everysingle object with a 'SaveableEntity' script on it
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }
            return state;
        }
        private void RestoreState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
            int numberOfStates = stateDict.Count;
            int checker = 0;
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>()) //gets everysingle object with a 'SaveableEntity' script on it
            {
                checker++;
                saveable.RestoreState(stateDict[saveable.GetUniqueIdentifier()]);
            }
            if(numberOfStates == checker)
            {
                Debug.Log("Number of cheks is correct");
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {

            //combines strings into a path
            //returns the save file path
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }


        //this is dead code
        //however, it is the bases for how serialization and deserialization works so I will keep it 

        private byte[] SerializeVector (Vector3 vector)
        {
            byte[] vectorBytes = new byte[3 * 4]; //three floats , each taking 4 bytes

            BitConverter.GetBytes(vector.x).CopyTo(vectorBytes, 0);//converts float to bytes
            BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 4);
            BitConverter.GetBytes(vector.z).CopyTo(vectorBytes, 8);

            return vectorBytes;
        }

        private Vector3 DeserializeVector(byte[] buffer)
        {
            Vector3 vector = new Vector3();
            //to single converts the Bit to a floating point integer
            vector.x = BitConverter.ToSingle(buffer, 0);
            vector.y = BitConverter.ToSingle(buffer, 4);
            vector.z = BitConverter.ToSingle(buffer, 8);

            return vector;
        }

      

        

    }
}


