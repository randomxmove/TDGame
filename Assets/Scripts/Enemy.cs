using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform canvasTransform;
    [SerializeField] private Transform targetPointTransform;
    [SerializeField] private Image healthUI;
    [SerializeField] private int maxHealth;
    [SerializeField] private int damage;
    [SerializeField] private int score;
    [SerializeField] private int funds;
    [SerializeField] private float speed = 1f;
    [SerializeField] private EnemyType type;
    [SerializeField] private ParticleSystem deathEffects;
    [SerializeField] private ParticleSystem coreHitEffects;

    private Animator animator;
    private Transform target;
    private int pointIndex;
    private int currentHealth;

    public Action<Enemy> onDeath;
    public Action<Enemy> onDestroyed;

    public Transform EnemyTargetPoint => targetPointTransform;

    public Waypoints Path { get; set; }

    public EnemyType Type => type;
    public int Funds => funds;
    public int Score => score;
    public int Damage => damage;

    public void SetMultiplier(int levelMultiplier)
    {
        speed = speed * levelMultiplier;
        maxHealth = maxHealth + (20 * (levelMultiplier - 1));
    }

    private void Awake()
    {
        gameObject.SetActive(false);

    }
    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {

        Vector3 dir = target.position - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) <= 0.2f)
        {
            pointIndex++;
            if (pointIndex >= Path.Points.Length) return;

            target = Path.Points[pointIndex];
            transform.LookAt(target);
        }
        
    }
    private void LateUpdate()
    {
        canvasTransform.LookAt(Camera.main.transform);
    }

    public void Spawn()
    {
        float spawnY = Path.Points[0].position.y;

        target = Path.Points[0];
        transform.position = new Vector3(transform.position.x, spawnY, transform.position.z);
        transform.LookAt(target);
        currentHealth = maxHealth;
        gameObject.SetActive(true);
    }


    private void Hit(int damage)
    {
        currentHealth -= damage;
        healthUI.fillAmount = (float)currentHealth / (float)maxHealth;

        animator.SetTrigger("Take Damage");

        if (currentHealth <= 0) Die();

    }

    private void Die()
    {
        animator.SetTrigger("Die");
        if (onDeath != null) onDeath.Invoke(this);

        Instantiate(deathEffects, EnemyTargetPoint.position, Quaternion.identity);

        Destroy(gameObject);
    }

    public void CoreHit()
    {
        animator.SetTrigger("Attack 02");
        Instantiate(coreHitEffects, EnemyTargetPoint.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Projectile")
        {
            Projectile projectile = collision.gameObject.GetComponent<Projectile>();
            if (projectile == null) return;
            Hit(projectile.Damage);
            Destroy(projectile.gameObject);
        }
    }

    private void OnDestroy()
    {
        onDestroyed.Invoke(this);
    }
}
