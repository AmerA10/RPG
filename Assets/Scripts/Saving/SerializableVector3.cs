using UnityEngine;
namespace RPG.Saving
{
    //allows the class to be Serializable 
    [System.Serializable]
    public class SerializableVector3
    {

        float x, y, z;


        public SerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }
    }

}


