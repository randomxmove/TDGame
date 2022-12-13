using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    public override void AimAt(Transform target, bool follow = true)
    {
        base.AimAt(target, follow);

        transform.LookAt(target);
    }


    public override void Update()
    {
        base.Update();
        
        if(target)
        {
            Rigidbody rigidbody = GetComponent<Rigidbody>();

            rigidbody.velocity = transform.forward * speed;

            var rocketTargetRot = Quaternion.LookRotation(target.position - transform.position);
            rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, rocketTargetRot, 200f));

        }
    }

}
