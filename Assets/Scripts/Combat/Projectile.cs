using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] float speed = 1f;
    [SerializeField] bool isHoming = true;

    Health target = null;
    float damage = 0;
    void Start()
    {
        transform.LookAt(GetAimLocation());
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if(isHoming && !target.IsDead())
        {
            transform.LookAt(GetAimLocation());
        }
        
        
    }

    public void SetTarget(Health target, float damage)
    {
        this.damage = damage;
        this.target = target;
    }

    private Vector3 GetAimLocation()
    {
        
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if(targetCapsule == null)
        {
            return target.transform.position;
        }
        else
        {
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != target) return;
        if(target.IsDead())
        {
            return;
            
        }
        target.TakeDamage(this.damage);
        Destroy(this.gameObject);


    }
}
