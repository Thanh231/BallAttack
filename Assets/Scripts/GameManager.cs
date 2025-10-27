using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager ins;

    private string highScore;

    // public int currentStage = 0;
    public GameObject player;
    public bool isInit = true;

    private Dictionary<GameObject, bool> enemyMap = new Dictionary<GameObject, bool>();

    public GameObject[] enemyPrefabs;

    public Vector3[] enemySpawnPos;
    public Vector3 playerPos;

    public int stageNumber = 1;
    private int numberEnemy = 0;
    private int rangeCollider = 0;
    private bool isEndGame = false;
    public GameObject tempAudio;
    public GameObject joyStick;

    private void Awake()
    {
        if (ins == null)
        {
            ins = this;
        }
        else
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            enemyMap.Add(enemyPrefabs[i], false);
        }
    }

    public int HighScore
    {
        get
        {
            return PlayerPrefs.GetInt(highScore, 0);
        }
        set
        {
            PlayerPrefs.SetInt(highScore, value);
        }
    }

    public void PlayGame()
    {

        SpawnEnemy();
        if (!player.gameObject.activeInHierarchy)
        {
            SpawPlayer();
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                joyStick.SetActive(true);
            }
        }
        if (!isInit)
        {
            EventManager.StartGame?.Invoke();
            isEndGame = false;
        }
        else
        {
            string text = "";
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                text = "Beweeg je bal met de joystick en Stoot andere ballen van het speelveld af ";
            }
            else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
            {
                text = "Beweeg je bal met WASD en Stoot andere ballen van het speelveld af";
            }
            EventManager.ShowTutorial?.Invoke(text);
        }
    }

    private void SpawPlayer()
    {
        player.transform.position = playerPos;
        player.gameObject.SetActive(true);
        tempAudio.SetActive(false);
    }

    private void SpawnEnemy()
    {
        GetNumberEnemyInStage();
        int i = 0;
        int count = 0;
        while (true)
        {
            var indexBall = Random.Range(0, enemyPrefabs.Length);
            var indexPos = Random.Range(0, enemySpawnPos.Length);
            Vector3 pos = enemySpawnPos[indexPos];
            GameObject enemy = enemyPrefabs[indexBall];
            bool value;
            if (enemyMap.TryGetValue(enemy, out value) && !value)
            {
                enemy.transform.position = pos;
                enemyMap[enemy] = true;
                float mass = GetEnemyMass();
                SphereCollider collider = enemy.GetComponent<SphereCollider>();
                Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                enemyScript.SetScale(new Vector3(rangeCollider, rangeCollider, rangeCollider));

                enemyRb.mass = mass;
                collider.radius = rangeCollider;
                enemy.SetActive(true);

                i++;
                if (i == numberEnemy)
                {
                    break;
                }
            }
            count++;
            if (count == 100)
                break;
        }
    }

    public void EndGame()
    {
        player.SetActive(false);
        tempAudio.SetActive(true);
        joyStick.SetActive(false);
        isEndGame = true;
        foreach (var item in enemyPrefabs)
        {
            item.SetActive(false);
        }
        stageNumber = 0;
        EventManager.ResetGame?.Invoke();
        StartCoroutine(ShowMenu());
    }
    private IEnumerator ShowMenu()
    {
        yield return new WaitForSeconds(2f);
        EventManager.ShowMenu?.Invoke();
        EventManager.CleanObject?.Invoke();
    }

    public void ReturnKnifeToPool(GameObject knife)
    {
        EventManager.ReturnObjectToPool?.Invoke(knife);
    }

    public void IncresePoint(int point)
    {
        EventManager.IncreasePoint?.Invoke(point);
    }

    private void GetNumberEnemyInStage()
    {

        if (stageNumber <= 10)
        {
            if (isInit)
            {
                numberEnemy = 1;
            }
            else
            {
                numberEnemy = Random.Range(1, enemyPrefabs.Length - 1);
            }
        }
        else if (stageNumber <= 20)
        {
            numberEnemy = Random.Range(2, enemyPrefabs.Length + 1);
        }
        else
        {
            numberEnemy = enemyPrefabs.Length;
        }
    }

    private float GetEnemyMass()
    {
        float roll = Random.value;
        float mass = 0;

        float chance1, chance2, chance3;

        if (stageNumber <= 10)
        {
            chance1 = 0.7f;
            chance2 = 0.25f;
            chance3 = 0.05f;
        }
        else if (stageNumber <= 20)
        {
            chance1 = 0.4f;
            chance2 = 0.4f;
            chance3 = 0.2f;
        }
        else
        {
            chance1 = 0.2f;
            chance2 = 0.4f;
            chance3 = 0.4f;
        }

        if (roll < chance1)
        {
            mass = 5;
            rangeCollider = 8;
        }
        else if (roll < chance1 + chance2)
        {
            mass = 10;
            rangeCollider = 10;
        }
        else
        {
            mass = 15;
            // I don't want my editor warning this variable is not used;
            chance3 = chance3 + 0;
            rangeCollider = 12;
        }

        return mass;
    }
    void OnTriggerEnter(Collider other)
    {
        if (isInit)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                isInit = false;
                EventManager.ShowTutorial?.Invoke("Nu, laten we vechten!!!");

                // Add point
                if (enemyMap.ContainsKey(other.gameObject))
                {
                    enemyMap[other.gameObject] = false;
                    other.gameObject.SetActive(false);
                }
                IncresePoint((int)other.GetComponent<Rigidbody>().mass * 5);
                AudioController.ins.PlaySound(AudioController.ins.aiDeath);
            }
            else if (other.gameObject.CompareTag("Player"))
            {
                SpawPlayer();
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                // Add point
                if (enemyMap.ContainsKey(other.gameObject))
                {
                    enemyMap[other.gameObject] = false;
                    other.gameObject.SetActive(false);
                    numberEnemy--;
                    if (numberEnemy == 0)
                    {
                        PlayGame();
                        if (Random.value > 0.5f)
                        {
                            stageNumber++;
                        }
                    }
                }
                AudioController.ins.PlaySound(AudioController.ins.aiDeath);
            }
            else if (other.gameObject.CompareTag("Player"))
            {
                if(!isEndGame)
                {  
                    AudioController.ins.PlaySound(AudioController.ins.aiDeath);
                    EndGame();
                }
            }
        }
    }

}
