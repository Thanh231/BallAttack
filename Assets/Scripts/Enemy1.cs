
using System.Collections;
using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    // public InputPlayer input;
    public float maxSpeed;
    public float currentSpeed;
    public Rigidbody rb;
    public GameObject model;

    private bool isKnockBack = false;
    private bool isStun = false;
    private Vector3 dir;


    void Start()
    {
        // input.GetDirection();
    }

    void Update()
    {
        if (isStun) return;
        Vector3 moveInput = GameManager.ins.player.transform.position - transform.position;
        moveInput.y = 0;

        // Debug.Log(moveInput);

        if (!isKnockBack)
        {
            dir = moveInput.normalized;
        }


        // Debug.Log("dir" + dir);

        // rb.AddForce(dir * currentSpeed, ForceMode.Acceleration);

        if (dir.magnitude > 0.001f)
        {

            currentSpeed = Mathf.Clamp(currentSpeed + Time.deltaTime * 100, 0, maxSpeed);
        }
        else
        {
            currentSpeed = 0;
        }
        // rb.AddForce(dir * currentSpeed, ForceMode.Acceleration);

        if (rb.velocity.magnitude > 0.001f)
        {
            Vector3 moveDir = rb.velocity.normalized;
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, moveDir).normalized;
            float rotationAngle = rb.velocity.magnitude * 3 * Time.fixedDeltaTime * Mathf.Rad2Deg / model.transform.localScale.x;
            model.transform.Rotate(rotationAxis, rotationAngle, Space.World);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isKnockBack = true;
            StartCoroutine(StopKnockBack());
            Vector3 pos = new Vector3(collision.transform.position.x, transform.position.y, collision.transform.position.z);
            dir = transform.position - pos;
            currentSpeed = 20;
        }
    }

    IEnumerator StopKnockBack()
    {
        yield return new WaitForSeconds(0.1f);
        currentSpeed = 0;
        dir = Vector3.zero;
        isKnockBack = false;
        // rb.velocity = Vector3.zero;
        StartCoroutine(StopStun());
        isStun = true;
    }

    IEnumerator StopStun()
    {
        yield return new WaitForSeconds(5f);
        // currentSpeed = 0;
        // dir = Vector3.zero;
        // isKnockBack = false;
        // rb.velocity = Vector3.zero;
        isStun = false;
    }

}
