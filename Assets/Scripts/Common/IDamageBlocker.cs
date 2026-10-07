using UnityEngine;

public interface IDamageBlocker
{
    bool TryBlock(Vector2 damageSource);   
}
