namespace FpsHorrorKit
{
    using UnityEngine;

    public class DragToOpenSystem : MonoBehaviour, IInteractable
    {
        public enum DoorDirection { left, right, front, back }

        [Header("Rotation Settings")]
        [Tooltip("Mouse movement sensitivity")]
        [SerializeField] private float rotationSpeed = 5f;

        [Tooltip("The angle of the door when it is closed (e.g., 0)")]
        [SerializeField] private float minAngle = 0f;

        [Tooltip("The angle of the door when it is fully open (e.g., 90)")]
        [SerializeField] private float maxAngle = 90f;

        [Tooltip("The direction that controls the door's interaction with the player")]
        [SerializeField] private DoorDirection doorDirection = DoorDirection.left;

        [Header("Collider Settings")]
        [SerializeField] private bool colliderDisabledDuringInteraction = false;

        [Header("Intercact Text")]
        [SerializeField] Sprite interactImageUi;

        

        private float currentAngle = 0f;
        private float initialAngle;
        private Vector3 initialForward;
        private Collider _collider;
        private Transform player;

        void Start()
        {
            _collider = GetComponent<Collider>();
            player = GameObject.FindGameObjectWithTag("Player").transform;

            initialAngle = transform.localEulerAngles.y;

            // Store the initial 'forward' direction of the door based on the enum.
            switch (doorDirection)
            {
                case DoorDirection.left:
                    initialForward = -transform.right;
                    break;
                case DoorDirection.right:
                    initialForward = transform.right;
                    break;
                case DoorDirection.front:
                    initialForward = transform.forward;
                    break;
                case DoorDirection.back:
                    initialForward = -transform.forward;
                    break;
            }
        }

        public void Interact()
        {
        }

        public void HoldInteract()
        {
            if (colliderDisabledDuringInteraction && _collider != null)
            {
                _collider.enabled = false;
            }
            // Get the mouse movement on the X-axis.
            float mouseX = Input.GetAxis("Mouse X");

            // To determine if the player is to the left or right of the door:
            float sideMultiplier = 1f; // default
            if (player != null)
            {
                // Vector from the door pivot to the player
                Vector3 doorToPlayer = player.position - transform.position;
                // Calculate the dot product with the initialForward vector.
                // If the result is positive, the player is on the "forward" side of the door.
                float dot = Vector3.Dot(initialForward, doorToPlayer);

                // Here, we reverse the effect of the mouse movement if the player is on the 'forward' side.
                sideMultiplier = (dot > 0) ? -1f : 1f;
            }

            // Update the angle based on mouse movement.
            currentAngle += mouseX * rotationSpeed * sideMultiplier;
            currentAngle = Mathf.Clamp(currentAngle, minAngle, maxAngle);
            float targetAngle = initialAngle + currentAngle;

            // Rotate the door around the Y-axis.
            transform.localEulerAngles = new Vector3(0, targetAngle, 0);
        }

        public void Highlight()
        {
            PlayerInteract.Instance.ChangeInteractImage(interactImageUi);
        }

        public void UnHighlight()
        {
            if (_collider != null)
            {
                _collider.enabled = true;
            }
        }
    }
}