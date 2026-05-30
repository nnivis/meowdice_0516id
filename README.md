# Dice Battle

🇷🇺 [Читать на русском](README.ru.md)

A turn-based dice strategy game for two players built in Unity. Roll dice, place them on your 3×3 board, and outscore your opponent.

---

## About

Dice Battle is a portfolio project focused on clean layered architecture in Unity. Players alternate turns — each turn the active player rolls a die (1–6) and places it in a free cell on their 3×3 board. When both boards are filled, column scores are tallied and a winner is declared. Matching dice values in opponent columns trigger special clearing rules, adding a tactical layer on top of luck.

VS AI mode is fully implemented. Local PvP and Online PvP are stubbed for future development.

---

## Features

- Roll a die and place it in any free cell on your 3×3 board
- Fill a column with matching values to clear the opponent's same column
- Column score formula: `value × count²` — combos scale exponentially
- VS AI opponent (random placement strategy)
- Cat skin customization for your character
- Match result screen after every game

---

## Architecture

| Pattern | Implementation |
|---|---|
| Dependency Injection | VContainer — `RootLifetimeScope` / `GameplayLifetimeScope` |
| State Machine | `MainSceneMode` — Main, Gameplay, Result states |
| Layered Design | Domain → Services → Infrastructure, no reverse dependencies |
| Async Game Loop | `GameFlow` / `TurnSystem` — `async/await` + `CancellationToken` |
| Data Layer | `IPersistentData` / `IDataProvider` — JSON via Newtonsoft.Json |

---

## Tech Stack

- **Engine:** Unity 6000.3 LTS
- **Language:** C#
- **DI:** VContainer
- **Serialization:** Newtonsoft.Json

---

## Getting Started

1. Clone the repository
2. Open in Unity 6000.3 or newer
3. Open `Assets/Scenes/BootstrapScene`
4. Install VContainer via Package Manager: `jp.hadashikick.vcontainer`
5. Press Play

---

## Contact

- GitHub: [nnivis](https://github.com/nnivis)
