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

    private AI_NPC currentNpc;
    private Animator currentNpcAnimator;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); }
        else { Instance = this; }

        sendButton.onClick.AddListener(SendPlayerQuestion);
        if (closeButton) closeButton.onClick.AddListener(CloseConversation);
    }

    private void OnEnable()
    {
        AIManager.OnDialogueReceived += DisplayNpcResponse;
    }

    private void OnDisable()
    {
        AIManager.OnDialogueReceived -= DisplayNpcResponse;
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

        PlayerInteract.Instance.UnHighlight();

        npcResponseText.text = "";
        playerInputField.text = "";
        dialogueCanvas.SetActive(true);

        InteractCameraSettings.Instance.ShowCursor();

        playerInputField.ActivateInputField();
    }

    public void CloseConversation()
    {
        

        dialogueCanvas.SetActive(false);
        currentNpc = null;
        currentNpcAnimator = null; // Clear the reference
        GameManager.Instance.SetGameState(GameState.Gameplay);

        InteractCameraSettings.Instance.HideCursor();
    }

    private void SendPlayerQuestion()
    {
        if (playerInputField.text != "" && currentNpc != null)
        {
            AIManager.Instance.AskQuestion(currentNpc.persona, playerInputField.text);

            npcResponseText.text = "...";

            playerInputField.text = "";
            playerInputField.ActivateInputField();
        }
    }

    private void DisplayNpcResponse(string response)
    {
        
        if (currentNpcAnimator != null)
        {
            currentNpcAnimator.SetTrigger("startTalking");
        }
       

        
        if (response.StartsWith("\"") && response.EndsWith("\""))
        {
            response = response.Substring(1, response.Length - 2);
        }
        npcResponseText.text = response;
    }
}

