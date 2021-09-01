using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float _limitingForce = 50f;
    private float _limitingHeight = 3.5f;
    private float gravityModifier = 1.5f;
    private Rigidbody _playerRb;
    private float _height;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource _playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip bounceSound;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        _playerAudio = GetComponent<AudioSource>();
        _playerRb = GetComponent<Rigidbody>();
        _height = GameObject.Find("Background").GetComponent<BoxCollider>().size.y;

        // Apply a small upward force at the start of the game
        _playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        bool boundaryPassed = transform.position.y > _height - _limitingHeight;
        if (boundaryPassed)
        {
            _playerRb.AddForce(Vector3.down * _limitingForce, ForceMode.Force);
            return;
        }

        // While space is pressed and player is low enough, float up
        if (Input.GetKeyDown(KeyCode.Space) && !gameOver)
        {
            _playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            _playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        }

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            _playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Ground") && !gameOver)
        {
            _playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
            _playerAudio.PlayOneShot(bounceSound, 1.0f);
        }
    }
}