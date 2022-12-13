using UnityEngine;

public class PlayerCore : MonoBehaviour
{
    [SerializeField] private int _hp = 200;

    private int maxHP;

    public bool IsAlive => _hp > 0;

    public int Hp => _hp;

    private void Awake()
    {
        maxHP = _hp;
    }

    public void Initialize()
    {
        _hp = maxHP;
    }
    public void TakeDamage(int amount)
    {
        if (!IsAlive)
        {
            return;
        }
        _hp = Mathf.Max(0, _hp - amount);
        if (!IsAlive)
        {
            OnDeath();
        }
    }
    private void Update()
    {

        if (Input.GetKeyUp(KeyCode.P))
        {
            TakeDamage(200);
        }

    }
    private void OnDeath()
    {
        GameManager.Instance.GameOver();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.CoreHit();
            if (enemy == null) return;
            TakeDamage(enemy.Damage);
            Destroy(enemy.gameObject);
        }
    }

}
