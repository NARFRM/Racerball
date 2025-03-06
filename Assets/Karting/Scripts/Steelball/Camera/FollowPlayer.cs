using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0f, 5f, -6f);
    public float RotationSpeed = 200.0f;
    public float zoomSpeed = 2.0f;
    public float minZoom = 2f;
    public float maxZoom = 10f;
    public float smoothSpeed = 5f; // Suavizado de movimiento
    public float zoomSmoothSpeed = 10f; // Suavizado del zoom

    private float yaw = 0f;
    private float pitch = 0f;
    private float targetZoom;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        targetZoom = offset.magnitude; // Iniciar con el zoom actual
    }

    void LateUpdate()
    {
        // Movimiento con el mouse
        float mouseX = Input.GetAxis("Mouse X") * RotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * RotationSpeed * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -30f, 60f);

        // Control de zoom con suavizado
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        targetZoom -= scroll * zoomSpeed;
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        // Aplicar suavizado al zoom
        float smoothZoom = Mathf.Lerp(offset.magnitude, targetZoom, Time.deltaTime * zoomSmoothSpeed);
        offset = offset.normalized * smoothZoom;

        // Aplicar la rotación y el desplazamiento suavemente
        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 rotatedOffset = targetRotation * offset;
        Vector3 desiredPosition = player.transform.position + rotatedOffset;

        // Movimiento suave hacia la nueva posición
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
        transform.LookAt(player.transform.position);
    }
}
