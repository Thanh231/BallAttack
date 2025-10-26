using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float timer = 0;
    private float dir = 1;
    private bool isStartGame = false;
    public List<int> speedArray;
    public int currentSpeed;
    private Animator anim;
    private float timeChangeBehavior;
    public void OnEnable()
    {
        anim = GetComponent<Animator>();
        EventManager.StartGame += StartGame;
        EventManager.ResetGame += ResetGame;
    }


    private void StartGame()
    {
        isStartGame = true;
        timeChangeBehavior = UnityEngine.Random.Range(1, 2);
        int index = Mathf.FloorToInt(UnityEngine.Random.Range(0, speedArray.Count - 1));
        currentSpeed = speedArray[index];
    }

    void OnDisable()
    {
        EventManager.StartGame -= StartGame;
        EventManager.ResetGame -= ResetGame;
    }

    private void ResetGame()
    {
        timer = 0;
        dir = (UnityEngine.Random.Range(0, 2) == 0) ? 1f : -1f;
        isStartGame = false;
    }

    void Update()
    {
        if (isStartGame)
        {
            timer += Time.deltaTime;
            if (timer > timeChangeBehavior)
            {
                timer = 0f;
                dir = (UnityEngine.Random.Range(0, 2) == 0) ? 1f : -1f;
                timeChangeBehavior = UnityEngine.Random.Range(2, 3);
                int index = Mathf.FloorToInt(UnityEngine.Random.Range(0, speedArray.Count));
                currentSpeed = speedArray[index];
            }
            transform.Rotate(0, 0, dir * currentSpeed * Time.deltaTime);
        }

    }

    public void ObstacleOnHit()
    {
        anim.SetTrigger("IsHit");
    }

}
