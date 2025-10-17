using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("Scene to Load")]
    
    public int mainGameSceneIndex = 1;

    [Header("Audio Settings")]
    
    public AudioClip backgroundMusic;
    
    public AudioSource musicAudioSource;
    
    public AudioClip lightningSound;
    
    public AudioSource sfxAudioSource;

    [Header("Sound Effect Timing")]
    
    public float minSoundInterval = 5f;
   
    public float maxSoundInterval = 15f;


    void Start()
    {
        
        if (musicAudioSource != null && backgroundMusic != null)
        {
            musicAudioSource.clip = backgroundMusic;
            musicAudioSource.loop = true;
            musicAudioSource.Play();
        }

        
        if (sfxAudioSource != null && lightningSound != null)
        {
            StartCoroutine(PlayRandomSound());
        }
    }

    
    public void PlayGame()
    {
        
        SceneManager.LoadScene(mainGameSceneIndex);
    }

    public void QuitGame()
    {
       
        Debug.Log("QUIT GAME");
        Application.Quit();
    }

    

    private IEnumerator PlayRandomSound()
    {
        
        while (true)
        {
            
            float waitTime = Random.Range(minSoundInterval, maxSoundInterval);
            yield return new WaitForSeconds(waitTime);

            
            sfxAudioSource.PlayOneShot(lightningSound);
        }
    }
}