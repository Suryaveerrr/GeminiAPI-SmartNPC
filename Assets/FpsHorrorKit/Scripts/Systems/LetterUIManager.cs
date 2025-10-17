using FpsHorrorKit;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterUIManager : MonoBehaviour
{
    public static LetterUIManager Instance { get; private set; }

    [Header("Letter UI")]
    [SerializeField] GameObject _letterUI;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Button _closeButton;

    [Header("Typing")]
    [SerializeField] private bool isTyping = false;
    [SerializeField] private float typingDelay = 0.1f;

    private FpsController _fpsController;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        _fpsController = GetComponent<FpsController>();
        if (_closeButton) _closeButton.onClick.AddListener(HideText);
    }

    private void Update()
    {
        // Allow closing with Escape key
        if (_letterUI.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            HideText();
        }
    }

    public void ShowText(string text)
    {
        // Check with GameManager before showing the letter
        if (GameManager.Instance.CurrentGameState != GameState.Gameplay) return;

        // Set the game state to ReadingLetter
        GameManager.Instance.SetGameState(GameState.ReadingLetter);
        InteractCameraSettings.Instance?.ShowCursor();
        if (_fpsController) _fpsController.enabled = false;

        if (isTyping) { StartCoroutine(Typing(text, typingDelay)); }
        else { _text.text = text; }

        _letterUI.SetActive(true);
    }

    public void HideText()
    {
        StopAllCoroutines();
        _letterUI.SetActive(false);
        _text.text = "";

        // Return the game state to Gameplay
        GameManager.Instance.SetGameState(GameState.Gameplay);
        InteractCameraSettings.Instance?.HideCursor();
        if (_fpsController) _fpsController.enabled = true;
    }

    IEnumerator Typing(string newText, float delay)
    {
        _text.text = "";
        foreach (char letter in newText.ToCharArray())
        {
            _text.text += letter;
            yield return new WaitForSeconds(delay);
        }
    }
}

