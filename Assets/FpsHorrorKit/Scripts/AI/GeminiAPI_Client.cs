using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System;


public class GeminiAPI_Client : MonoBehaviour
{
   
    public static GeminiAPI_Client Instance { get; private set; }

   
    private const string GeminiTextApiUrl = "PASTE_YOUR_API_KEY";
    private const string GeminiSpeechApiUrl = "PASTE_YOUR_API_KEY";
    private string apiKey = null; // This will be loaded at runtime

    #region Text API Data Structures
    [Serializable]
    private class TextRequest
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
    #endregion

    #region TTS API Data Structures
    [Serializable]
    private class TTSRequest
    {
        public TTSContents contents;
        public TTSGenerationConfig generationConfig;
        public string model = "gemini-2.5-flash-preview-tts";
    }

    [Serializable]
    private class TTSContents
    {
        public TTSPart[] parts;
    }

    [Serializable]
    private class TTSPart
    {
        public string text;
    }

    [Serializable]
    private class TTSGenerationConfig
    {
        public string[] responseModalities = { "AUDIO" };
        public TTSSpeechConfig speechConfig;
    }

    [Serializable]
    private class TTSSpeechConfig
    {
        public TTSVoiceConfig voiceConfig;
    }

    [Serializable]
    private class TTSVoiceConfig
    {
        public TTSPrebuiltVoiceConfig prebuiltVoiceConfig;
    }

    [Serializable]
    private class TTSPrebuiltVoiceConfig
    {
        public string voiceName;
    }

    [Serializable]
    private class TTSResponse
    {
        public TTSCandidate[] candidates;
    }

    [Serializable]
    private class TTSCandidate
    {
        public TTSContent content;
    }

    [Serializable]
    private class TTSContent
    {
        public TTSAudioPart[] parts;
    }

    [Serializable]
    private class TTSAudioPart
    {
        public TTSInlineData inlineData;
    }

    [Serializable]
    private class TTSInlineData
    {
        public string mimeType; 
        public string data;     
    }
    #endregion


    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); 

        apiKey = GetApiKey();
    }

    
    private string GetApiKey()
    {
        
        
        string key = "AIzaSyCiBmdAuM2jorcwx4NnQEz2jhlGravNcWg"; 
        if (string.IsNullOrEmpty(key))
        {
            Debug.LogWarning("API Key is not set in GeminiAPI_Client.cs. Please add your key.");
        }
        return key;
    }

    
    public IEnumerator GenerateContent(string prompt, Action<string> onResponse)
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("GeminiAPI Error: API Key is missing.");
            onResponse?.Invoke(null);
            yield break;
        }
        string fullApiUrl = GeminiTextApiUrl + apiKey;

        
        var requestBody = new TextRequest
        {
            contents = new[] { new Content { parts = new[] { new Part { text = prompt } } } }
        };
        string jsonBody = JsonUtility.ToJson(requestBody);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

       
        using (UnityWebRequest uwr = new UnityWebRequest(fullApiUrl, "POST"))
        {
            uwr.uploadHandler = new UploadHandlerRaw(bodyRaw);
            uwr.downloadHandler = new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            yield return uwr.SendWebRequest();

           
            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("GeminiAPI Text Error: " + uwr.error);
                Debug.LogError("Response: " + uwr.downloadHandler.text);
                onResponse?.Invoke(null);
            }
            else
            {
               
                string jsonResponse = uwr.downloadHandler.text;
                GeminiResponse responseData = JsonUtility.FromJson<GeminiResponse>(jsonResponse);

                if (responseData != null && responseData.candidates != null && responseData.candidates.Length > 0)
                {
                    string generatedText = responseData.candidates[0].content.parts[0].text;
                    onResponse?.Invoke(generatedText.Trim());
                }
                else
                {
                    Debug.LogWarning("GeminiAPI: No text candidate returned or invalid response format.");
                    Debug.Log("Raw Response: " + jsonResponse);
                    onResponse?.Invoke(null);
                }
            }
        }
    }

   
    public IEnumerator GenerateSpeech(string textToSpeak, string voiceName, System.Action<AudioClip> onSpeechReady)
    {
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("GeminiAPI Error: API Key is missing.");
            onSpeechReady?.Invoke(null);
            yield break;
        }
        string fullApiUrl = GeminiSpeechApiUrl + apiKey;

        // 1. Construct the TTSRequest payload
        TTSRequest ttsRequest = new TTSRequest
        {
            contents = new TTSContents
            {
                parts = new TTSPart[] { new TTSPart { text = textToSpeak } }
            },
            generationConfig = new TTSGenerationConfig
            {
                speechConfig = new TTSSpeechConfig
                {
                    voiceConfig = new TTSVoiceConfig
                    {
                        prebuiltVoiceConfig = new TTSPrebuiltVoiceConfig
                        {
                            voiceName = voiceName
                        }
                    }
                }
            }
        };

        string jsonPayload = JsonUtility.ToJson(ttsRequest);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);

        
        using (UnityWebRequest uwr = new UnityWebRequest(fullApiUrl, "POST"))
        {
            uwr.uploadHandler = new UploadHandlerRaw(bodyRaw);
            uwr.downloadHandler = new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.Success)
            {
                // 3. Parse the response
                TTSResponse ttsResponse = JsonUtility.FromJson<TTSResponse>(uwr.downloadHandler.text);

                if (ttsResponse.candidates != null && ttsResponse.candidates.Length > 0 &&
                    ttsResponse.candidates[0].content != null && ttsResponse.candidates[0].content.parts != null &&
                    ttsResponse.candidates[0].content.parts.Length > 0 && ttsResponse.candidates[0].content.parts[0].inlineData != null)
                {
                    TTSInlineData audioData = ttsResponse.candidates[0].content.parts[0].inlineData;

                    // 4. Decode and Convert PCM data to an AudioClip
                    AudioClip clip = ConvertToAudioClip(audioData.data, audioData.mimeType);
                    onSpeechReady?.Invoke(clip);
                }
                else
                {
                    Debug.LogWarning("GeminiAPI: No speech candidate returned or invalid audio data.");
                    Debug.Log("Raw Response: " + uwr.downloadHandler.text);
                    onSpeechReady?.Invoke(null);
                }
            }
            else
            {
                Debug.LogError("GeminiAPI Speech Error: " + uwr.error);
                Debug.LogError("Response: " + uwr.downloadHandler.text);
                onSpeechReady?.Invoke(null);
            }
        }
    }

    
    private AudioClip ConvertToAudioClip(string base64Data, string mimeType)
    {
        try
        {
            
            int sampleRate = 24000; // Default
            if (!string.IsNullOrEmpty(mimeType))
            {
                string[] mimeParts = mimeType.Split(';');
                foreach (string part in mimeParts)
                {
                    if (part.Trim().StartsWith("rate="))
                    {
                        int.TryParse(part.Split('=')[1], out sampleRate);
                        break;
                    }
                }
            }

          
            byte[] pcmData = System.Convert.FromBase64String(base64Data);

           
            float[] floatData = PCM16ByteArrayToFloatArray(pcmData);

            
            AudioClip clip = AudioClip.Create("NPC_Speech", floatData.Length, 1, sampleRate, false);
            clip.SetData(floatData, 0);

            return clip;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error converting PCM to AudioClip: " + e.Message);
            return null;
        }
    }

   
    private float[] PCM16ByteArrayToFloatArray(byte[] pcmData)
    {
        
        int samples = pcmData.Length / 2;
        float[] floatArray = new float[samples];

        for (int i = 0; i < samples; i++)
        {
            
            short sample = System.BitConverter.ToInt16(pcmData, i * 2);

            
            floatArray[i] = sample / 32768f;
        }

        return floatArray;
    }
}
