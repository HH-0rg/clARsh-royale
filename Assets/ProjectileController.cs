using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CRC;
using UnityEngine.Networking;

public class ProjectileController : MonoBehaviour
{
    Damageable projTarget;
    float projDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Fire(Damageable target, float speed, float damage)
    {
        projTarget = target;
        projDamage = damage;
        this.transform.LookAt(target.transform);
        this.GetComponent<Rigidbody>().velocity = speed * this.transform.forward;
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(GameObject.ReferenceEquals(collision.gameObject, projTarget.gameObject))
        {
            projTarget.Hurt(projDamage);
            this.gameObject.SetActive(false);
            GameObject.Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
