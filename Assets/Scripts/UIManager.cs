
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject tutorial;
    public GameObject menu;
    public Text currentScoreText;
    public Text highScoreText;
    public Text tutorialText;
    public Text scoreText;
    private bool isIncreasePoint = false;
    private int increaseDuration = 0;

    private int currentScore = 0;
    private int highScore = 0;
    private bool isEndGame = false;

    void OnEnable()
    {
        EventManager.StartGame += StartGame;
        EventManager.ShowMenu += ShowMenu;
        EventManager.IncreasePoint += IncresePoint;
        EventManager.ShowTutorial += ShowTutorial;
    }

    private void IncresePoint(int point)
    {
        currentScore += point;
        scoreText.text = currentScore.ToString("0000");
    }

    void OnDisable()
    {
        EventManager.StartGame -= StartGame;
        EventManager.ShowMenu -= ShowMenu;
        EventManager.IncreasePoint -= IncresePoint;
        EventManager.ShowTutorial -= ShowTutorial;
    }

    private void ShowTutorial(string text)
    {
        tutorialText.text = text;
        menu.SetActive(false);
        tutorial.SetActive(true);
    }
    public void SkipTutorial()
    {
        tutorial.SetActive(false);
        if (!GameManager.ins.isInit)
        {
            GameManager.ins.PlayGame();
        }
    }

    private void StartGame()
    {
        menu.SetActive(false);
        if(isEndGame)
        {
            currentScore = 0;
            increaseDuration = 0;
            isEndGame = false;
            scoreText.text = currentScore.ToString("0000");
        }
    }

    // public void SkipMenu()
    // {
    //     // tutorial.SetActive(true);
    //     menu.SetActive(false);

    // }

    private void Start()
    {
        highScoreText.text = "";
        currentScoreText.text = "";
        menu.SetActive(true);
    }

    private void ShowMenu()
    {
        isIncreasePoint = true;
        isEndGame = true;
        highScore = GameManager.ins.HighScore;
        highScoreText.text = "HIGH SCORE \n" + highScore.ToString("0000");
        menu.SetActive(true);
    }

    void Update()
    {
        if (isIncreasePoint)
        {
            increaseDuration++;
            if (increaseDuration >= currentScore)
            {
                isIncreasePoint = false;
            }
            if (currentScore > highScore)
            {
                currentScoreText.text = "NEW BEST \n" + increaseDuration.ToString("0000");
                GameManager.ins.HighScore = currentScore;
            }
            else
            {
                currentScoreText.text = "SCORE \n" + increaseDuration.ToString("0000");
            }
        }
    }

}
