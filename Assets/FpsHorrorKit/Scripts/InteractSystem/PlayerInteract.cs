using FpsHorrorKit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteract : MonoBehaviour
{
    public static PlayerInteract Instance { get; private set; }
    [Header("Raycast Settings")]
    public float interactRange = 2.0f; // Interaction distance

    [Header("Highlight Settings")]
    public GameObject higlightObject;
    public TextMeshProUGUI interactTextUI;
    public Image interactImageUI;

    private IInteractable currentInteractable;

    private GameObject defaultHighlightObj;
    private string defaultInteractText;
    [SerializeField] private bool canDragDoor;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this); }
        else { Instance = this; }
    }
    private void Start()
    {
        defaultInteractText = "Press [E] to interact";
        if (interactTextUI != null) interactTextUI.text = defaultInteractText;

        defaultHighlightObj = higlightObject;
    }

    void Update()
    {
        // Guard Clause: Only run interaction logic if the game is in the Gameplay state.
        if (GameManager.Instance.CurrentGameState != GameState.Gameplay) return;

        SendRaycast();

        // Handle mouse input for dragging doors, separate from the raycast result.
        if (currentInteractable != null)
        {
            if (Input.GetMouseButton(0) && canDragDoor)
            {
                if (higlightObject != null) higlightObject.SetActive(false);
                currentInteractable.HoldInteract();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                UnHighlight();
                currentInteractable.UnHighlight();
                canDragDoor = false;
                currentInteractable = null;
            }
        }
    }

    private void SendRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                // We are looking at a new interactable object.
                if (interactable != currentInteractable)
                {
                    currentInteractable = interactable;
                    Highlight();
                }

                // Check for the interact key press.
                if (Input.GetKeyDown(KeyCode.E))
                {
                    currentInteractable.Interact();
                }

                // Allow door dragging.
                canDragDoor = true;
            }
            else
            {
                // We are looking at something, but it's not interactable.
                UnHighlight();
                currentInteractable = null;
            }
        }
        else
        {
            // We are looking at nothing.
            UnHighlight();
            currentInteractable = null;
        }
    }

    public void Highlight()
    {
        if (currentInteractable != null)
        {
            currentInteractable.Highlight(); // This tells the object (e.g., AI_NPC) to set its custom text.
        }
        if (higlightObject != null) higlightObject.SetActive(true);
    }

    public void UnHighlight()
    {
        if (currentInteractable != null)
        {
            currentInteractable.UnHighlight();
        }
        if (higlightObject != null) higlightObject.SetActive(false);
        if (interactTextUI != null) interactTextUI.text = defaultInteractText;

        higlightObject = defaultHighlightObj;
    }

    public void ChangeInteractText(string interactText)
    {
        if (interactTextUI != null)
        {
            interactTextUI.text = interactText;
            higlightObject = interactTextUI.gameObject;
        }
    }

    public void ChangeInteractImage(Sprite interactImage)
    {
        if (interactImageUI != null)
        {
            interactImageUI.sprite = interactImage;
            higlightObject = interactImageUI.gameObject;
        }
    }
}

