# ShooterGame ![Unity](https://img.shields.io/badge/Engine-Unity-blue) ![AI](https://img.shields.io/badge/AI-Enabled-green) ![License](https://img.shields.io/github/license/UmitOzteker/ShooterGame)

 ![GitHub Total Commits](https://img.shields.io/github/commit-activity/t/UmitOzteker/Labyrinth_Game_Project) 
  ![GitHub Last Commit](https://img.shields.io/github/last-commit/UmitOzteker/Labyrinth_Game_Project) 
  ![GitHub Issues](https://img.shields.io/github/issues/UmitOzteker/Labyrinth_Game_Project) 
  ![Code Size](https://img.shields.io/github/languages/code-size/UmitOzteker/C_Projects)

This project is a **top-down shooter AI system** built with Unity, designed for developers who want to learn about AI movement, automatic shooting mechanics, enemy management, and health bar logic in game development.

---

## Features

- **Player Movement**
  - The player character moves across the map using Unity's NavMeshAgent.
  - Movement is controlled by mouse clicks, making navigation intuitive and responsive.

- **Enemy AI**
  - Enemies spawn randomly on the map at regular intervals.
  - Each enemy is programmed to pursue the player using basic AI pathfinding.

- **Automatic Shooting System**
  - The player automatically detects enemies using raycasting and fires bullets without manual input.
  - Bullets are visualized with trail effects for a dynamic and engaging shooting experience.

- **Health Bar System**
  - Each enemy has a health bar that updates in real-time as it takes damage.
  - Health bars provide immediate feedback to players about enemy status.

- **Visual Effects**
  - Bullets leave trails for improved visual clarity and game feel.
  - Damage indicators and health bar changes make the game more immersive.

---

## Getting Started

### Prerequisites

- Unity 2021 or newer (recommended)
- Basic understanding of C# scripting for Unity

### Installation

1. **Clone the Repository**
   ```sh
   git clone https://github.com/UmitOzteker/ShooterGame.git
   ```
2. **Open in Unity**
   - Launch Unity Hub and open the cloned project.
   - Import all packages if prompted.

3. **Setup**
   - Ensure that scripts like `ShooterController`, `EnemySpawner`, and `HealthBar` are attached to the relevant GameObjects in your scene.
   - Adjust the NavMesh for your level if necessary.

4. **Run the Game**
   - Press Play in the Unity Editor to test player movement, enemy AI, shooting, and health bar systems.

---

## Learning Objectives

This project is ideal for developers who want to learn and experiment with:

- Unity NavMesh for player movement
- Enemy AI pathfinding and behavior
- Raycast-based enemy detection and auto-shooting
- Implementing visual effects (bullet trails, health bars)
- Real-time UI updates for health/status indicators

---

## Contributing

Contributions are welcome! If you find bugs or want to suggest new features, please open an issue or submit a pull request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a pull request

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## Contact

For questions, feedback, or collaboration, open an issue or contact [UmitOzteker](https://github.com/UmitOzteker).

---

> ShooterGame is a learning-focused, top-down shooter template — perfect for exploring AI, shooting mechanics, and game UI systems in Unity.
