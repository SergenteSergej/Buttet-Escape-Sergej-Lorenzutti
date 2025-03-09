using UnityEngine;

public class Destroy_Bullet_On_Collide : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        //if (other.gameObject.GetComponent<FireBulletsAtTarget>()) return;

        Destroy(gameObject);
    }
}