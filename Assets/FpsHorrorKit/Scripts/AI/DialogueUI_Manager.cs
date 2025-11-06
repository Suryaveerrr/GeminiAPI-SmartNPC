using FpsHorrorKit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI_Manager : MonoBehaviour
{
    public static DialogueUI_Manager Instance { get; private set; }

    [Header("UI Components")]
    [SerializeField] private GameObject dialogueCanvas;
    [SerializeField] private TextMeshProUGUI npcResponseText;
    [SerializeField] private TMP_InputField playerInputField;
    [SerializeField] private Button sendButton;
    [SerializeField] private Button closeButton;

    
    [Header("Audio")]
    [Tooltip("The AudioSource component that will play the NPC's speech.")]
    [SerializeField] private AudioSource npcAudioSource;
    private string currentVoiceName;
    

    private AI_NPC currentNpc;
    private Animator currentNpcAnimator;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); }
        else { Instance = this; }

        sendButton.onClick.AddListener(SendPlayerQuestion);
        if (closeButton) closeButton.onClick.AddListener(CloseConversation);

        
        // Safety check for the AudioSource
        if (npcAudioSource == null)
        {
            Debug.LogError("DialogueUI_Manager: NPC Audio Source is not assigned in the Inspector! Please add one.");
        }
        
    }

    private void OnEnable()
    {
        
        // Stop listening to the old event
        // AIManager.OnDialogueReceived -= DisplayNpcResponse;

        // Listen for the new, synchronized event
        AIManager.OnDialogueReady += DisplayDialogue;
        
    }

    private void OnDisable()
    {
        
        // AIManager.OnDialogueReceived -= DisplayNpcResponse;
        AIManager.OnDialogueReady -= DisplayDialogue;
        
    }

    private void Update()
    {
        // Guard clause for game state
        if (GameManager.Instance.CurrentGameState != GameState.InDialogue) return;

        // Allow closing with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseConversation();
        }
    }

    public void StartConversation(AI_NPC npc)
    {
        GameManager.Instance.SetGameState(GameState.InDialogue);
        currentNpc = npc;

        // Get the Animator from the NPC
        currentNpcAnimator = currentNpc.GetComponent<Animator>();

        
        // Get the voice name from the NPC
        currentVoiceName = npc.voiceName;
        

        PlayerInteract.Instance.UnHighlight();

        npcResponseText.text = "";
        playerInputField.text = "";
        dialogueCanvas.SetActive(true);

        InteractCameraSettings.Instance.ShowCursor();

        playerInputField.ActivateInputField();
    }

    public void CloseConversation()
    {
        // Stop any currently playing audio when closing
        if (npcAudioSource != null)
        {
            npcAudioSource.Stop();
        }

        dialogueCanvas.SetActive(false);
        currentNpc = null;
        currentNpcAnimator = null; // Clear the reference
        currentVoiceName = null; // Clear the voice name
        GameManager.Instance.SetGameState(GameState.Gameplay);

        InteractCameraSettings.Instance.HideCursor();
    }

    private void SendPlayerQuestion()
    {
        if (playerInputField.text != "" && currentNpc != null)
        {
            // Stop any playing audio when sending a new question
            if (npcAudioSource != null)
            {
                npcAudioSource.Stop();
            }

            
            // We now pass the voiceName when we ask the question
            AIManager.Instance.AskQuestion(currentNpc.persona, playerInputField.text, currentVoiceName);
            

            npcResponseText.text = "..."; // Show thinking indicator

            playerInputField.text = "";
            playerInputField.ActivateInputField();
        }
    }

    /// <summary>
    /// This function is called by the AIManager when a new text response is ready.
    /// </summary>
    

    
    /// <summary>
    /// This function is called by AIManager when BOTH text and audio are ready.
    /// </summary>
    private void DisplayDialogue(string response, AudioClip clip)
    {
        // 1. Trigger the talking animation
        if (currentNpcAnimator != null)
        {
            currentNpcAnimator.SetTrigger("startTalking");
        }

        // 2. Remove quotes from the response
        if (response.StartsWith("\"") && response.EndsWith("\"") && response.Length > 2)
        {
            response = response.Substring(1, response.Length - 2);
        }

        // 3. Set the text
        npcResponseText.text = response;

        // 4. Play the audio
        if (clip != null && npcAudioSource != null && GameManager.Instance.CurrentGameState == GameState.InDialogue)
        {
            npcAudioSource.PlayOneShot(clip);
        }
        else if (clip == null)
        {
            Debug.LogWarning("DisplayDialogue was called, but the AudioClip was null.");
        }
    }
 
    
}