namespace FpsHorrorKit
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class PhotoCaptureSystem : MonoBehaviour
    {
        public static PhotoCaptureSystem Instance { get; private set; }

        [Header("Inventory Item")]
        public Item photoCameraItem;

        [Header("Camera Settings")]
        public Camera photoCamera; // The camera that takes the photo
        public RenderTexture renderTexture; // The RenderTexture where the photo will be captured
        public PhotoAlbum photoAlbum; // The ScriptableObject where we will store the photos

        [Header("UI Elements")]
        public GameObject photoUIPanel; // The panel where the photo is displayed
        public Image displayImage; // The UI Image element where the photo is shown
        public Image currentDisplayImage; // The UI Image element to show the photo immediately after capture
        public Button nextPhotoButton; // The button used to show the next photo
        public Button previousPhotoButton; // The button used to show the previous photo

        private FpsController _fpsController;

        private int photoIndex = 0; // A variable to keep track of the photo's order
        private bool isShowPhoto = false;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            _fpsController = FindAnyObjectByType<FpsController>();
        }
        private void Start()
        {
            photoAlbum.photos.Clear();
            nextPhotoButton.onClick.AddListener(() => ShowPhoto(photoIndex + 1, true));
            previousPhotoButton.onClick.AddListener(() => ShowPhoto(photoIndex - 1, true));
        }
        private void Update()
        {
            if (photoCameraItem.hasItem && photoCameraItem.canUseItem)
            {
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    OpenAlbum();
                }
            }
        }
        public void OpenAlbum()
        {
            isShowPhoto = !isShowPhoto;
            _fpsController.isInteracting = isShowPhoto;
            ItemUsageSystem.Instance.isAlbumActive = isShowPhoto;

            if (isShowPhoto)
            {
                InteractCameraSettings.Instance.Interacting();
                InteractCameraSettings.Instance.ShowCursor();
                ItemUsageSystem.Instance.cameraFrameUI.SetActive(false);
            }
            else
            {
                InteractCameraSettings.Instance.NotInteracting();
                InteractCameraSettings.Instance.HideCursor();
                ItemUsageSystem.Instance.cameraFrameUI.SetActive(true);
            }
            ShowPhoto(0, isShowPhoto);
        }

        public void CapturePhoto()
        {
            // Photo capture process
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = renderTexture;

            photoCamera.targetTexture = renderTexture;
            photoCamera.Render();

            Texture2D photo = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            photo.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            photo.Apply();

            RenderTexture.active = currentRT;
            photoCamera.targetTexture = null;

            // Convert the photo to a Sprite
            Sprite photoSprite = Sprite.Create(photo, new Rect(0, 0, photo.width, photo.height), new Vector2(0.5f, 0.5f));

            // Save the photo to the ScriptableObject
            photoAlbum.AddPhoto(photoSprite);

            // Show in the UI Image element
            if (currentDisplayImage != null)
            {
                currentDisplayImage.gameObject.SetActive(true);
                currentDisplayImage.sprite = photoSprite;
                StartCoroutine(DelayedShowPhoto(0.5f));
            }
        }

        public void ShowPhoto(int index, bool isShow)
        {
            photoUIPanel.gameObject.SetActive(isShow);
            if (index >= 0 && index < photoAlbum.photos.Count)
            {
                displayImage.sprite = photoAlbum.photos[index];
                photoIndex = index;
            }
        }
        IEnumerator DelayedShowPhoto(float delay)
        {
            yield return new WaitForSeconds(delay);
            currentDisplayImage.gameObject.SetActive(false);
        }
    }
}
