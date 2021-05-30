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



        public void Save(string saveFile)
        {
           

            Dictionary<string, object> state  = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state); 

        }

       

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            //creates an automatically closing stream when exiting the using statement
            using (FileStream stream = File.Open(path, FileMode.Open)) 
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);

            }
        }

        private void SaveFile(string saveFile, object state)
        {
           
            string path = GetPathFromSaveFile(saveFile);

            //creates an automic close when exiting the using statement
            using (FileStream stream = File.Open(path, FileMode.Create)) 
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            };
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            //the string will be the unique identifier
           
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>()) //gets everysingle object with a 'SaveableEntity' script on it
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }
           
        }
        private void RestoreState(Dictionary<string, object> state)
        {
            Dictionary<string, object> stateDict = state;
            int numberOfStates = stateDict.Count;
            int checker = 0;
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>()) //gets everysingle object with a 'SaveableEntity' script on it
            {
                checker++;
                string id = saveable.GetUniqueIdentifier();
                if(stateDict.ContainsKey(id))
                {
                    saveable.RestoreState(stateDict[id]);
                }
            
            }
            if(numberOfStates == checker)
            {
                Debug.Log("Number of check is correct");
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


