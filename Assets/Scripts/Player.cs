using UnityEngine;

public class Player : MonoBehaviour
{

    // public GameObject knifePrefabs;
    public InputPlayer input;
    // private bool isStartGame = false;
    private Vector3 defaultPos = new Vector3(0, 1.6f, 0);
    public Rigidbody rb;
    void OnEnable()
    {
        EventManager.StartGame += StartGame;
        EventManager.ResetGame += ResetGame;
    }
    void OnDisable()
    {
        EventManager.StartGame -= StartGame;
        EventManager.ResetGame -= ResetGame;
    }

    private void ResetGame()
    {
        transform.position = defaultPos;
        transform.rotation = Quaternion.identity;
    }

    private void StartGame()
    {
        // isStartGame = true;
        // rb = GetComponent<Rigidbody>();

    }

    public void DisablePlayerObject()
    {
        // isStartGame = false;
    }

    private void Update()
    {
        // if (isStartGame)
        // {
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         Instantiate(knifePrefabs, transform.position, Quaternion.identity);
        //     }
        // }

    }
    void FixedUpdate()
    {
        Vector3 dir = new Vector3(input.GetDirection().x,0,input.GetDirection().y);
        dir = dir.normalized;
        // dir.y = 0;
        // rb.velocity = dir * 1000 * Time.deltaTime;
        rb.AddForce(dir * 20, ForceMode.Impulse);
    }
}
