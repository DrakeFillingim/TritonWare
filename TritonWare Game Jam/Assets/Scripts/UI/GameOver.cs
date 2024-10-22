using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private PlayerStats _stats;
    public GameObject gameOver;
    public Button restartButton;
    private GameObject music1;
    private GameObject music2;
    private GameObject music3;
    private GameObject music4;

    private bool _setTime = true;

    void Start()
    {
        _stats = GameObject.Find("Player").GetComponent<PlayerStats>();
        restartButton.onClick.AddListener(OnRestartButtonPressed);
        music1 = GameObject.Find("PhaseOneMusic");
        music2 = GameObject.Find("PhaseTwoMusic");
        music3 = GameObject.Find("TritonDefeated");
        music4 = GameObject.Find("PlayerDefeated");
    }

    void Update()
    {
        if (_stats.CurrentHealth <= 0)
        {
            
            gameOver.SetActive(true);
            music1.GetComponent<FMODUnity.StudioEventEmitter>().Stop();
            music2.GetComponent<FMODUnity.StudioEventEmitter>().Stop();
            music3.GetComponent<FMODUnity.StudioEventEmitter>().Stop();
            music4.GetComponent<FMODUnity.StudioEventEmitter>().Play();
            restartButton.gameObject.SetActive(true);
            if (_setTime)
            {
                Time.timeScale = 0;
                _setTime = false;
            }
        }
    }

    public void OnRestartButtonPressed()
    {
        Time.timeScale = 1;
        music4.GetComponent<FMODUnity.StudioEventEmitter>().Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
