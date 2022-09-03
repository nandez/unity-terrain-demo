using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarController : MonoBehaviour
{
    [SerializeField] private string activationGemTag;

    [Header("Inner Stone Settings")]
    [SerializeField] private GameObject stone;
    [SerializeField] private float rotationDegreesPerSecond = 30f;
    [SerializeField] private float stoneBounceFrequency = 1f;
    [SerializeField] private float stoneBounceAmplitude = 0.025f;
    private Vector3 stoneInitialPosition;

    public bool IsActivated { get; private set; }

    
    

    private void Awake()
    {
        stone.SetActive(false);
    }

    private void Start()
    {
        stoneInitialPosition = stone.transform.position;
    }

    void Update()
    {
        if(IsActivated)
        {
            // Rotación de la piedra
            stone.transform.Rotate(new Vector3(0f, Time.deltaTime * rotationDegreesPerSecond, 0f), Space.World);

            // Oscilación de la piedra
            var position = stoneInitialPosition;
            position.y += Mathf.Sin(Time.fixedTime * Mathf.PI * stoneBounceFrequency) * stoneBounceAmplitude;
            stone.transform.position = position;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Detectamos si el pilar aun no ha sido activado y el objeto con el
        // que colisiona es la gema de activación
        if(collision.gameObject.CompareTag(activationGemTag) && !IsActivated)
        {
            IsActivated = true;
            stone.SetActive(true);

            collision.gameObject.SetActive(false);
            Destroy(collision.gameObject);
        }
    }
}
