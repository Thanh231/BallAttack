// using System.Collections;
// using TMPro;
// using UnityEngine;

// public class GamePlayManager : MonoBehaviour
// {
//     [SerializeField] private TextMeshProUGUI scoreText;
//     private bool isStartGame = false;
//     public TextMeshProUGUI levelUp;

//     public int currentScore;

//     private float score = 0;
//     private float timer = 0.8f;

//     void OnEnable()
//     {
//         EventManager.StartGame += StartGame;
//         EventManager.ResetGame += ResetGame;
//     }

//     void OnDisable()
//     {
//         EventManager.StartGame -= StartGame;
//         EventManager.ResetGame -= ResetGame;
//     }

//     private void ResetGame()
//     {
//         SetEndGame();
//         isStartGame = false;
//         score = 0;
//     }

//     private void StartGame()
//     {
//         isStartGame = true;
//     }

//     void Update()
//     {
//         if (!isStartGame) return;

//         // score += GameManager.ins.GetIncreasePointSpeed() * Time.deltaTime;

//         scoreText.text = score.ToString("00000");

//         // if (score > GameManager.ins.GetPivotScoreForNextLevel())
//         // {
//         //     GameManager.ins.currentLevel++;
//         //     SoundManager.instance.Play(SoundManager.instance.updateSound);

//         //     levelUp.gameObject.SetActive(true);
//         //     StartCoroutine(HideLevelUp());
//         // }
//     }
//     private void SetEndGame()
//     {
//         Debug.Log("score" + score);
//         GameManager.ins.ShowMenu((int)score);
//     }
//     private IEnumerator HideLevelUp()
//     {
//         yield return new WaitForSeconds(timer);
//         levelUp.gameObject.SetActive(false);
//     }
// }
