using UnityEngine;

public class PlayerPickUpController : MonoBehaviour
{
    // Marcamos los campos con el decorado SerializeField
    // para setearlos desde el editor y evitar
    // que se accedan desde fuera de la clase.

    [SerializeField] private float pickupRange = 5f;
    [SerializeField] private Transform pickupPoint;
    [SerializeField] private LayerMask pickableMask;
    [SerializeField] private float moveForce = 25f;
    [SerializeField] private PlayerLookController lookCtrl;

    private GameObject pickedItem;

    private void Update()
    {
        // Detectamos si el jugador presiona la tecla de agarre.
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (pickedItem == null)
            {
                // En este caso, el jugador no tiene ningun objeto agarrado;
                // utilizando la dirección en la que se encuentra mirando
                // el jugador, proyectamos un raycast de largo determinado por
                // la propiedad "pickupRange", cuyo objetivo es colisionar con
                // objetos que tengan asignada la capa especificada en la propiedad "pickableMask",
                var direction = lookCtrl.transform.TransformDirection(Vector3.forward);

                //Debug.DrawRay(transform.position, direction, Color.red, pickupRange);
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction, out hit, pickupRange, pickableMask))
                    PickUp(hit.transform.gameObject);
            }
            else
            {
                // En este caso, el jugador ya tiene un objeto agarrado, por lo que
                // al presionar la tecla de agarre, soltamos el objeto.
                DropItem();
            }
        }

        if (pickedItem != null)
        {
            MoveItem();
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

            itemRb.transform.parent = pickupPoint;
            pickedItem = item;
        }
    }

    private void DropItem()
    {
        if (pickedItem != null)
        {
            // Al soltar el objeto agarrado, le habilitamos la gravedad
            // y le restauramos el arrastre para que se comporte de forma normal.
            var pickedItemRb = pickedItem.GetComponent<Rigidbody>();
            pickedItemRb.useGravity = true;
            pickedItemRb.drag = 1;

            // Le resteamos el parent y limpiamos la variable que nos indica
            // que objeto estamos cargando.
            pickedItem.transform.parent = null;
            pickedItem = null;
        }
    }

    private void MoveItem()
    {
        var itemDistance = Vector3.Distance(pickedItem.transform.position, pickupPoint.position);

        if (itemDistance > 0.1f)
        {
            Vector3 moveDirection = pickupPoint.position - pickedItem.transform.position;
            pickedItem.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }
    }
}