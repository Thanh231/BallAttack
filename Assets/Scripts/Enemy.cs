
using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxSpeed;
    public float currentSpeed;
    public Rigidbody rb;
    public GameObject model;

    private bool isKnockBack = false;
    private Vector3 dir;
    public float knockbackDuration;
    private bool isStartGame = false;

    void OnEnable()
    {
        EventManager.StartGame += StartGame;
        EventManager.ResetGame += EndGame;
    }

    void OnDisable()
    {
        EventManager.StartGame -= StartGame;
        EventManager.ResetGame -= EndGame;
    }

    private void EndGame()
    {
        isStartGame = false;
    }

    private void StartGame()
    {
        isStartGame = true;
        isKnockBack = false;
    }

    void Update()
    {
        if (rb.linearVelocity.magnitude > 0.001f)
        {
            Vector3 moveDir = rb.linearVelocity.normalized;
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, moveDir).normalized;
            float rotationAngle = rb.linearVelocity.magnitude * 3 * Time.fixedDeltaTime * Mathf.Rad2Deg / model.transform.localScale.x;
            model.transform.Rotate(rotationAxis, rotationAngle, Space.World);
        }
    }
    void FixedUpdate()
    {
        if (!isKnockBack && isStartGame)
        {
            Vector3 moveInput = GameManager.ins.player.transform.position - transform.position;
            moveInput.y = 0;
            dir = moveInput.normalized;
            
            Vector3 targetVelocity = new Vector3(dir.x * maxSpeed, rb.linearVelocity.y, dir.z * maxSpeed);
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, Time.fixedDeltaTime * 10f);
        }
    }
    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isKnockBack)
            {
                isKnockBack = true;
                StartCoroutine(DisableControlFor(knockbackDuration));
            }
        }
    }

    public void SetScale(Vector3 scale)
    {
        model.transform.localScale = scale;
    }

    IEnumerator DisableControlFor(float duration)
    {
        yield return new WaitForSeconds(duration);
        isKnockBack = false;
    }
}
