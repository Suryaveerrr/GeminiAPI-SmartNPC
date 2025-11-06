# GeminiAPI-SmartNPC
A first-person narrative horror game where you solve a domestic mystery by having real-time conversations with AI-powered characters, built in Unity with the Google Gemini API.
A 3D narrative horror game where you solve a mystery by having real-time, multi-modal conversations with generative AI-powered NPCs.

About The Project

This project was built to solve the "illusion of choice" problem found in traditional narrative games. Instead of presenting the player with a pre-written dialogue tree (A, B, C), this game provides a free-form text input field. The player must actively think, listen, and ask the right questions to uncover a dark family secret.

The challenge is no longer "click all the options," but "think like a detective."

Core Features

Real-time Generative Dialogue: Replaces static dialogue trees. The player can ask anything, leading to emergent, unscripted conversations.

Dynamic Persona System: Each NPC's personality, secrets, knowledge, and voice are controlled by a detailed "persona prompt" written in the Unity Inspector.

Multi-Modal Synchronized Output: The system's key innovation. It generates and delivers Text, Text-to-Speech Audio, and 3D Animation all at the exact same time for a polished, immersive experience.

Chained API Call Architecture: A robust system that first generates the text response, then immediately generates the speech for that text, ensuring the audio always matches.

Robust State Management: A central GameManager (Singleton) prevents all input conflicts, cleanly separating "Gameplay" and "InDialogue" states.

Technical Architecture: The Multi-Modal Flow

The system's greatest challenge was solving the audio/text sync issue. This was achieved with a "chained" 4-step architecture:

INPUT: The DialogueUI_Manager captures the player's text and sends the persona, voiceName, and question to the AIManager. A "..." loading message is shown.

TEXT API CALL: The AIManager starts a coroutine and tells the GeminiAPI_Client to call the gemini-2.5-flash-preview model.

SPEECH API CALL: When the text response is received, it is hidden from the player. The AIManager immediately makes a second API call, sending the new text to the gemini-2.5-flash-preview-tts model.

SYNCHRONIZED RESPONSE: When the AudioClip is generated and returned, the AIManager fires a single OnDialogueReady event. The DialogueUI_Manager receives this event and does three things on the same frame:

Shows the generated text.

Plays the generated AudioClip.

Triggers the startTalking animation.

Technologies Used

Game Engine: Unity 2022.3 (LTS)

Language: C#

Text Generation: Google Gemini API (gemini-2.5-flash-preview)

Speech Generation: Google Gemini TTS API (gemini-2.5-flash-preview-tts)

Animation: Adobe Mixamo

Web Requests: UnityWebRequest (for asynchronous API calls)

UI: TextMeshPro

Getting Started

To get this project running, you will need a valid Google AI API key.

Core Script Breakdown

AIManager.cs: The "brain" of the system. Manages the state of the conversation and orchestrates the chained API calls.

GeminiAPI_Client.cs: The "delivery driver." Manages all low-level UnityWebRequest calls for both text and TTS, handles JSON, and converts Base64 audio to AudioClip.

DialogueUI_Manager.cs: The "waiter." Manages all UI elements, listens for the final OnDialogueReady event, and fires all three synchronized outputs.

AI_NPC.cs: A simple data component attached to characters, holding their persona and voiceName.

GameManager.cs: The master state controller. Pauses player movement and input when the dialogue UI is active.

Acknowledgments

All 3D character models sourced from the Unity Asset Store.

All character animations sourced from Adobe Mixamo.
