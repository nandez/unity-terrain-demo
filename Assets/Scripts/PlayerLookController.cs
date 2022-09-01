using UnityEngine;

public class PlayerLookController : MonoBehaviour
{
    // Marcamos los campos con el decorado SerializeField
    // para setearlos desde el editor y evitar
    // que se accedan desde fuera de la clase.

    [SerializeField] private Transform playerBody;
    [SerializeField] private float mouseSensivity = 100f;

    private float xRotation = 0f;

    private void Start()
    {
        // Bloqueamos el cursor para evitar hacer click fuera de la pantalla.
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Obtenemos el input del mouse mediante los ejes y los normalizo
        var mouseX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        var mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;

        // Utilizamos el valor del eje vertical del mouse, para establecer la rotación
        // del objeto (en este caso la cámara) y con clamp, nos aseguramos de proyectar
        // el valor en un rango entre -90 y 90, para evitar que al mirar hacia arriba
        // o abajo, la camara siga rotando hacia atras.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Finalmente, utilizamos el valor del eje horizontal del mouse para girar en
        // el eje 'y' al objeto que representa al jugador.
        playerBody.Rotate(Vector3.up * mouseX);
    }
}