using UnityEngine;

// Define ALL the possible states of the game.
public enum GameState
{
    Gameplay,       // Player can move, interact, use items, etc.
    InDialogue,     // Player is in a conversation.
    InInspection,   // Player is inspecting an object.
    ReadingLetter,  // Player is reading a letter.
    Paused          // Game is paused (for menus, etc.).
}

// This is the master controller for the game's state.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentGameState { get; private set; }

    void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: keeps the manager across scene loads.

        // Start the game in the default gameplay state.
        SetGameState(GameState.Gameplay);
    }

    // Public method to change the game state.
    public void SetGameState(GameState newState)
    {
        CurrentGameState = newState;
        Debug.Log("Game State changed to: " + newState);
    }
}
