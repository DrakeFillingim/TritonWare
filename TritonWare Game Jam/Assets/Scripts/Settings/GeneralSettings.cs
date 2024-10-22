using UnityEngine;
using UnityEngine.InputSystem;

public class GeneralSettings : MonoBehaviour
{
    private InputActionMap _inputMap;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 144;
    }

    private void OnEnable()
    {
        _inputMap = GetComponent<PlayerInput>().actions.FindActionMap("Player");
        _inputMap["Quit"].performed += OnQuit;
    }

    private void OnDisable()
    {
        _inputMap["Quit"].performed -= OnQuit;
    }

    private void OnQuit(InputAction.CallbackContext context)
    {
        print("here");
        Application.Quit();
    }
}
