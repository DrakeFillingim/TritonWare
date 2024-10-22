using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private PlayerStats _stats;
    public GameObject gameOver;
    public Button restartButton;

    private bool _setTime = true;

    void Start()
    {
        _stats = GameObject.Find("Player").GetComponent<PlayerStats>();
        restartButton.onClick.AddListener(OnRestartButtonPressed);
    }

    void Update()
    {
        if (_stats.CurrentHealth <= 0)
        {
            gameOver.SetActive(true);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
