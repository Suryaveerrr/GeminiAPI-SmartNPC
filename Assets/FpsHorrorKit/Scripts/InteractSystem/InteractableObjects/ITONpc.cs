namespace FpsHorrorKit
{
    using UnityEngine;

    public class ITONpc : MonoBehaviour, IInteractable
    {
        [Tooltip("Dialogue datas for the NPC")] public DialogueData[] dialogueData;
        [SerializeField] private string interactText = "Talk Npc [E]";
        private int currentDialogueIndex = 0;

        public void Interact()
        {
            if (GameManager.Instance.CurrentGameState != GameState.Gameplay) return;

            bool dialogueStarted = DialogueSystem.Instance.StartDialogue(currentDialogueIndex, dialogueData, OnDialogueEnd);

            if (dialogueStarted)
            {
                currentDialogueIndex++;
            }
        }

        public void Highlight()
        {
            PlayerInteract.Instance.ChangeInteractText(interactText);
        }

        private void OnDialogueEnd()
        {
            // The old DialogueSystem script should handle returning the game state to Gameplay.
        }

        public void HoldInteract() { }
        public void UnHighlight() { }
    }
}

