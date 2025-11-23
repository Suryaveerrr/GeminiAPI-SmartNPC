# 👁️ Real-Time AI Narrative Horror

![Unity](https://img.shields.io/badge/Unity-2022.3%20LTS-black?style=flat&logo=unity)
![AI](https://img.shields.io/badge/Model-Gemini%202.5%20Flash-4285F4?style=flat&logo=google)
![Status](https://img.shields.io/badge/Status-Prototype-orange)

> **A first-person mystery where every conversation matters. No dialogue trees. No scripts. Just you and the AI.**

This is a narrative horror experiment where **you don't pick pre-written dialogue options**. Instead, you must think like a detective, typing your own questions to uncover a dark family secret.

Powered by **Google Gemini**, NPCs generate real-time, context-aware dialogue (Text + Speech + Animation), ensuring that no two players will ever have the same experience.

<img width="1580" height="885" alt="image" src="https://github.com/user-attachments/assets/1462606d-370f-4d1b-a221-e9663b5e6fe3" />
<img width="1574" height="883" alt="image" src="https://github.com/user-attachments/assets/4e8d0ba1-30c7-4ccb-b6b8-91065915e83e" />
<img width="1576" height="883" alt="image" src="https://github.com/user-attachments/assets/5f429e09-f2ed-40b0-95d1-8116e9410064" />
<img width="1576" height="882" alt="image" src="https://github.com/user-attachments/assets/bae7a30b-559c-4ba6-afc5-64877ee7505a" />
<img width="1580" height="877" alt="image" src="https://github.com/user-attachments/assets/085ba6e1-3ccd-443f-bb4b-d06fb5a0e8ef" />
<img width="1581" height="887" alt="image" src="https://github.com/user-attachments/assets/8726d8b4-5d80-4605-9d71-52832d0bb159" />
<img width="1580" height="883" alt="image" src="https://github.com/user-attachments/assets/c1ea3356-b81a-469f-a178-4d9206953ccc" />


---

## 🎯 Project Goal

Traditional narrative games limit storytelling by offering predictable A/B/C dialogue trees. This project eliminates the illusion of choice.

* ✅ **True Freedom:** You speak freely; the characters respond naturally.
* ✅ **Emergent Story:** Every secret must be uncovered through actual deduction.
* ✅ **Live Generation:** Every response is AI-generated in real-time.

---

## 🔧 Technical Architecture

The core innovation is a **4-Step Synchronized Multi-Modal Flow**. The system ensures that text, audio, and animation remain perfectly synced to maintain immersion.

### The "Synchronized Delivery" Flow

1.  **Player Input** 📝
    * `DialogueUI_Manager` captures user text.
    * Sends the Persona, Voice Name, and Question to the `AIManager`.
    * UI displays a "..." thinking indicator.

2.  **Text Generation** 🧠
    * `AIManager` calls the **Gemini 2.5 Flash API**.
    * The response text is generated and stored internally (hidden from player).

3.  **Speech Generation** 🗣️
    * The text is immediately piped into the **Gemini 2.5 Flash TTS API**.
    * Returns an AudioClip (Base64 decoding handled internally).

4.  **Unified Delivery** 🎬
    * `AIManager` triggers the `OnDialogueReady` event.
    * **IN THE SAME FRAME:** The text reveals, the audio plays, and the NPC's talking animation triggers.

---

## 🧩 Core Features

* **Real-Time Generative Dialogue:** No static trees. The conversation evolves based on your exact words.
* **Dynamic Persona System:** NPCs have hidden motives, specific knowledge bases, and personality traits configured directly inside the Unity Inspector.
* **Multi-Modal Output Sync:** Eliminates the "lag" between reading text and hearing voice.
* **Game State Manager:** Handles clean transitions between free-roaming exploration and focused dialogue states.

---

## 🛠️ Tech Stack

| Category | Tool/Library |
| :--- | :--- |
| **Game Engine** | Unity 2022.3 LTS |
| **Language** | C# |
| **Text Model** | **Google Gemini 2.5 Flash Preview** |
| **Speech Model** | **Google Gemini 2.5 Flash TTS** |
| **Networking** | UnityWebRequest (Async/Await) |
| **Animation** | Adobe Mixamo |
| **UI** | TextMeshPro |

---

## 📂 Script Overview

| Script | Responsibility |
| :--- | :--- |
| `AIManager.cs` | The brain. Orchestrates the flow between text generation, TTS, and state handling. |
| `GeminiAPI_Client.cs` | Handles the raw API requests, JSON parsing, and Base64 audio conversion. |
| `DialogueUI_Manager.cs` | Manages the frontend: displays text, plays audio sources, and triggers Animator parameters. |
| `AI_NPC.cs` | Configurable component defining the character's specific persona prompts and voice settings. |
| `GameManager.cs` | Controls macro gameplay states (Switching between First-Person Controller and Dialogue Mode). |

---

## 🚀 Getting Started

### Requirements
* Unity 2022.3 LTS or higher
* A valid **Google AI Studio API Key**

### Installation
1.  Clone the repository.
2.  Open the project in Unity.
3.  Navigate to `Assets/Resources/GoogleAIConfig.asset` (or where you stored your config).
4.  Paste your **API Key**.
5.  Select an NPC in the scene and edit their **Persona Prompt** in the `AI_NPC` component.
6.  Hit **Play** and start the interrogation.

---
