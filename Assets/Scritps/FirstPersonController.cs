using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float speed = 5f;
    
    void Update()
    {
        // Movimiento
        float moveDirectionY = Input.GetAxis("Vertical");
        float moveDirectionX = Input.GetAxis("Horizontal");

        Vector3 move = transform.right * moveDirectionX + transform.forward * moveDirectionY;
        transform.Translate(move * speed * Time.deltaTime, Space.World);
    }
}
