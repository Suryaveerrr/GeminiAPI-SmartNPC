using System;
using UnityEngine;


public class AIManager : MonoBehaviour
{
    public static AIManager Instance { get; private set; }

    
    // This new event will send BOTH the text and the audio clip at the same time.
    public static event Action<string, AudioClip> OnDialogueReady;

    
    // public static event Action<string> OnDialogueReceived;
    

    
    // Variables to store the data between API calls
    private string storedTextResponse;
    private string storedVoiceName;
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

   
    public void AskQuestion(string persona, string question, string voiceName)
    {
        
        this.storedVoiceName = voiceName;
       
        
        string prompt = $"{persona}\n\nPlayer: \"{question}\"\n\nCharacter:";
        Debug.Log("Sending text prompt to Gemini: " + prompt);

        
        StartCoroutine(GeminiAPI_Client.Instance.GenerateContent(prompt, HandleTextResponse));
    }

   
    private void HandleTextResponse(string response)
    {
        if (!string.IsNullOrEmpty(response))
        {
            Debug.Log("Gemini Response: " + response);

            
            
            this.storedTextResponse = response;

            
            StartCoroutine(GeminiAPI_Client.Instance.GenerateSpeech(response, this.storedVoiceName, HandleSpeechResponse));
            
        }
        else
        {
            Debug.LogError("Failed to get a response from Gemini.");
            
            
            OnDialogueReady?.Invoke("...I'm not sure what to say.", null);
            
        }
    }

    
    
    private void HandleSpeechResponse(AudioClip clip)
    {
        
        OnDialogueReady?.Invoke(this.storedTextResponse, clip);
    }
    


    
}
