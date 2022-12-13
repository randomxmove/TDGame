using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] protected float speed = 1f;

    protected Transform target;

    public int Damage { get => damage; set => damage = value; }

    public virtual void AimAt(Transform _target, bool follow = false)
    {
        target = _target;
    }

    public virtual void Update()
    {
        //if (target == null)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
    }

    public virtual void OnHit()
    {

    }
}
