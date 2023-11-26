using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    private int fixedRobotsCount = 0;
    public int maxHealth = 5;
    public int score = 0; // Variable to track the score
    public TextMeshProUGUI fixedRobotsCountText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;
    bool isGameOver;
    public int totalRobotsCount = 4;
    public GameObject projectilePrefab;
    public ParticleSystem hitVFX;
    public ParticleSystem HealthVFX;
    public AudioClip throwSound;
    public AudioClip hitSound;
   
    public int health { get { return currentHealth; }}
    int currentHealth;
   
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
   
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
   
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
   
    AudioSource audioSource;
   
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
       
        currentHealth = maxHealth;




        audioSource = GetComponent<AudioSource>();
    }




    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
       
        Vector2 move = new Vector2(horizontal, vertical);
       
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
       
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
       
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
       
        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
       
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
        if (fixedRobotsCount >= totalRobotsCount)
        {
            WinGame();
        }

        if (currentHealth <= 0 && !isGameOver)
        {
            LoseGame();
        }
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
        }
    }
    void WinGame()
    {
        isGameOver = true;
        winText.gameObject.SetActive(true);
        winText.text = "You Win! Game Created by Group 25";
        // Optionally, you may want to perform other actions when the player wins.
    }
    void LoseGame()
    {
        isGameOver = true;
        loseText.gameObject.SetActive(true);
        loseText.text = "You lost! Press R to restart";
        // Disable player movement
        speed = 0f;

        // Optionally, you may want to perform other actions when the player loses.
    }
    void RestartGame()
    {
        // Reset relevant variables
        currentHealth = maxHealth;
        isGameOver = false;
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
        speed = 3.0f;

        // Optionally, you may want to reset other game state variables.

        // Allow player movement again
        speed = 3.0f;

        // Restart the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;




        rigidbody2d.MovePosition(position);
    }




    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;
           
            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
            GameObject hitVFXObject = Instantiate(hitVFX.gameObject,  rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            PlaySound(hitSound);
            if (hitVFX !=null)
            {
                hitVFX.Play();
            }
        }
        else if (amount > 0)
        {
            // Instantiate health particle effect
            GameObject HealthVFXObject = Instantiate(HealthVFX.gameObject, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            if (HealthVFX != null)
            {
                // Parent the particle effect to the character to make it follow
                HealthVFXObject.transform.parent = transform;


                ParticleSystem particleSystem = HealthVFXObject.GetComponent<ParticleSystem>();
                if (particleSystem != null)
                {
                    particleSystem.Play();
                }
            }
        }
       
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
       
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }
   
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
       
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);




        animator.SetTrigger("Launch");
       
        PlaySound(throwSound);
    }
   
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    public void ChangeFixedRobotsCount(int count)
    {
        fixedRobotsCount += count; // Increment the count of fixed robots
        UpdateFixedRobotsText(); // Update the UI Text element
    }
    void UpdateFixedRobotsText()
    {
        // Assuming you have a TextMeshProUGUI variable for displaying fixed robots count
        if (fixedRobotsCountText != null)
        {
            fixedRobotsCountText.text = "Fixed Robots: " + fixedRobotsCount.ToString();
        }
    }
}

