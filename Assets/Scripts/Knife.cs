using UnityEngine;

public class Knife : MonoBehaviour
{
    // private BoxCollider2D knifeCollider;
    private bool isMoving = false;
    private bool isCollide = false;
    private bool isActive = true;
    public SpriteRenderer knifeSprite;

    void OnEnable()
    {
        EventManager.CleanObject += CleanObject;
    }

    void OnDisable()
    {
        EventManager.CleanObject -= CleanObject;
    }

    private void CleanObject()
    {
        SetDefaulGameObject();
    }

    public void ReleaseKnife()
    {
        isMoving = true;
    }

    public void SetEnable(bool state)
    {
        knifeSprite.enabled = state;
        GetComponent<BoxCollider2D>().enabled = state;
    }

    private void SetDefaulGameObject()
    {
        isActive = true;
        isMoving = false;
        isCollide = false;
        GameManager.ins.ReturnKnifeToPool(this.gameObject);
    }

    void Update()
    {
        if (isActive)
        {
            if (isMoving)
            {
                transform.position += Vector3.up * 20f * Time.deltaTime;
            }
            if (isCollide)
            {
                transform.position -= Vector3.up * 30f * Time.deltaTime;
                transform.Rotate(0, 0, 2000 * Time.deltaTime);
            }
        }

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle" && !isCollide)
        {
            isMoving = false;
            this.transform.parent = collision.transform;
            collision.gameObject.GetComponent<Obstacle>().ObstacleOnHit();
            isActive = false;
            GameManager.ins.IncresePoint(10);
        }
        else if (collision.gameObject.tag == "Knife")
        {
            isMoving = false;
            isCollide = true;
            EndGame();
        }
    }

    private void EndGame()
    {
        GameManager.ins.EndGame();
    }
}
