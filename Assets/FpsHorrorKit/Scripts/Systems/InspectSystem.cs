using FpsHorrorKit;
using System.Collections;
using UnityEngine;

public class InspectSystem : MonoBehaviour, IInteractable
{
    [Header("Player Controller")]
    [Tooltip("Assign the Player object here.")]
    [SerializeField] private FpsController _fpsController;

    [Header("For Rotation")]
    [Tooltip("Object rotation speed")] public float rotationSpeed;
    [Tooltip("Speed for returning to the starting position")] public float returnSpeed = 5f;

    [Header("For Camera")]
    [Tooltip("Distance to the camera during inspection")] public float distanceToCamera;

    [Header("Higlight UI")]
    public string interactText = "Press [E] to Inspect";

    [Header("For Lantern")]
    public Light lanternLight;
    public float lightInspectIntensity;

    private float _startLightIntensity;
    private FpsAssetsInputs _input;
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private bool _isInspecting;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        // We find the input on the Player object assigned in the inspector.
        if (_fpsController != null)
        {
            _input = _fpsController.GetComponent<FpsAssetsInputs>();
        }
        else
        {
            Debug.LogError($"FATAL ERROR: FpsController is not assigned in the Inspector on {gameObject.name}!");
        }
    }

    private void Start()
    {
        _startPosition = transform.position;
        _startRotation = transform.rotation;
        if (lanternLight != null)
            _startLightIntensity = lanternLight.intensity;
    }

    private void LateUpdate()
    {
        // Guard Clause: Only run if we are in the inspection state.
        if (GameManager.Instance.CurrentGameState != GameState.InInspection || !_isInspecting) return;

        HandleInspection();
    }

    public void Interact()
    {
        // Safety check.
        if (_fpsController == null || Camera.main == null)
        {
            Debug.LogError("Cannot inspect: FpsController or Main Camera is missing!");
            return;
        }

        GameManager.Instance.SetGameState(GameState.InInspection);
        _isInspecting = true;

        Vector3 targetPosition = Camera.main.transform.position + Camera.main.transform.forward * distanceToCamera;

        StopAllCoroutines();
        StartCoroutine(SmoothTransition(targetPosition, _startRotation));

        ToggleCollider(false);
        ToggleLantern(false);
    }

    private void HandleInspection()
    {
        InteractCameraSettings.Instance?.Interacting(distanceToCamera);

        float rotationX = _input.look.x * rotationSpeed * Time.deltaTime;
        float rotationY = _input.look.y * rotationSpeed * Time.deltaTime;

        transform.Rotate(Camera.main.transform.up, -rotationX, Space.World);
        transform.Rotate(Camera.main.transform.right, rotationY, Space.World); // Note: Flipped sign for more intuitive rotation

        if (_input.stopInteract)
        {
            _input.stopInteract = false;
            StopInspection();
        }
    }

    private void StopInspection()
    {
        _isInspecting = false;
        StopAllCoroutines();
        StartCoroutine(SmoothTransition(_startPosition, _startRotation));

        GameManager.Instance.SetGameState(GameState.Gameplay);

        InteractCameraSettings.Instance?.NotInteracting();
        ToggleLantern(true);
        ToggleCollider(true);
    }

    private IEnumerator SmoothTransition(Vector3 targetPosition, Quaternion targetRotation)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f || Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, returnSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, returnSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }

    private void ToggleCollider(bool isActive)
    {
        if (_collider != null) _collider.enabled = isActive;
    }

    private void ToggleLantern(bool isActive)
    {
        if (lanternLight != null)
        {
            lanternLight.intensity = isActive ? _startLightIntensity : lightInspectIntensity;
        }
    }

    public void Highlight()
    {
        PlayerInteract.Instance.ChangeInteractText(interactText);
    }

    public void HoldInteract() { }
    public void UnHighlight() { }
}

