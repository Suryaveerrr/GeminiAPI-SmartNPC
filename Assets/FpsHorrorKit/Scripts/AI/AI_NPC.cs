using FpsHorrorKit;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class AI_NPC : MonoBehaviour, IInteractable
{
    [Header("Persona")]
    [Tooltip("The secret instructions and knowledge for this NPC.")]
    [TextArea(10, 20)]
    public string persona;

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

    // not needed for this object but required by the interface 
    public void HoldInteract() { }
    public void UnHighlight() { }
}