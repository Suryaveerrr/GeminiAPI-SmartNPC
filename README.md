A Real-Time AI-Driven Narrative Horror Experience

This is a first-person mystery horror game where every conversation matters. Instead of picking pre-written dialogue options, you must think like a detective, asking the right questions to uncover a dark family secret.

Powered by Google Gemini, NPCs generate real-time, context-aware dialogue (text + speech + animation), ensuring no two players will ever have the same experience.

🎯 Project Goal

Traditional narrative games limit storytelling by offering predictable A/B/C dialogue trees.
This game eliminates the illusion of choice — you speak freely, the characters respond naturally.

✅ Every conversation is emergent
✅ Every secret must be uncovered through deduction
✅ Every response is AI-generated live

🧩 Core Features
Feature	Description
Real-Time Generative Dialogue	No static dialogue trees — players type freely to interact with NPCs.
Dynamic Persona System	NPCs have hidden motives, knowledge, and personality configured inside Unity.
Multi-Modal Output Sync	Text + TTS Audio + Animation play together perfectly every time.
Chained API Call Architecture	Ensures the speech always matches the generated text.
Game State Manager	Clean transitions between free movement and dialogue interaction.
🔧 Technical Architecture

The core innovation is a 4-Step Synchronized Multi-Modal Flow:

Player Input → Text Generation → Speech Generation → Unified Delivery

Detailed Breakdown:

Player Input

DialogueUI_Manager sends persona, voiceName, and question to AIManager

Displays a loading “…” indicator

Text Generation

AIManager calls the Gemini Text API (gemini-2.5-flash-preview)

Response text is stored but not yet shown

Speech Generation

AIManager immediately sends the text to Gemini TTS API
(gemini-2.5-flash-preview-tts)

AudioClip returned + Base64 decoding handled internally

Synchronized Response Event

AIManager triggers OnDialogueReady

DialogueUI_Manager:
✅ Reveals the text
✅ Plays the audio
✅ Triggers startTalking animation

🧠 All outputs appear on the very same frame — perfect immersion.

🏗️ Technologies Used
Category	Tool
Game Engine	Unity 2022.3 LTS
Programming Language	C#
Text AI Model	Google Gemini 2.5 Flash Preview
Speech Model	Google Gemini 2.5 Flash TTS
Networking	UnityWebRequest (Async Calls)
UI	TextMeshPro
Animation	Adobe Mixamo
🛠️ Core Script Overview
Script	Role
AIManager.cs	Orchestrates conversation flow + state handling
GeminiAPI_Client.cs	Handles API requests + JSON + Base64 audio conversion
DialogueUI_Manager.cs	Displays dialogue, plays audio, triggers animations
AI_NPC.cs	Defines each character’s persona + TTS voice settings
GameManager.cs	Controls gameplay states (exploration vs dialogue)
🚀 Getting Started
Requirements

✅ Unity 2022.3 LTS
✅ Google AI API Key

Setup

Install Unity 2022.3 (LTS)

Add your API key to the GoogleAIConfig.asset in Unity

Plug your NPC’s persona prompts into AI_NPC.cs component

Hit Play and start talking with the unknown…
