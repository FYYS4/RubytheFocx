using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public float speed;         // Speed of the slime      
    public float changeTime = 3.0f;  // Time interval to change direction
    private RubyController rubyController;
    private AudioSource audioSource;  // Add an AudioSource component
    public AudioClip slimeSound;
    Animator animator;
    Rigidbody2D rigidbody2D;
    float timer;
    int direction = 1;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();  
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
       MoveSlime();
    }
    void MoveSlime()
    {
        Vector2 position = rigidbody2D.position;
        position.x = position.x + Time.deltaTime * speed * direction;
        animator.SetFloat("Move X", direction);
        rigidbody2D.MovePosition(position);
    }
     void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController >();


        if (player != null)
        {
            // Assign the reference to the rubyController
            rubyController = player;

            // Check if rubyController is not null before using it
            if (rubyController != null)
            rubyController.DecreaseSpeed(2, 5.0f);
            PlaySlimeAudio();
            AudioSource audioSource = GetComponent<AudioSource>();
        }

    }
     void PlaySlimeAudio()
    {
        if (slimeSound != null)
        {
            AudioSource audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // Play the slime girl audio clip
            audioSource.PlayOneShot(slimeSound);
        }
    }
}



