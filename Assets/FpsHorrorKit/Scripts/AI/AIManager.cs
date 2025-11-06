using System;
using UnityEngine;

/// <summary>
/// This is the central brain of the AI system. It is a Singleton.
/// It takes requests from the UI (DialogueUI_Manager) and uses the API Client (GeminiAPI_Client)
/// to get responses from the Google Gemini API.
/// </summary>
public class AIManager : MonoBehaviour
{
    public static AIManager Instance { get; private set; }

    
    // This new event will send BOTH the text and the audio clip at the same time.
    public static event Action<string, AudioClip> OnDialogueReady;

    // We remove the old event
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
        // 1. Store the voice name for the next step
        this.storedVoiceName = voiceName;
        // --- END MODIFIED ---

        // 2. Construct the final prompt.
        string prompt = $"{persona}\n\nPlayer: \"{question}\"\n\nCharacter:";
        Debug.Log("Sending text prompt to Gemini: " + prompt);

        // 3. Call the API client's coroutine to send the request.
        // We pass HandleTextResponse as the "callback" function.
        StartCoroutine(GeminiAPI_Client.Instance.GenerateContent(prompt, HandleTextResponse));
    }

   
    private void HandleTextResponse(string response)
    {
        if (!string.IsNullOrEmpty(response))
        {
            Debug.Log("Gemini Response: " + response);

            
            // 1. Store the text response
            this.storedTextResponse = response;

            // 2. Immediately call the Speech API.
            // We pass HandleSpeechResponse as the *next* callback.
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