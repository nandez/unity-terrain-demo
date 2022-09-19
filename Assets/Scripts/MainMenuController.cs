using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("HUD References")]
    [SerializeField] private Animator transition;
    [SerializeField] private float transitionTime = 1f;

    [Header("Pillar Settings")]
    [SerializeField] private GameObject pillarInnerStone;
    [SerializeField] private float rotationDegreesPerSecond = 30f;
    [SerializeField] private float stoneBounceFrequency = 1f;
    [SerializeField] private float stoneBounceAmplitude = 0.025f;

    private Vector3 stoneInitialPosition;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        stoneInitialPosition = pillarInnerStone.transform.position;
    }
    private void Update()
    {
        // Rotación de la piedra
        pillarInnerStone.transform.Rotate(new Vector3(0f, Time.deltaTime * rotationDegreesPerSecond, 0f), Space.World);

        // Oscilación de la piedra
        var position = stoneInitialPosition;
        position.y += Mathf.Sin(Time.fixedTime * Mathf.PI * stoneBounceFrequency) * stoneBounceAmplitude;
        pillarInnerStone.transform.position = position;
    }
    public void OnPlayButtonClick()
    {
        StartCoroutine(nameof(LoadGameScene));
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
    }

    private IEnumerator LoadGameScene()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("GameScene");
    }
}
