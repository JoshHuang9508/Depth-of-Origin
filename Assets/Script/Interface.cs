
using UnityEngine;

public interface Damageable
{
    public void OnHit(float damage, bool isCrit, Vector2 knockbackForce, float knockbackTime);
}

public interface KeyRequired
{
    public bool haveKey();
}