using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum QuestStatus {
    PENDING,
    ACTIVE,
    RESOLVED,
    COMPLETE
}

public class GolemController : MonoBehaviour
{
    [SerializeField] private GameObject exclamationMark;
    [SerializeField] private GameObject questionMark;

    private QuestStatus questStatus;

    void Start()
    {
        questStatus = QuestStatus.PENDING;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Talk()
    {
        // TODO: crear un objeto con un script "quest manager"
        // el mismo se encargará de levantar la UI para mostrar la info
        // de la quest.
        // Adicionalmente, crear una clase que representa la información de la quest
        // y asignarle al golem dicha quest como disponible.

        if(questStatus == QuestStatus.PENDING)
        {
            // TODO: llamar al quest manager para mostrar la ventana correspondiente
            // con boton de acept / decline, por ahora, hago la lógica aca nomas..

            questStatus = QuestStatus.ACTIVE;
            exclamationMark.SetActive(false);
            questionMark.GetComponent<Renderer>().material.SetColor("_Color", Color.gray);
            questionMark.SetActive(true);
        }
        else if(questStatus == QuestStatus.ACTIVE)
        {
            // TODO: llamar al quest manager para mostrar la ventana correspondiente

            var color = exclamationMark.GetComponent<Renderer>().material.GetColor("_Color");
            questionMark.GetComponent<Renderer>().material.SetColor("_Color", color);
            questStatus = QuestStatus.RESOLVED;

        }else if (questStatus == QuestStatus.RESOLVED)
        {
            questStatus = QuestStatus.COMPLETE;

            questionMark.SetActive(false);
        }
    }
}
