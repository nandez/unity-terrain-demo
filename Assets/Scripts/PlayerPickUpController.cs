using UnityEngine;

public class PlayerPickUpController : MonoBehaviour
{
    // Marcamos los campos con el decorado SerializeField
    // para setearlos desde el editor y evitar
    // que se accedan desde fuera de la clase.

    [Header("Picked Item Settings")]
    [SerializeField] private Transform holdPoint;
    [SerializeField] private float moveForce = 25f;
    [SerializeField] private float throwForce = 150f;

    public GameObject CurrentItem { get; private set; }

    private void Update()
    {
        if (CurrentItem != null)
            MoveItem();

        if (Input.GetButtonDown("Fire1") && CurrentItem != null)
        {
            // En este caso, el jugador ya tiene un objeto agarrado, por lo que
            // al presionar el botón de disparo, soltamos el objeto.
            ThrowItem();
        }
    }

    public bool CanBePicked(GameObject item)
    {
        return CurrentItem == null;
    }

    public void PickUpItem(GameObject item)
    {
        if (CanBePicked(item))
        {
            var itemRb = item.GetComponent<Rigidbody>();
            if (itemRb != null)
            {
                // Deshabilitamos la gravedad del objeto agarrado para evitar que se caiga
                // e incrementamos el valor de drag para ralentizar al objeto.
                itemRb.useGravity = false;
                itemRb.drag = 10;

                itemRb.transform.SetParent(holdPoint);
                CurrentItem = item;
            }
        }
    }

    private void ThrowItem()
    {
        if (CurrentItem != null)
        {
            // Resteamos el parent del objeto..
            CurrentItem.transform.SetParent(null);

            // Al soltar el objeto agarrado, le habilitamos la gravedad
            // y le restauramos el arrastre para que se comporte de forma normal.
            var pickedItemRb = CurrentItem.GetComponent<Rigidbody>();
            pickedItemRb.useGravity = true;
            pickedItemRb.drag = 0;

            // Aplicamos una fuerza para lanzar al objeto.
            pickedItemRb.AddForce(holdPoint.transform.forward * throwForce);

            // Finalmente limpiamos la variable que nos indica que objeto estamos cargando.
            CurrentItem = null;
        }
    }

    private void MoveItem()
    {
        var itemDistance = Vector3.Distance(CurrentItem.transform.position, holdPoint.position);

        var currentItemRb = CurrentItem.GetComponent<Rigidbody>();

        if (itemDistance > 0.1f)
        {
            // Mientras el objeto se encuentre lejos del punto de sotén, calculamos la
            // dirección de movimiento y aplicamos una fuerza para mover al objeto.
            Vector3 moveDirection = holdPoint.position - CurrentItem.transform.position;
            currentItemRb.AddForce(moveDirection * moveForce);
        }
        else
        {
            // Una vez que el objeto esta próximo al punto de sostén, entonces
            // reseteamos la propiedad velocity.
            currentItemRb.velocity = Vector3.zero;
        }
    }
}
