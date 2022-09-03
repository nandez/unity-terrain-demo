using UnityEngine;

public class PlayerPickUpController : MonoBehaviour
{
    // Marcamos los campos con el decorado SerializeField
    // para setearlos desde el editor y evitar
    // que se accedan desde fuera de la clase.

    [SerializeField] private float pickupRange = 5f;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private LayerMask pickableMask;
    [SerializeField] private float moveForce = 25f;
    [SerializeField] private float throwForce = 150f;
    [SerializeField] private Camera cam;


    [SerializeField] private GameObject pickedItem;

    private void Update()
    {
        // Detectamos si el jugador presiona la tecla de agarre.
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pickedItem == null)
            {
                // Como el jugador no tiene ningún objeto agarrado, proyectamos un rayo
                // desde el centro de la pantalla hacia una longitud determinada por la propiedad
                // pickupRange, para determinar si el mismo colisiona con algún objeto que tenga
                // seteada la capa especificada en la propiedad "pickableMask".
                Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, pickupRange,  pickableMask))
                    PickUp(hit.transform.gameObject);
            }
        }

        if (pickedItem != null)
        {
            MoveItem();
        }


        if(Input.GetButtonDown("Fire1") && pickedItem != null)
        {
            // En este caso, el jugador ya tiene un objeto agarrado, por lo que
            // al presionar el botón de disparo, soltamos el objeto.
            ThrowItem();
        }
    }

    private void PickUp(GameObject item)
    {
        var itemRb = item.GetComponent<Rigidbody>();
        if (itemRb != null)
        {
            // Deshabilitamos la gravedad del objeto agarrado para evitar que se caiga
            // e incrementamos el valor de drag para ralentizar al objeto.
            itemRb.useGravity = false;
            itemRb.drag = 10;

            itemRb.transform.SetParent(holdPoint);
            pickedItem = item;
        }
    }

    private void ThrowItem()
    {
        if (pickedItem != null)
        {
            // Resteamos el parent del objeto..
            pickedItem.transform.SetParent(null);

            // Al soltar el objeto agarrado, le habilitamos la gravedad
            // y le restauramos el arrastre para que se comporte de forma normal.
            var pickedItemRb = pickedItem.GetComponent<Rigidbody>();
            pickedItemRb.useGravity = true;
            pickedItemRb.drag = 0;

            // Aplicamos una fuerza para lanzar al objeto.
            pickedItemRb.AddForce(holdPoint.transform.forward * throwForce);

            // Finalmente limpiamos la variable que nos indica que objeto estamos cargando.
            pickedItem = null;
        }
    }

    private void MoveItem()
    {
        var itemDistance = Vector3.Distance(pickedItem.transform.position, holdPoint.position);

        if (itemDistance > 0.1f)
        {
            // Mientras el objeto se encuentre lejos del punto de sotén, calculamos la
            // dirección de movimiento y aplicamos una fuerza para mover al objeto.
            Vector3 moveDirection = holdPoint.position - pickedItem.transform.position;
            pickedItem.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }
        else
        {
            // Una vez que el objeto esta próximo al punto de sostén, entonces
            // reseteamos la propiedad velocity.
            pickedItem.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}