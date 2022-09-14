using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    [Header("Quest Panel Settings")]
    [SerializeField] private GameObject questPanel;
    [SerializeField] private TMPro.TMP_Text questTitle;
    [SerializeField] private TMPro.TMP_Text questText;

    [Header("Quest Tracker Settings")]
    [SerializeField] private GameObject questTrackerPanel;
    [SerializeField] private TMPro.TMP_Text questTrackerMessage;
    [SerializeField] private CanvasGroup questTrackerCanvasGroup;

    [Header("Pillar References")]
    [SerializeField] private PillarController firePillar;
    [SerializeField] private PillarController earthPillar;
    [SerializeField] private PillarController waterPillar;

    [Header("Controller References")]
    [SerializeField] private CrosshairController crossHairCtrl;

    [Header("Panel Fade Settings")]
    [SerializeField] private float fadeDuration;



    public QuestStatus CurrentQuestStatus { get; private set; }

    private void Start()
    {
        questPanel.SetActive(false);
        questTrackerPanel.SetActive(false);

        CurrentQuestStatus = QuestStatus.PENDING;

        RefreshQuestState();
    }

    public void OpenQuestPanel()
    {
        // Deshabilitamos la mira y mostramos el cursor para habilitar la selección en el hud.
        crossHairCtrl.HideCrosshair();
        crossHairCtrl.SetCursorLockState(CursorLockMode.Confined);

        // Actualizamos el estado de la quest para mostrar los textos correspondientes.
        RefreshQuestState();

        // Mostramos el detalle de la quest..
        questPanel.SetActive(true);
    }

    public void CloseQuestPanel()
    {
        questPanel.SetActive(false);

        // Reseteamos el estado de la mira y ocultamos el cursor.
        crossHairCtrl.ResetState();
        crossHairCtrl.SetCursorLockState(CursorLockMode.Locked);
    }

    public void CloseButtonHandler()
    {
        // La transición de estos estados se realiza mediante el click del panel.
        if (CurrentQuestStatus == QuestStatus.PENDING)
        {
            // TODO: para simplificar el alcance del obligatorio, al cerrar el cuadro de dialogo
            // automáticamente aceptamos la quest. Queda pendiente implementar un mecanismo para
            // aceptar / rechazar una quest.
            CurrentQuestStatus = QuestStatus.ACTIVE;
            UpdateQuestTracker();
        }
        else if (CurrentQuestStatus == QuestStatus.RESOLVED)
        {
            // Al cerrar el panel de la quest, si la misma se encuentra resuelta
            // (todos los objetivos se cumplieron), la misma pasa a finalizada.
            CurrentQuestStatus = QuestStatus.COMPLETE;
            UpdateQuestTracker();
        }

        CloseQuestPanel();
    }

    /// <summary>
    /// Este método se bindea desde el editor, para ser ejecutado cuando los pilares
    /// emiten el evento de activación. Se actualiza el estado de la quest en base
    /// al estado de activación de los pilares.
    /// </summary>
    public void OnPillarActivatedHandler()
    {
        // Actualizamos la lista de objetivos.
        UpdateQuestTracker();

        // Si la quest se encuentra activa y todos los pilares estan activos,
        // marcamos la quest como resuelta.
        if (CurrentQuestStatus == QuestStatus.ACTIVE && firePillar.IsActivated
             && earthPillar.IsActivated && waterPillar.IsActivated)
        {
            CurrentQuestStatus = QuestStatus.RESOLVED;
        }
    }


    private void RefreshQuestState()
    {
        /*
            TODO: para simplificar el alcance del obligatorio, el jugador tiene disponible
            una sola mision para completar.

            - Queda pendiente el refactor y diseño de un mecanismo que permita la carga de
              multiples misiones y su asignación a NPCs (quest givers).

            - Estudiar la posibilidad de leer y parsear un archivo de recursos en formato json,
              con una estructura predefinida y mapearlo con una clase, para levantar las quests
              disponibles y los mensajes de según cada estado.
        */

        // Actualizamos el texto a mostrar en el panel de la quest.
        UpdateQuestPanel();

        // Actualizamos el texto a mostrar en el panel de objetivos.
        UpdateQuestTracker();
    }

    private void UpdateQuestPanel()
    {
        var questTextLines = new List<string>();
        if (CurrentQuestStatus == QuestStatus.PENDING)
        {
            questTextLines.Add("Welcome little one... you're just in time. Your arrival has been long anticipated, though that concept is meaningless to our kind.");
            questTextLines.Add("I've been watching over this place since ages, however something happened recently. My power is no longer what used to be. I can feel it slowly fading away.");
            questTextLines.Add("You must help this old stone, down there in the valley rest three ancient pillars... each one magically imbued with energy from an elemental stone... Look for them! I sense something is not right...");

        }
        else if (CurrentQuestStatus == QuestStatus.ACTIVE)
        {
            questTextLines.Add("Be brave little one... Face the most difficult step in your journey, as the current one is your only choice...");
        }
        else if (CurrentQuestStatus == QuestStatus.RESOLVED)
        {
            questTextLines.Add("Your spirit burns with life, young creature... You have done it well.");
            questTextLines.Add("The cycle comes to an end... My essence is now in place... and finally...");
            questTextLines.Add("...I can depart. I bid you well young one..");
        }

        var questDescription = questTextLines?.Count > 0
            ? string.Join(Environment.NewLine + Environment.NewLine, questTextLines)
            : string.Empty;
        questText.SetText(questDescription);

    }

    private void UpdateQuestTracker()
    {
        if (CurrentQuestStatus == QuestStatus.PENDING)
        {
            // Si la quest aun no ha sido aceptada.. no mostramos el tracker..
            StartCoroutine(FadePanel(questTrackerCanvasGroup, 1, 0));
            questTrackerPanel.SetActive(false);
        }
        else if (CurrentQuestStatus == QuestStatus.COMPLETE)
        {
            // La quest ya fue completada.. no mostramos el tracker..
            StartCoroutine(FadePanel(questTrackerCanvasGroup, 1, 0));
            questTrackerPanel.SetActive(false);
        }
        else
        {
            // Si la quest se encuentra activa o ya resuelta.. entonces el tracker debe
            // ser visible.. si además el panel no es visible aun (primera transición) de
            // estados, entonces activamos el tracker..
            if (!questTrackerPanel.activeInHierarchy)
            {
                questTrackerPanel.SetActive(true);
                StartCoroutine(FadePanel(questTrackerCanvasGroup, 0, 1));
            }
            // Creamos los textos para representar el estado de activación de cada pilar.
            var trackerTextLines = new List<string>()
            {
                $"- {(firePillar.IsActivated ? "1" : "0")}/1 Fire Pillar",
                $"- {(earthPillar.IsActivated ? "1" : "0")}/1 Earth Pillar",
                $"- {(waterPillar.IsActivated ? "1" : "0")}/1 Water Pillar"
            };

            var objectivesText = string.Join(Environment.NewLine + Environment.NewLine, trackerTextLines);
            questTrackerMessage.SetText(objectivesText);
        }
    }

    private IEnumerator FadePanel(CanvasGroup panel, float start, float end)
    {
        var counter = 0f;

        while (counter < fadeDuration)
        {
            counter += Time.deltaTime;
            panel.alpha = Mathf.Lerp(start, end, counter / fadeDuration);

            yield return null;
        }
    }
}
