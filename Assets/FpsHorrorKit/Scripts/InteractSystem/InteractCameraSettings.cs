namespace FpsHorrorKit
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.HighDefinition;

    public class InteractCameraSettings : MonoBehaviour
    {
        private Volume volume;
        private DepthOfField depthOfField;
        private float startFocusDistance;

        public static InteractCameraSettings Instance;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            // Get the Volume component in the scene
            volume = FindAnyObjectByType<Volume>();

            if (volume != null && volume.profile.TryGet<DepthOfField>(out depthOfField))
            {
                startFocusDistance = depthOfField.focusDistance.value;
            }
            else
            {
                Debug.LogError("Depth of Field or Volume not found!");
            }
        }

        // Enable or disable Depth of Field
        public void Interacting(float focusDistanceWhenInspecting = .5f)
        {
            if (depthOfField != null)
            {
                depthOfField.focusDistance.value = focusDistanceWhenInspecting;
            }
        }

        public void NotInteracting()
        {
            if (depthOfField != null)
            {
                depthOfField.focusDistance.value = startFocusDistance;
            }
        }

        // Enable or disable the cursor
        public void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Enable or disable the cursor
        public void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
