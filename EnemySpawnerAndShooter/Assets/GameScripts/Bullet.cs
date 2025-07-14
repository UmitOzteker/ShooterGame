using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 50f;
    public float speed = 50f;

    public void Initialize(float damageValue, float speedValue)
    {
        damage = damageValue;
        speed = speedValue;
    }
}
