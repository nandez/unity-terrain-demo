using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    // Marcamos los campos con el decorado SerializeField
    // para setearlos desde el editor y evitar
    // que se accedan desde fuera de la clase.

    [SerializeField] private float speed = 10f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float jumpHeight = 2f;

    private CharacterController charCtrl;
    private Vector3 velocity;
    private bool isGrounded;

    private void Awake()
    {
        charCtrl = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Verificamos si el jugador esta tocando tierra o si se encuentra en el aire.
        isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Obtenemos el input de los ejes horizontal y vertical
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Generamos un vector para el movimiento, teniendo en cuenta la rotación
        // actual del jugador. De lo contrario, el movimiento siempre sería global,
        // independiente de hacia donde mira el jugador.
        var move = transform.right * x + transform.forward * z;

        // Utilizamos el método Move de la clase CharacterController para mover al jugador
        // hacia la posición deseada.
        charCtrl.Move(move * speed * Time.deltaTime);

        // Controlamos que al presionar el botón de salto, el jugador se encuentre
        // sobre el suelo; a partir de la formula v2 = h * -2 * g, podemos despejar
        // el valor de "y" que va a tener nuestro vector.
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Aplicamos la fuerza de gravedad e invocamos al método Move nuevamente, para
        // reflejar el cambio de altura en el jugador.
        velocity.y += gravity * Time.deltaTime;
        charCtrl.Move(velocity * Time.deltaTime);
    }
}