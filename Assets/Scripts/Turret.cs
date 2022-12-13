using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : BaseBuilding
{
    [SerializeField] Transform turretBase;
    [SerializeField] Transform turretBarrel;
    [SerializeField] private EnemyType targetType;

    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float range = 3;
    [SerializeField] private float rotationSpeed = 100;
    [SerializeField] protected int damage;

    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawn;

    private float fireTimer = 0f;
    protected Transform target;


    protected virtual void Start()
    {
        InvokeRepeating("UpdateTarget", 0, 0.5f);
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        fireTimer -= Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("SHOOOOT");
            GameObject bulletObject = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
            Projectile projectile = bulletObject.GetComponent<Projectile>();

        }

        if (target == null) return;
                

        Ray ray = new Ray(turretBarrel.position, projectileSpawn.forward);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo, range))
        {
            if (hitinfo.transform.tag == "Building")
                Debug.DrawRay(turretBarrel.position, projectileSpawn.forward * range, Color.red);
            if (hitinfo.transform.tag == "Enemy")
                Debug.DrawRay(turretBarrel.position, projectileSpawn.forward * range, Color.green);
        }
        else
            Debug.DrawRay(turretBarrel.position, projectileSpawn.forward * range, Color.gray);


        // Rotate Turret
        Quaternion lookRotation = Quaternion.LookRotation(target.position - turretBase.position);
        Quaternion targetRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        turretBase.rotation = Quaternion.RotateTowards(turretBase.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Rotate Barrel 
        Quaternion barrelRotation = Quaternion.LookRotation(target.position - turretBarrel.position);
        Quaternion targetXRotation = Quaternion.Euler(barrelRotation.eulerAngles.x, turretBase.eulerAngles.y, 0);
        turretBarrel.rotation = Quaternion.RotateTowards(turretBarrel.rotation, targetXRotation, rotationSpeed * Time.deltaTime);

        // Fire bullets
        if (fireTimer <= 0f)
        {
            // Check if turret is already looking at target before shooting
            if (Quaternion.Angle(targetRotation, turretBase.rotation) < 1)
            {
                if (hitinfo.transform != null)
                {
                    if (hitinfo.transform.tag == "Enemy")
                    {
                        Shoot();
                        fireTimer = 1f / fireRate;
                    }
                }
            }

        }
    }

    public virtual void Shoot()
    {
        if (projectilePrefab == null) return;

        GameObject bulletObject = Instantiate(projectilePrefab, projectileSpawn.position, projectileSpawn.rotation);
        Projectile projectile = bulletObject.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Damage = damage;
            projectile.AimAt(target);
        }
    }

    private void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(target == null)
        {
            float closestDistance = Mathf.Infinity;

            GameObject nearestEnemy = null;

            foreach (GameObject enemyObject in enemies)
            {
                Enemy enemy = enemyObject.GetComponent<Enemy>();
                if (targetType == enemy.Type)
                {

                    float distance = Vector3.Distance(transform.position, enemyObject.transform.position);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        nearestEnemy = enemyObject;
                    }
                }
            }

            if (nearestEnemy != null && closestDistance <= range)
            {
                Enemy enemy = nearestEnemy.GetComponent<Enemy>();
                target = enemy.EnemyTargetPoint;
            }
        }
        else
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance > range) target = null;
        }
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.DrawRay(projectileSpawn.position, projectileSpawn.forward);


    }
}
