
using UnityEngine;

public interface Damage_Interface 
{
    public void OnHit(float damage, bool isCrit, Vector2 knockbackForce, float knockbackTime);
}
