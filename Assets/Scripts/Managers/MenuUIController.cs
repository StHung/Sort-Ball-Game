
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] Transform gridButton;
    [SerializeField] Button levelButtonPrefab;

    [SerializeField] Sprite lockIcon;
    [SerializeField] Sprite tickedIcon;

    private int amountLevel;
    private int currentLevel;


    private void Start()
    {
        amountLevel = SceneManager.sceneCountInBuildSettings - 1;
        currentLevel = DataManager.Instance.CurentLevel;

        for (int i = 1; i <= amountLevel; i++)
        {
            CreateButton(i);
        }
    }

    private void CreateButton(int index)
    {
        Button button = Instantiate(levelButtonPrefab, gridButton);
        int level = index;
        button.GetComponentInChildren<Text>().text = level.ToString();
        
        if( level < currentLevel + 1)
        {
            button.transform.Find("State Icon").GetComponent<Image>().sprite = tickedIcon;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => LoadLevel(level));
        }
        else if(level == currentLevel + 1)
        {
            button.transform.Find("State Icon").GetComponent<Image>().sprite = null;
            button.transform.Find("State Icon").GetComponent<Image>().color = new Color(0f,0f,0f,0f);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => LoadLevel(level));
        }
        else
        {
            button.transform.Find("State Icon").GetComponent<Image>().sprite = lockIcon;
        }
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene($"Level_{level}");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
