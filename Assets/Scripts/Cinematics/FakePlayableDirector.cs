using UnityEngine;
using System.Collections;
using System;
namespace RPG.Cinematics
{
    public class FakePlayableDirector : MonoBehaviour
    {
        
        public event Action<float> onFinish;

        // Use this for initialization
        void Start()
        {
            Invoke("OnFinish", 3f);
        }

        void OnFinish()
        {
            onFinish(4.3f);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}