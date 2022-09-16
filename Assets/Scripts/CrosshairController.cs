using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    [Header("Crosshair Settings")]
    [SerializeField] private GameObject normalCrosshair;
    [SerializeField] private GameObject interactiveCrosshair;

    [Header("Interactive Tooltip Settings")]
    [SerializeField] private CanvasGroup interactionTooltip;
    [SerializeField] private TMPro.TMP_Text actionKey;
    [SerializeField] private TMPro.TMP_Text actionLabel;


    private void Start()
    {
        // Bloqueamos el cursor para evitar hacer click fuera de la pantalla.
        SetCursorLockState(CursorLockMode.Locked);
    }

    public void SetInteractiveState(string key, string text)
    {
        this.actionKey.SetText(key);
        this.actionLabel.SetText(text);
        this.interactionTooltip.alpha = 1;
        this.interactiveCrosshair.SetActive(true);
        this.normalCrosshair.SetActive(false);
    }

    public void ResetState()
    {
        this.actionKey.SetText(string.Empty);
        this.actionLabel.SetText(string.Empty);
        this.interactionTooltip.alpha = 0;
        this.interactiveCrosshair.SetActive(false);
        this.normalCrosshair.SetActive(true);
    }

    public void HideCrosshair()
    {
        ResetState();
        this.normalCrosshair.SetActive(false);
    }

    public void SetCursorLockState(CursorLockMode mode)
    {
        Cursor.lockState = mode;
    }
}
