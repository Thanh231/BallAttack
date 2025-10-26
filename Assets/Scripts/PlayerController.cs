using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InputPlayer input;
    public float maxSpeed = 10f;
    public Rigidbody rb;
    public GameObject model;
    private bool isKnockbackActive = false;
    private float knockbackDuration = 1f;
    private bool isStartGame = false;
    private bool isReset = false;
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

    private void StartGame()
    {
        isStartGame = true;
        isReset = false;
    }

    private void ResetGame()
    {
        isStartGame = false;
    }

    void Update()
    {
        if (rb.velocity.magnitude > 0.01f)
        {
            Vector3 moveDir = rb.velocity.normalized;
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, moveDir).normalized;
            float rotationAngle = rb.velocity.magnitude * 30f * Time.deltaTime * Mathf.Rad2Deg / model.transform.localScale.x;
            model.transform.Rotate(rotationAxis, rotationAngle, Space.World);
        }
    }

    void FixedUpdate()
    {
        if (!isKnockbackActive && isStartGame)
        {
            Vector2 moveInput = input.GetDirection();
            Vector3 moveDir = new Vector3(moveInput.x, 0f, moveInput.y).normalized;
            Vector3 targetVelocity = new Vector3(moveDir.x * maxSpeed, rb.velocity.y, moveDir.z * maxSpeed);
            rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, Time.fixedDeltaTime * 10f);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Rigidbody otherRb = col.gameObject.GetComponent<Rigidbody>();
            if (otherRb == null) return;

            Vector3 normal = (transform.position - col.transform.position).normalized;
            Vector3 v1 = rb.velocity;
            Vector3 v2 = otherRb.velocity;

            float m1 = rb.mass;
            float m2 = otherRb.mass;

            float v1n = Vector3.Dot(v1, normal);
            float v2n = Vector3.Dot(v2, normal);
            float v1nAfter = ((m1 - m2) * v1n + 2f * m2 * v2n) / (m1 + m2);
            float v2nAfter = ((m2 - m1) * v2n + 2f * m1 * v1n) / (m1 + m2);

            Vector3 v1t = v1 - v1n * normal;
            Vector3 v2t = v2 - v2n * normal;

            rb.AddForce(v1t + v1nAfter * normal * -50);
            otherRb.AddForce(v2t + v2nAfter * normal * -50);

            if (!isKnockbackActive)
            {
                isKnockbackActive = true;
                StartCoroutine(DisableControlFor(knockbackDuration));
            }
            var mass = otherRb.mass;
            GameManager.ins.IncresePoint((int)mass * 2);
            AudioController.ins.PlaySound(AudioController.ins.hitSfx);
        }
        if (col.gameObject.CompareTag("Plane") && isReset == false)
        {
            // GameManager.ins.PlayGame();
            isReset = true;
            isStartGame = true;
        }

    }

    IEnumerator DisableControlFor(float duration)
    {
        yield return new WaitForSeconds(duration);
        isKnockbackActive = false;
    }
}