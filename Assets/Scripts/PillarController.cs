using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarController : MonoBehaviour
{
    [SerializeField] private GameObject activationGem;

    [Header("Inner Stone Settings")]
    [SerializeField] private GameObject stone;
    [SerializeField] private float rotationDegreesPerSecond = 30f;
    [SerializeField] private float stoneBounceFrequency = 1f;
    [SerializeField] private float stoneBounceAmplitude = 0.025f;


    private Vector3 stoneInitialPosition;
    private new Renderer renderer;
    private Color emissionColor;

    public bool IsActivated { get; private set; }

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        stone.SetActive(false);
    }

    private void Start()
    {
        stoneInitialPosition = stone.transform.position;

        // Guardamos el valor actual del color para la propiedad Emision
        // y lo reseteamos para dejar "inactivo" el detalle en el material.
        emissionColor = renderer.material.GetColor("_EmissionColor");
        renderer.material.SetColor("_EmissionColor", Color.black);
    }

    void Update()
    {
        if (IsActivated)
        {
            // Rotación de la piedra
            stone.transform.Rotate(new Vector3(0f, Time.deltaTime * rotationDegreesPerSecond, 0f), Space.World);

            // Oscilación de la piedra
            var position = stoneInitialPosition;
            position.y += Mathf.Sin(Time.fixedTime * Mathf.PI * stoneBounceFrequency) * stoneBounceAmplitude;
            stone.transform.position = position;
        }
    }

    public void ActivateElement(GameObject activationGem)
    {
        // Verificamos si la gema es la correcta
        if (activationGem == this.activationGem && !IsActivated)
        {
            // Activamos la piedra flotante..
            IsActivated = true;
            stone.SetActive(true);

            // Restauramos el color de emision para el material.
            renderer.material.SetColor("_EmissionColor", emissionColor);

            // Destruimos la piedra que porta el jugador..
            activationGem.SetActive(false);
            Destroy(activationGem.gameObject);

            // Finalmente reseteamos la capa del pilar para evitar
            // que se pueda interactuar nuevamente.
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
