using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    [SerializeField] private CrosshairController crosshairCtrl;
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private GolemController golemCtrl;

    private bool isPaused = false;
    void Update()
    {
        // Si apretamos la tecla escape y no estamos en modo dialogo, entonces
        // abrimos / cerramos el menu dependiendo del estado actual.
        if (Input.GetKeyDown(KeyCode.Escape) && !golemCtrl.IsTalking)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        crosshairCtrl.ResetState();
        crosshairCtrl.SetCursorLockState(CursorLockMode.Locked);
        gameMenu.SetActive(false);
        isPaused = false;
    }

    public void PauseGame()
    {
        crosshairCtrl.HideCrosshair();
        crosshairCtrl.SetCursorLockState(CursorLockMode.Confined);
        gameMenu.SetActive(true);
        isPaused = true;
    }

    public void BackToMenuClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
