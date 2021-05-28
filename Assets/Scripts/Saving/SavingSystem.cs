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
        }
        public void Save(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            using (FileStream stream = File.Open(path, FileMode.Create)) //creates an automic close when exiting the using statement
            {
                Transform playerTransform = GetPlayerTransform();
         
                BinaryFormatter formatter = new BinaryFormatter();
                SerializableVector3 position = new SerializableVector3(playerTransform.position);
                formatter.Serialize(stream, position);


            };
    
         
        }
        public void Load(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);     
            using (FileStream stream = File.Open(path, FileMode.Open)) //creates an automatically closing stream when exiting the using statement
            {
                Transform playerTransform = GetPlayerTransform();
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                playerTransform.position = DeserializeVector(buffer);
                
                //maybe change the target of the player to nothing
            };
        }

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

        private string GetPathFromSaveFile(string saveFile)
        {
            
            //combines strings into a path
            //returns the save file path
            return Path.Combine(Application.persistentDataPath , saveFile + ".sav");
        }
        
        private Transform GetPlayerTransform()
        {
            return GameObject.FindGameObjectWithTag("Player").transform;
        }

    }
}


