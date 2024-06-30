using UnityEngine;

public static class Radar<T> where T : IDamageable
{
    private static readonly Collider[] results = new Collider[10];

    public static T FindNearestTarget(Vector3 origin, float radius, LayerMask targetLayer)
    {
        int found = Physics.OverlapSphereNonAlloc(origin, radius, results, targetLayer);
        T bestTarget = default;
        if (found > 0)
        {
            float closestDistanceSqr = Mathf.Infinity;
            for (int i = 0; i < found; i++)
            {
                Vector3 directionToTarget = results[i].transform.position - origin;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    //T tmpTarget = results[i].transform.GetComponent<T>();
                    if (results[i].TryGetComponent<T>(out T tmpTarget) && tmpTarget.IsAlive)
                    {
                        closestDistanceSqr = dSqrToTarget;
                        bestTarget = tmpTarget;
                    }
                }
            }
        }
        return bestTarget;
    }
}
