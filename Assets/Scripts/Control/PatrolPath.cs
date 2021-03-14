using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control {
    public class PatrolPath : MonoBehaviour
    {
        // Start is called before the first frame update
        const float wayPointGizmoRadius = 0.4f;
        private void OnDrawGizmos()
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWaypoint(i), wayPointGizmoRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));

            }
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }

        public int GetNextIndex(int i)
        {
            if(i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }
    }



}


