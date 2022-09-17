using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockZoneTriggerController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] fallingRocks;
    [SerializeField] private ParticleSystem[] dustEffect;


    private void OnTriggerEnter(Collider coll)
    {
        // Verificamos si el jugador llega a la zona de acción
        if (coll.gameObject == player)
        {
            // Activamos todos los efectos de particulas para simular el derrumbe
            foreach (var dustFx in dustEffect)
            {
                dustFx.Play(false);
            }

            // Activamos la gravedad de cada piedra que debe caer y quitamos las restricciones
            // de movimiento y rotación.
            foreach (var rock in fallingRocks)
            {
                var currentRockRb = rock.GetComponent<Rigidbody>();
                currentRockRb.useGravity = true;
                currentRockRb.constraints = 0;
            }
        }
    }
}
