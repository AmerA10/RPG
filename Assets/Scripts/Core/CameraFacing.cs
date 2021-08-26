using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace RPG.Core
{
    public class CameraFacing : MonoBehaviour
    {
        void LateUpdate()
        {
        //the transform of the text would be the same as the forwad of the camera
            transform.forward = Camera.main.transform.forward;
        }

    }
}

