using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{

    private Vector3 initialDistance;

    private void Start()
    {
        initialDistance = transform.position;    
    }

    public override void AimAt(Transform target, bool follow = false)
    {
        base.AimAt(target, follow);
    }

    public override void Update()
    {
        base.Update();

        float distance = Vector3.Distance(initialDistance, transform.position);

        if (distance < 5)
        {
            GetComponent<Rigidbody>().velocity = transform.forward * speed;
        }
        else
        {
            GetComponent<Rigidbody>().velocity = transform.forward * 0;

        }
    }
}
