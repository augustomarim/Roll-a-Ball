using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public AudioClip victoryMusic;
    public AudioClip gameOverMusic;
    public AudioClip explosionSound;

    private AudioSource audioSource;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayVictory()
    {
        audioSource.loop = false;
        audioSource.clip = victoryMusic;
        audioSource.Play();
    }
    public void PlayExplosion()
    {
        audioSource.PlayOneShot(explosionSound);
    }

    public void PlayGameOver()
    {
        audioSource.loop = false;
        audioSource.clip = gameOverMusic;
        Invoke("StartGameOver", 3f);
    }

    void StartGameOver()
    {
        audioSource.Play();
    }

    void Update()
    {

    }
}