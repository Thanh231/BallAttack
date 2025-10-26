using System.Collections.Generic;
using UnityEngine;

public class GameStage : MonoBehaviour
{
    private bool isStartGame = false;
    public GameObject knifePrefabs;
    public Stack<GameObject> knifes = new Stack<GameObject>();

    public List<GameObject> obstacle;
    public int numbeOfKnife = 10;
    public Transform parent;

    void Awake()
    {
        for (int i = 0; i < numbeOfKnife; i++)
        {
            GameObject knife = Instantiate(knifePrefabs, new Vector3(0, -10, 0), Quaternion.identity);
            SetDefault(knife);
        }
    }

    private void SetDefault(GameObject knife)
    {
        knifes.Push(knife);
        knife.transform.parent = parent;
        knife.GetComponent<Knife>().SetEnable(false);
        knife.transform.position = new Vector3(0, -10, 0);
        knife.transform.rotation = Quaternion.identity;
    }

    void OnEnable()
    {
        EventManager.StartGame += StartGame;
        EventManager.ResetGame += EndGame;
        EventManager.ReturnObjectToPool += ReturnKnife;
    }
    void OnDisable()
    {
        EventManager.StartGame -= StartGame;
        EventManager.ResetGame -= EndGame;
        EventManager.ReturnObjectToPool -= ReturnKnife;
    }

    private void ReturnKnife(GameObject knife)
    {
        SetDefault(knife);
    }

    private void EndGame()
    {
        isStartGame = false;
    }

    private void StartGame()
    {
        isStartGame = true;
    }

    void Update()
    {
        if (isStartGame)
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject knife = knifes.Pop();
                knife.transform.position = transform.position;
                transform.rotation = Quaternion.identity;
                knife.transform.parent = null;

                Knife knifeScript = knife.GetComponent<Knife>();
                knifeScript.SetEnable(true);
                knifeScript.ReleaseKnife();

            }
        }
    }


}
