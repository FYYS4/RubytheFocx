//Thu Part//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SlimeGirlControl : MonoBehaviour
{
    public float displayTime = 4.0f;
    public AudioClip slimeGirlAudioClip;
    public GameObject dialogBox;
    public TextMeshProUGUI dialogText;
    private RubyController rubyController;
    float timerDisplay;
    
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
        rubyController = FindObjectOfType<RubyController>();
    }
    
    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }
    
    public void DisplayDialog()
    {

            if (rubyController.candiesCount >= 4)
            {
                // Display a different dialogue for Ruby with 4 candies
                dialogBox.SetActive(true);
                dialogText.text = "Thank you for the candies! You're so sweet! the slimes should be gone now";
            }
            else
            {
                // Display the original dialogue
                dialogBox.SetActive(true);
                dialogText.text = "Be aware of those slimes, they will slow you down but they're friendly so they will not cause any damage.  I can help you make them gone if you give me 4 candies";
            }
            timerDisplay = displayTime;
            PlaySlimeGirlAudio();
    
    }
    void PlaySlimeGirlAudio()
    {
        if (slimeGirlAudioClip != null)
        {
            AudioSource audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // Play the slime girl audio clip
            audioSource.PlayOneShot(slimeGirlAudioClip);
        }
    }
}
//End//