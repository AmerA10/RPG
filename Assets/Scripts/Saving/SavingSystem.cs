using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

namespace RPG.Saving {
    public class SavingSystem : MonoBehaviour
    {
        public void Save(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            using (FileStream stream = File.Open(path, FileMode.Create)) //creates an automic close when exiting the using statement
            {
                Transform playerTransform = GetPlayerTransform();
                byte[] buffer = SerializeVector(playerTransform.position);
                stream.Write(buffer, 0, buffer.Length);
            };
    
         
        }
        public void Load(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);     
            using (FileStream stream = File.Open(path, FileMode.Open)) //creates an automic close when exiting the using statement
            {
                Transform playerTransform = GetPlayerTransform();
               
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

            vector.x = BitConverter.ToSingle(buffer, 0);
            vector.y = BitConverter.ToSingle(buffer, 4);
            vector.z = BitConverter.ToSingle(buffer, 8);

            return vector;
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath , saveFile + ".sav");
        }
        
        private Transform GetPlayerTransform()
        {
            return GameObject.FindGameObjectWithTag("Player").transform;
        }

    }
}


