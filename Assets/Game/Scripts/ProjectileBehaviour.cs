using Lean.Pool;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [Header("Settings")]
    private float autoRemoveTimer;
    public int Damage; //TODO: silaha göre farklı damage vereceksek bunu nasıl yapmamız gerekir?
    [Header("Movement")]
    public float movementSpeed;

    [Header("Auto Remove Projectile")]
    public float autoRemoveCountdown;

    public LayerMask layerMask;
    private IDamageable target;

    public bool DamagePopUp { get; internal set; }

    void OnEnable()
    {
        autoRemoveTimer = 0;
        //Seek(null, 0);
    }
    public void Seek(IDamageable target, int damage)
    {
        this.target = target;
        Damage = damage;
        Vector3 dir = target.DamagePoint.position - transform.position;
        transform.forward = dir;
    }
    void Update()
    {
        Vector3 tempPos = transform.position;
        transform.position += transform.forward * movementSpeed * Time.deltaTime;

        if (Physics.Linecast(tempPos, transform.position, out RaycastHit hitInfo, layerMask))
        {
            if (hitInfo.transform.TryGetComponent(out IDamageable target))
            {
                target.TakeDamage(Damage);
                if (DamagePopUp)
                {
                    //TODO: bu mermi damage pop up çıkartacak!
                    //VFXManager.Instance.DamageFloatingText(hitInfo.point, Damage.ToString(), Color.red);
                }
                //VFXManager.Instance.BloodSplash(hitInfo.point);
            }
            else
            {
                //VFXManager.Instance.Spark(hitInfo.point + (hitInfo.normal * .01f), Quaternion.FromToRotation(Vector3.forward, hitInfo.normal));
            }


            // VFXManager.Instance.Spark(hitInfo.point, sparkSize);
            RemoveProjectile();
        }
        else
        {
            autoRemoveTimer += Time.deltaTime;
            if (autoRemoveTimer >= autoRemoveCountdown)
            {
                RemoveProjectile();
            }
        }

    }


    void RemoveProjectile()
    {
        LeanPool.Despawn(this);
    }
}