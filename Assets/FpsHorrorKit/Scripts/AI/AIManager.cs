using UnityEngine;
using System;


public class AIManager : MonoBehaviour
{
    
    public static AIManager Instance { get; private set; }

    
    public static event Action<string> OnDialogueReceived;

    // Reference to the API client that will handle the web requests.
    private GeminiAPI_Client apiClient;

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
       

        // Get the API client component attached to the same GameObject.
        apiClient = GetComponent<GeminiAPI_Client>();
    }

    
    void Start()
    {
        
    }
    


    
    public void AskQuestion(string persona, string question)
    {
        // 1. Construct the final prompt by combining the persona and the question.
        // This is the core of our "Prompt Engineering".
        string prompt = $"{persona}\n\nPlayer: \"{question}\"\n\nCharacter:";

        Debug.Log("Sending prompt to Gemini: " + prompt);

        // 2. Call the API client to send the request.
        // We pass a "callback" function to handle the response when it arrives.
        apiClient.SendRequest(prompt, (response) =>
        {
            // This code block is executed when the API client gets a response.
            if (response != null)
            {
                Debug.Log("Gemini Response: " + response);
                // 3. Broadcast the successful response to any listening scripts.
                OnDialogueReceived?.Invoke(response);
            }
            else
            {
                Debug.LogError("Failed to get a response from Gemini.");
                
            }
        });
    }
}