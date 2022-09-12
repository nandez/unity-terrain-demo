using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerPickUpController))]
public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject hud;

    [Header("Interaction Settings")]
    [SerializeField] private float interactionRange = 5f;
    [SerializeField] private LayerMask interactionMask;
    [SerializeField] private KeyCode actionKey;

    private CrosshairController crosshairController;
    private PlayerPickUpController pickupController;

    private void Awake()
    {
        crosshairController = hud.GetComponent<CrosshairController>();
        pickupController = GetComponent<PlayerPickUpController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Proyectamos un rayo desde el centro de la pantalla hacia la longitud determinada
        // por la propiedad "interactionRange", para determinar si el mismo colisiona con algún
        // objeto que tenga seteada la capa que definimos para los objetos con los que se puede
        // interactuar.
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        RaycastHit rHit;

        if (Physics.Raycast(ray, out rHit, interactionRange, interactionMask))
        {
            var target = rHit.collider.gameObject;

            // Comparamos los tags del collider para mostrar el mensaje correspondiente.
            if(target.CompareTag("Gemstone"))
            {
                // Verificamos si el objeto puede levantarse, evitando por ejemplo, mostrar
                // el mensaje de acción para el mismo objeto que estamos cargando.
                var isPickable = pickupController.CanBePicked(target);

                if(isPickable)
                    crosshairController.SetInteractiveState(actionKey.ToString(), "Pick Item");

                if(Input.GetKeyDown(actionKey) && isPickable)
                    pickupController.PickUpItem(target);
            }
            else if(target.CompareTag("Golem"))
            {
                crosshairController.SetInteractiveState(actionKey.ToString(), "Talk");
            }
            else if(target.CompareTag("ActivationPillar"))
            {
                crosshairController.SetInteractiveState(actionKey.ToString(), "Activate");
                if(Input.GetKeyDown(actionKey))
                    target.GetComponent<PillarController>().ActivateElement(pickupController.CurrentItem);
            }
        }
        else
        {
            // Reseteamos al estado normal
            crosshairController.ResetState();
        }
    }
}
