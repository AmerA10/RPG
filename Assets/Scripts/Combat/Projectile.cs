using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Attributes;
namespace RPG.Combat
{

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] float speed = 1f;
    [SerializeField] bool isHoming = true;
    [SerializeField] GameObject hitEffect = null;
    [SerializeField] float maxLifeTime = 10f;
    [SerializeField] GameObject[] destroyOnHit = null;
    [SerializeField] float lifeAfterImpact = 2f;

    Health target = null;
    float damage = 0;
    [SerializeField] GameObject instigator = null;
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

    public void SetTarget(Health target, GameObject instigator, float damage)
    {
        this.damage = damage;
        this.target = target;
        this.instigator = instigator;

        Destroy(gameObject, maxLifeTime);
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
        if(hitEffect != null)
        {
           GameObject spawnedHitEffect = Instantiate(hitEffect, GetAimLocation(), Quaternion.identity);
        }
        target.TakeDamage(instigator,this.damage);
        speed = 0;
        foreach(GameObject toDestroy in destroyOnHit)
        {
            Destroy(toDestroy);
        }

        Destroy(this.gameObject, lifeAfterImpact);


    }
}
}
