<div align="center">

# 📍 Random Spawn Point *Bruh!*

### *Customized player spawn positioning and randomized world origins for Valheim.*

[![GitHub Release](https://img.shields.io/github/v/release/Vapok/RandomSpawnPointBruh?include_prereleases&logo=github&style=for-the-badge)](https://github.com/Vapok/RandomSpawnPointBruh/releases)
[![Thunderstore Version](https://img.shields.io/thunderstore/v/Vapok/RandomSpawnPointBruh?logo=thunderstore&style=for-the-badge)](https://thunderstore.io/c/valheim/p/Vapok/RandomSpawnPointBruh/)
[![Nexus Mods](https://img.shields.io/badge/Nexus_Mods-Available-da8e35?logo=nexusmods&style=for-the-badge)](https://www.nexusmods.com/valheim/mods/2544)
<br>
[![Discord](https://img.shields.io/badge/Discord-Join%20Community-7289da?logo=discord&logoColor=white&style=for-the-badge)](https://discord.gg/5YAJkRFBXt)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](https://opensource.org/licenses/MIT)

---

</div>

Looking to shake up your Valheim world starts or establish a custom spawn location for your multiplayer server? **Random Spawn Point Bruh!** gives server admins and world creators full control over where new Vikings begin their journey.

---

<div align="center">

<br>

[![Survival Servers](https://raw.githubusercontent.com/Vapok/RandomSpawnPointBruh/main/images/survivalservers_banner.png)](https://www.survivalservers.com/services/game_servers/valheim/?ref=vapok)

</div>

## 🧭 Spawn Modes

| Spawn Mode | Description |
| :--- | :--- |
| **🎲 Random Spawn** | Dynamically places new players within a configurable radius, search range, and selected biome. Perfect for survival challenges and rogue-like gameplay! |
| **📌 Static Coordinate Spawn** | Sets a fixed `(X, Y, Z)` coordinate for all new players. Ideal for dedicated servers with custom hub towns or starting bases! |
| **🏛️ Vanilla Spawn** | Retains the traditional sacrificial stones circle at the center of the world. |

---

## ⚙️ Configuration & Settings

Configure via the in-game [BepInEx Configuration Manager](https://github.com/BepInEx/BepInEx.ConfigurationManager) (<kbd>F1</kbd>) or in `RandomSpawnPointBruh.cfg`:

| Setting | Default | Description |
| :--- | :--- | :--- |
| **Spawn Method** | `Random` | Selects between `Random`, `Static`, or `Vanilla` spawn logic. |
| **Custom Spawn Point** | `(0, 0, 0)` | Specific `(X, Y, Z)` coordinates when using `Static` spawn mode. |
| **Min Search Range** | `100.0` | Minimum distance from world center to search for spawn locations. |
| **Max Search Range** | `2000.0` | Maximum distance from world center to search for spawn locations. |
| **Range Increment** | `100` | Step distance when scanning for valid ground positions. |
| **Spawn Biome** | `Meadows` | Allowed biome(s) where random spawn locations can be generated. |

---

## 🛡️ Advanced Safeguards

* ⛰️ **Heightmap & Collision Safety**: Performs automatic terrain collision queries to guarantee players never spawn inside geometry, beneath terrain meshes, or underwater.
* 🌐 **ServerSync Enforced**: On dedicated servers, spawn coordinates and modes are dictated by the server configuration to ensure a consistent experience for all incoming players.

---

## 🌐 Available Translations

<div align="center">

🇺🇸 **English** (Default)

</div>

*Want to help translate Random Spawn Point Bruh? Community translations are welcome! Please submit a PR on [GitHub](https://github.com/Vapok/RandomSpawnPointBruh) or stop by our [Discord](https://discord.gg/5YAJkRFBXt).*

---

## 📥 Installation & Server Setup

### Mod Manager (Recommended)
1. Install via **R2ModMan** or **Thunderstore Mod Manager**.
2. Dependencies (`BepInExPack`, `Jotunn (JVL)`) are installed automatically.

### Dedicated Server Requirement
* **Both Client & Server Required**: Random Spawn Point Bruh must be installed on the dedicated server and connecting clients to properly enforce custom spawn locations.

---

<div align="center">

### 👨‍💻 Created by Vapok Gaming

[![Vapok Gaming](https://avatars.githubusercontent.com/u/1264136?s=120&v=4)](https://github.com/Vapok)

**Author**: [Vapok](https://github.com/Vapok)  
**Source Code**: [GitHub Repository](https://github.com/Vapok/RandomSpawnPointBruh)  
**Community & Support**: [Discord Server](https://discord.gg/5YAJkRFBXt)  
**Changelog**: [Release Notes](https://github.com/Vapok/RandomSpawnPointBruh/blob/main/CHANGELOG.md)

</div>
