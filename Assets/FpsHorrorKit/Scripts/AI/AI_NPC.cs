using FpsHorrorKit;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class AI_NPC : MonoBehaviour, IInteractable
{
    [Header("Persona")]
    [Tooltip("The secret instructions and knowledge for this NPC.")]
    [TextArea(10, 20)]
    public string persona;

    
    [Header("Voice")]
    [Tooltip("The prebuilt voice name (e.g., 'Kore', 'Puck', 'Leda')")]
    public string voiceName = "Kore";
    

    [Header("Interaction")]
    [SerializeField] private string interactText = "Talk [E]";

    public void Interact()
    {
        
        DialogueUI_Manager.Instance.StartConversation(this);
    }

    public void Highlight()
    {
        PlayerInteract.Instance.ChangeInteractText(interactText);
    }

    
    public void HoldInteract() { }
    public void UnHighlight() { }
}
