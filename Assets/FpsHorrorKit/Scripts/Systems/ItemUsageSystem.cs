using FpsHorrorKit;
using UnityEngine;

public class ItemUsageSystem : MonoBehaviour
{
    public static ItemUsageSystem Instance { get; private set; }

    [Header("Player Input")]
    // We will assign this directly in the Inspector.
    [SerializeField] private FpsAssetsInputs _input;

    [Header("Items")]
    [SerializeField] private Item itemLantern;
    [SerializeField] private Item itemCamera;

    [Header("Item Objects Flaslight")]
    public GameObject lantern;
    public GameObject _light;
    public GameObject _lanternCanvas;

    [Header("Item Objects Camera")]
    public GameObject photoCaptureSystem;
    public GameObject cameraFrameUI;
    public GameObject cameraCanvas;
    public bool isAlbumActive = false;

    private bool isFirstCameraOpen = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // We removed the FindObjectByType call. Now we check if it was assigned.
        if (_input == null)
        {
            Debug.LogError("FATAL ERROR: Player Inputs (FpsAssetsInputs) is not assigned in the ItemUsageSystem Inspector!");
        }
    }

    private void Start()
    {
        // ... (The rest of your Start method is unchanged)
        // Update flashlight status when the game starts
        itemLantern.canUseItem = false;
        itemLantern.isUsingItem = false;
        if (itemLantern.energyLevel > 0)
        {
            itemLantern.isEnergyEnough = true;
        }
        else
        {
            itemLantern.isEnergyEnough = false;
        }

        // Update camera status when the game starts
        itemCamera.canUseItem = false;
        itemCamera.isUsingItem = false;
        if (itemCamera.energyLevel > 0)
        {
            itemCamera.isEnergyEnough = true;
        }
        else
        {
            itemCamera.isEnergyEnough = false;
        }
    }

    private void Update()
    {
        // Guard Clause: Do not process input if not in Gameplay state.
        if (GameManager.Instance.CurrentGameState != GameState.Gameplay) return;
        if (_input == null) return; // Safety check

        CheckInputSelect();
        CheckInputUse();
    }

    private void CheckInputSelect()
    {
        if (isAlbumActive) { return; }

        if (_input.itemIndex == 1 && _input.isPressed)
        {
            SelectFlashlight();
            DiSelectCamera();
            _input.isPressed = false;
            return;
        }
        if (_input.itemIndex == 2 && _input.isPressed)
        {
            SelectCamera();
            DiSelectFlashlight();
            _input.isPressed = false;
            return;
        }
        if (_input.itemIndex == 3 && _input.isPressed)
        {
            DiSelectFlashlight();
            DiSelectCamera();
            _input.isPressed = false;
            return;
        }
        if (_input.itemIndex == 4 && _input.isPressed)
        {
            DiSelectFlashlight();
            DiSelectCamera();
            _input.isPressed = false;
            return;
        }
    }

    public void CheckInputUse()
    {
        if (_input.useFlashlight)
        {
            UseFlashlight();
            _input.useFlashlight = false;
        }
        if (_input.useCamera)
        {
            UseCamera();
            _input.useCamera = false;
        }
    }

    public void SelectFlashlight()
    {
        if (lantern == null) { Debug.LogError("Flashlight Object not found!"); return; }
        if (_light == null) { Debug.LogError("Flashlight Light not found!"); return; }
        if (_lanternCanvas == null) { Debug.LogError("Flashlight Canvas not found!"); return; }

        if (itemLantern.hasItem)
        {
            itemLantern.canUseItem = _input.isSelectedItem;
            // This value will be true when the flashlight starts being used, not when it's selected.
            itemLantern.isUsingItem = false;

            lantern.SetActive(_input.isSelectedItem);
            _lanternCanvas.SetActive(_input.isSelectedItem);
            _light.SetActive(false);
        }
    }

    public void DiSelectFlashlight()
    {
        if (lantern == null) { Debug.LogError("Flashlight Object not found!"); return; }
        if (_light == null) { Debug.LogError("Flashlight Light not found!"); return; }
        if (_lanternCanvas == null) { Debug.LogError("Flashlight Canvas not found!"); return; }

        if (itemLantern.hasItem)
        {
            itemLantern.canUseItem = false;
            itemLantern.isUsingItem = false;

            lantern.SetActive(false);
            _lanternCanvas.SetActive(false);
            _light.SetActive(false);
        }
    }

    public void UseFlashlight()
    {
        if (lantern == null) { Debug.LogError("Flashlight Object not found!"); return; }
        if (_light == null) { Debug.LogError("Flashlight Light not found!"); return; }
        if (_lanternCanvas == null) { Debug.LogError("Flashlight Canvas not found!"); return; }

        if (itemLantern.hasItem && itemLantern.canUseItem)
        {
            itemLantern.isUsingItem = !itemLantern.isUsingItem;

            if (itemLantern.isEnergyEnough)
            {
                _light.SetActive(!_light.activeSelf);
            }
        }
    }

    public void SelectCamera()
    {
        if (photoCaptureSystem == null) { Debug.LogError("Camera Object not found!"); return; }
        if (cameraFrameUI == null) { Debug.LogError("Camera Frame UI not found!"); return; }
        if (cameraCanvas == null) { Debug.LogError("Camera Canvas not found!"); return; }

        if (itemCamera.hasItem)
        {
            itemCamera.canUseItem = _input.isSelectedItem;
            itemCamera.isUsingItem = _input.isSelectedItem;

            photoCaptureSystem.SetActive(_input.isSelectedItem);
            cameraFrameUI.SetActive(_input.isSelectedItem);
            cameraCanvas.SetActive(_input.isSelectedItem);

            if (isFirstCameraOpen == false)
            {
                InteractMessageScript.Instance?.ShowMessage("Press tab to open album!");
                isFirstCameraOpen = true;
            }
        }
    }

    public void DiSelectCamera()
    {
        if (photoCaptureSystem == null) { Debug.LogError("Camera Object not found!"); return; }
        if (cameraFrameUI == null) { Debug.LogError("Camera Frame UI not found!"); return; }
        if (cameraCanvas == null) { Debug.LogError("Camera Canvas not found!"); return; }

        if (itemCamera.hasItem)
        {
            itemCamera.canUseItem = false;
            itemCamera.isUsingItem = false;

            photoCaptureSystem.SetActive(false);
            cameraFrameUI.SetActive(false);
            cameraCanvas.SetActive(false);
        }
    }

    public void UseCamera()
    {
        if (photoCaptureSystem == null) { Debug.LogError("Camera Object not found!"); return; }
        if (PhotoCaptureSystem.Instance == null) { Debug.LogError("PhotoCaptureSystem script not found!"); return; }

        if (itemCamera.hasItem && itemCamera.canUseItem && itemCamera.isEnergyEnough)
        {
            PhotoCaptureSystem.Instance.CapturePhoto();
        }
    }
}

