using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] float speed = 1f;

    Health target = null;
    float damage = 0;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
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

            target.TakeDamage(this.damage);
            Destroy(this.gameObject);

    }
}
