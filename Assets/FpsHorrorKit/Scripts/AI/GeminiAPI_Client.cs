using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;


public class GeminiAPI_Client : MonoBehaviour
{
    
    private string apiKey = "AIzaSyCiBmdAuM2jorcwx4NnQEz2jhlGravNcWg";
   

    private string model = "gemini-2.5-flash";
    private string url;

    // Data structures to match the JSON format for requests and responses.
    [Serializable]
    private class RequestBody
    {
        public Content[] contents;
    }

    [Serializable]
    private class Content
    {
        public Part[] parts;
    }

    [Serializable]
    private class Part
    {
        public string text;
    }

    [Serializable]
    private class GeminiResponse
    {
        public Candidate[] candidates;
    }

    [Serializable]
    private class Candidate
    {
        public Content content;
    }

    void Awake()
    {
        // Construct the full URL for the API request.
        url = $"https://generativelanguage.googleapis.com/v1/models/{model}:generateContent?key={apiKey}";
    }

    // Public method to start the API request Coroutine.
    // It takes the prompt and a callback action to execute when the response is received.
    public void SendRequest(string prompt, Action<string> onResponse)
    {
        StartCoroutine(RequestCoroutine(prompt, onResponse));
    }

    // Coroutine to handle the asynchronous web request.
    private IEnumerator RequestCoroutine(string prompt, Action<string> onResponse)
    {
        // 1. Create the JSON payload for the request.
        var requestBody = new RequestBody
        {
            contents = new[] { new Content { parts = new[] { new Part { text = prompt } } } }
        };
        string jsonBody = JsonUtility.ToJson(requestBody);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        // 2. Create and configure the UnityWebRequest.
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // 3. Send the request and wait for a response.
            yield return request.SendWebRequest();

            // 4. Handle the response.
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error);
                Debug.LogError("Response: " + request.downloadHandler.text); // Log the full error response from the server.
                onResponse?.Invoke(null); // Return null on error.
            }
            else
            {
                // 5. Parse the successful response and extract the generated text.
                string jsonResponse = request.downloadHandler.text;
                GeminiResponse responseData = JsonUtility.FromJson<GeminiResponse>(jsonResponse);

                // Check if the response is valid and contains text.
                if (responseData != null && responseData.candidates.Length > 0)
                {
                    string generatedText = responseData.candidates[0].content.parts[0].text;
                    onResponse?.Invoke(generatedText.Trim());
                }
                else
                {
                    Debug.LogError("Invalid response format from Gemini API.");
                    onResponse?.Invoke(null);
                }
            }
        }
    }
}