using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    public AudioClip collectSound;
    public AudioClip wallSound;
    private AudioSource audioSource;

    public GameObject splashPrefab;
    private float splashTimer = 0f;
    public float splashInterval = 0.3f;

    public GameObject deathParticle;

    void Start()
    {
        count = 0;
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        SetCountText();
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        if (movement.magnitude > 0)
        {
            splashTimer += Time.fixedDeltaTime;
            if (splashTimer >= splashInterval)
            {
                Quaternion splashRotation = Quaternion.LookRotation(-movement);
                Instantiate(splashPrefab, transform.position, splashRotation);
                splashTimer = 0f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            ParticleSystem particle = other.gameObject.GetComponentInChildren<ParticleSystem>();
            if (particle != null)
            {
                particle.transform.parent = null;
                particle.Play();
                Destroy(particle.gameObject, particle.main.duration);
            }
            audioSource.PlayOneShot(collectSound);
            count = count + 1;
            other.gameObject.SetActive(false);
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 8)
        {
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            winTextObject.SetActive(true);
            GameManager.Instance.PlayVictory();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            audioSource.PlayOneShot(wallSound);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<EnemyMovement>().StopSound();
            GameManager.Instance.PlayExplosion();
            GameManager.Instance.PlayGameOver();
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
            Destroy(gameObject);
        }
    }
}
