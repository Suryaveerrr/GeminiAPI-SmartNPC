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
        
        if (GameManager.Instance.CurrentGameState != GameState.InDialogue) return;

        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseConversation();
        }
    }

    public void StartConversation(AI_NPC npc)
    {
        GameManager.Instance.SetGameState(GameState.InDialogue);
        currentNpc = npc;

        
        currentNpcAnimator = currentNpc.GetComponent<Animator>();

        
        
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
        
        if (npcAudioSource != null)
        {
            npcAudioSource.Stop();
        }

        dialogueCanvas.SetActive(false);
        currentNpc = null;
        currentNpcAnimator = null; 
        currentVoiceName = null; 
        GameManager.Instance.SetGameState(GameState.Gameplay);

        InteractCameraSettings.Instance.HideCursor();
    }

    private void SendPlayerQuestion()
    {
        if (playerInputField.text != "" && currentNpc != null)
        {
            
            if (npcAudioSource != null)
            {
                npcAudioSource.Stop();
            }

            
            
            AIManager.Instance.AskQuestion(currentNpc.persona, playerInputField.text, currentVoiceName);
            

            npcResponseText.text = "..."; 

            playerInputField.text = "";
            playerInputField.ActivateInputField();
        }
    }

    
    

    
   
    private void DisplayDialogue(string response, AudioClip clip)
    {
        
        if (currentNpcAnimator != null)
        {
            currentNpcAnimator.SetTrigger("startTalking");
        }

        
        if (response.StartsWith("\"") && response.EndsWith("\"") && response.Length > 2)
        {
            response = response.Substring(1, response.Length - 2);
        }

        npcResponseText.text = response;

        
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
