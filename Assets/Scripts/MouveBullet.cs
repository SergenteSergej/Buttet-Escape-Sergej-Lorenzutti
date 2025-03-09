using UnityEngine;

public class Mouve_Bullets : MonoBehaviour
{
    private float _speed = 1;
    public void Configure(float speed)
    {
        _speed = speed;
        Destroy(gameObject, 3);
    }
    void Update()
    {
        transform.Translate(Vector3.up * (Time.deltaTime * _speed), Space.Self);
    }
}