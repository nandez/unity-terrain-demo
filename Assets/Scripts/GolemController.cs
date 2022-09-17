using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemController : MonoBehaviour
{
    [Header("Quests Indicators")]
    [SerializeField] private GameObject exclamationMark;
    [SerializeField] private GameObject questionMark;

    [SerializeField] private QuestManager questMgr;
    [SerializeField] private Transform player;
    [SerializeField] private float maxInteractionDistance = 17f;
    [SerializeField] private float rotationResetSpeed = 10f;

    public bool IsTalking { get; private set; }
    private Quaternion initialRotation;

    void Start()
    {
        // Guardamos la rotación original del golem
        initialRotation = transform.rotation;
        IsTalking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsTalking)
        {
            // Determinamos la dirección en del jugador para girar el golem
            var playerDirection = player.position - transform.position;
            playerDirection.y = 0;
            var rotation = Quaternion.LookRotation(playerDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationResetSpeed * Time.deltaTime);

            // Calculamos la distancia al jugador, y en caso de ser mayor al máximo
            // deifnido por la variable maxInteractionDistance, cerramos el cuadro de díalogo.
            var distanceToPlayer = Vector3.Distance(player.position, transform.position);
            if (distanceToPlayer >= maxInteractionDistance)
            {
                questMgr.CloseQuestPanel();
                IsTalking = false;
            }
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, rotationResetSpeed * Time.deltaTime);
        }

        UpdateQuestMarkers();
    }

    public void Talk()
    {
        IsTalking = true;
        questMgr.OpenQuestPanel();
    }

    public void UpdateQuestMarkers()
    {
        if (questMgr.CurrentQuestStatus == QuestStatus.PENDING)
        {
            exclamationMark.SetActive(true);
            questionMark.SetActive(false);
        }
        else if (questMgr.CurrentQuestStatus == QuestStatus.ACTIVE)
        {
            exclamationMark.SetActive(false);
            questionMark.GetComponent<Renderer>().material.SetColor("_Color", Color.gray);
            questionMark.SetActive(true);
        }
        else if (questMgr.CurrentQuestStatus == QuestStatus.RESOLVED)
        {
            var color = exclamationMark.GetComponent<Renderer>().material.GetColor("_Color");
            questionMark.GetComponent<Renderer>().material.SetColor("_Color", color);
        }
        else if (questMgr.CurrentQuestStatus == QuestStatus.COMPLETE)
        {
            exclamationMark.SetActive(false);
            questionMark.SetActive(false);
        }
    }
}
