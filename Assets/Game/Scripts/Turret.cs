using EPOOutline;
using KBCore.Refs;
using Lean.Pool;
using UnityEngine;

public class Turret : MonoBehaviour, IUpgradeable, ISelectable
{
    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");
    public int Damage => turretData.Damage;
    public int Level => level;
    public float AttackDelay => turretData.AttackDelay;

    public bool IsUnlocked => level >= 0;
    public bool IsActive => IsUnlocked;

    [SerializeField, Anywhere] Transform radarPoint;
    [SerializeField, Anywhere] Transform projectileSpawnPoint;
    [SerializeField, Anywhere] TurretDataSO[] turretDatas;
    [SerializeField] TurretDataSO turretData;
    [SerializeField, Self] Animator animator;
    [SerializeField, Self] Outlinable outline;
    [SerializeField, Anywhere] GameObject gfx;

    private ProjectileType projectileType;
    private IDamageable Target;
    private float nextAttackTime;
    private int level;

    private void OnEnable()
    {
        outline.enabled = false;
        Target = null;
    }

    private void Update()
    {
        if (!IsUnlocked)
            return;
        FindTarget();
        LookTarget();
        Fire();
    }

    #region Seek&Destroy
    public void FindTarget()
    {
        if (Target is not null && Target.IsAlive)
        {
            return;
        }
        Target = Radar<IDamageable>.FindNearestTarget(transform.position, turretData.AttackRange, turretData.targetLayer);
    }
    private void LookTarget()
    {
        if (Target is null || !Target.IsAlive)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, turretData.RotationSpeed * Time.deltaTime);
            return;
        }
        Vector3 dir = Target.DamagePoint.position - transform.position;
        dir.Normalize();

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir, Vector3.up), turretData.RotationSpeed * Time.deltaTime);
    }
    private void Fire()
    {
        if (Target is null || !Target.IsAlive)
        {
            return;
        }

        if (Time.realtimeSinceStartup < nextAttackTime)
        {
            return;
        }
        CinemachineShake.Instance.ShakeCamera();
        animator.SetTrigger(FIRE_HASH);
        nextAttackTime = Time.realtimeSinceStartup + turretData.AttackDelay;
        ProjectileBehaviour projectile = LeanPool.Spawn(turretData.Projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        projectile.Seek(Target, turretData.Damage);

    }


    #endregion

    #region Selection Methods
    public void Select()
    {
        outline.enabled = true;
        Debug.Log("Turret selected ", gameObject);
        EventManager.Fire(new GameEvents.OnCameraChange(gameObject));
        EventManager.Fire(new GameEvents.OnTurretSelected(this));
    }

    public void Deselect()
    {
        outline.enabled = false;
        EventManager.Fire(new GameEvents.OnCameraChange(null));
    }
    #endregion

    #region Upgrade Methods
    public bool IsUpgradeable()
    {
        return level + 1 < turretDatas.Length;
    }

    public void Upgrade()
    {
        if (!IsUpgradeable())
        {
            Debug.LogError("Max Level!");
            return;
        }
        SetLevel(++level);
    }

    private void SetLevel(int level)
    {
        if (!IsUnlocked) return;
        turretData = turretDatas[level];
    }
    public void Init(int level, ProjectileType projectileType)
    {
        gfx.SetActive(level >= 0);
        this.level = level;
        this.projectileType = projectileType;
        SetLevel(level);
    }
    #endregion
}

public enum ProjectileType
{
    Bullet,
    Cannon
}