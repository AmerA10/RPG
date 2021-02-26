using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform target;
    NavMeshAgent nav;
    

    Ray lastRay;
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        nav.destination = target.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MoveToCursor();
           
        }
        UpdateAnimator();
       
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = nav.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity); // transforms the vector from a world space to local space
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("forwardSpeed", speed);
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit =  Physics.Raycast(ray, out hit); //takes the ray and hit and stores info into the hit, the method is a bool return type
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 1f);
        if (hasHit)
        {
            nav.destination = hit.point;
        }
        
    }
}
