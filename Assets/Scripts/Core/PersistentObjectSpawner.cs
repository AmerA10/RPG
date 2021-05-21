using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab;
        // Start is called before the first frame updates
        static bool hasSpawned = false; //lives and dies with the application not with the instance of the class, generally its a bad idea but options are few

        private void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistentObjects();

            hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

