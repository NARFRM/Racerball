using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMov : MonoBehaviour
{
    
    public float torqueForce = 10f;  // Fuerza de torque (giro de la esfera)
    public Transform cameraTransform;  // Cámara para orientar el movimiento
    private Rigidbody rb;  // Componente Rigidbody de la esfera
    private bool isGrounded;  // Verifica si la esfera está en el suelo

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 20f; // Limita la velocidad angular

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main?.transform;
            if (cameraTransform == null)
            {
                Debug.LogError("No se encontró la cámara principal. Asigna una cámara al script.");
            }
        }
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Obtiene la dirección de movimiento basada en la cámara
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * moveZ + right * moveX).normalized;

        // Aplicar fuerza para el desplazamiento
        rb.AddForce(moveDirection * torqueForce, ForceMode.Force);

        // Aplicar torque para que la esfera ruede
        Vector3 torqueDirection = new Vector3(moveDirection.z, 0, -moveDirection.x); // Giro perpendicular al movimiento
        rb.AddTorque(torqueDirection * torqueForce, ForceMode.Force);

        
    }
    
    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                break;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}