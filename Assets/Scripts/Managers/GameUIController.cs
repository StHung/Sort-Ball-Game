using TigerForge;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameUIController : MonoBehaviour, IGameOver
{
    [SerializeField] GameObject gameOverDiaLog;
    [SerializeField] Text levelText;

    private int currentLevel;

    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        currentLevel = int.Parse(sceneName.Substring(sceneName.IndexOf("_")+1));
        levelText.text = "Level " + currentLevel;
    }

    private void OnEnable()
    {
        EventManager.StartListening(EventName.On_Game_Over, OnGameOver);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventName.On_Game_Over, OnGameOver);
    }

    public void OnGameOver()
    {
        gameOverDiaLog.SetActive(true);
        if(currentLevel > DataManager.Instance.CurentLevel)
        {
            DataManager.Instance.CurentLevel = currentLevel;
        }
    }

    public void ReLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadNextLevel()
    {
        int nextLevel = currentLevel + 1;
        if (nextLevel <= SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene($"Level_{nextLevel}");
        }
        else
        {
            BackToMenu();
        }
    }
}
