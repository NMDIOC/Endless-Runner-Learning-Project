# Endless-Runner-Learning-Project
This project is a dedicated learning exercise focused on creating a high-performance Lane-Based Endless Runner. The goal is to master procedural generation, object pooling, and efficient game-state management in Unity.
🎯 Learning Objectives
Procedural Generation: Implementing a dynamic obstacle and collectable spawning system.

Object Pooling: Optimizing performance by reusing objects instead of constant instantiation and destruction.

Game State Management: Creating a robust system to handle Game Over conditions, score tracking, and UI updates.

Architecture: Decoupling game logic using Events (C# Delegates/Actions).

🛠 Tech Stack
Engine: Unity [Insertar versión]

Language: C#

Core Systems:

Object Pooling for obstacles and coins.

Event-based communication for player health and score.

Timers and Spawning logic.

📂 Project Structure
Assets/Scripts/Generator/: Logic for infinite terrain/object spawning.

Assets/Scripts/Player/: Character control and collision detection.

Assets/Scripts/Core/: Game Manager, Score, and UI event listeners.

📝 Learning Log
[x] Basic player movement and lane switching.

[x] Random obstacle and coin spawning.

💡 Key Engineering Challenges
This project has been a great opportunity to learn about:

Memory Management: Learning to use Object Pooling to avoid garbage collection spikes.

Decoupling: Using OnPlayerDeath events to trigger multiple game-over systems without creating messy dependencies.

Timing: Handling spawn rates and game difficulty scaling over time.
