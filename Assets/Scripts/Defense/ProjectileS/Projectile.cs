using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected Transform target;
    protected GameObject attacker;    
    protected float damageValue;  

    [Tooltip("Optional hit VFX prefab")]
    public GameObject hitEffectPrefab;

    public AudioClip shootSound;

    /// <summary>Call immediately after Instantiate()</summary>
    /// 
    protected virtual void Start()
    {
        // Base initialization
    }

    public void Initialize(Transform Target, MonoBehaviour Owner)
    {
        target = Target;
        attacker = Owner.gameObject;

        if (Owner is Defense def)
            damageValue = def.attackDamage;
        else
            damageValue = 0f;

        //if (shootSound != null)
          //  AudioManager.Instance.PlaySFX(shootSound);
    }

    protected virtual void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        MoveTowardsTarget();
    }

    protected abstract void MoveTowardsTarget();

    protected virtual void HitTarget()
    {
        // 1) optional VFX
        if (hitEffectPrefab != null)
            Instantiate(hitEffectPrefab, transform.position, transform.rotation);

        // 2) apply damage
        if (target.TryGetComponent<Damageable>(out var dmg))
            dmg.ApplyDamage(damageValue, attacker);

        if (shootSound != null)
            AudioManager.Instance.PlaySFX(shootSound);

        // 3) destroy self
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
            HitTarget();
    }
}
