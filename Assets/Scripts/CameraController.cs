using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    [SerializeField][Range(0.1f, 10)] private float mouvementSpeed = 0.5f;
    [SerializeField][Range(1, 180)] private float rotationSpeed = 45;
    [SerializeField][Range(1, 200)] private float mouseSpeed = 8f;
    [SerializeField][Range(1, 10)] private float turboSpeed = 2f;

    [SerializeField] bool useMouseLook = true;
    [SerializeField] CursorLockMode useLockState = CursorLockMode.Locked;

    private float _turbo;
    private float _h;
    private float _v;

    void Start()
    {
        if (useMouseLook)
        {
            Cursor.lockState = useLockState;
        }
    }

    void Update()
    {
        _turbo = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? turboSpeed : 1;

        float mouse = Input.GetAxis("Mouse X");

        _h = useMouseLook ? mouse : Input.GetAxis("Horizontal");
        _v = Input.GetAxis("Vertical");

        float xDirection = useMouseLook ? Input.GetAxis("Horizontal") : 0;
        float zDirection = _v * mouvementSpeed;

        Vector3 direction = new Vector3(xDirection, 0, zDirection).normalized * (_turbo * Time.deltaTime);

        transform.Translate(direction);

        if (useMouseLook)
        {
            transform.Rotate(new Vector3(0, mouse * mouseSpeed * Time.deltaTime, 0));
        }
        else
        {
            transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime * _h * _turbo));
        }


    }

}