using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTurret : Turret
{
    [SerializeField] private Transform projectileSpawn1;

    public override void Shoot()
    {
        base.Shoot();

        GameObject bulletObject = Instantiate(projectilePrefab, projectileSpawn1.position, projectileSpawn1.rotation);
        Projectile projectile = bulletObject.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Damage = damage;
            projectile.AimAt(target);
        }
    }
}
