# 2.0.8 - Dependency Updates & Stability
* **Dependency Updates**: Updated Jotunn to 2.30.2 and internal dependencies for stability.
* **Stability**: Verified player spawn point calculation across singleplayer, hosted co-op, and dedicated servers.

<details>
<summary><b>2.0 Changelog History (Valheim Release)</b> (<i>click to expand</i>)</summary>

### 2.0.7 - Dedicated Server Spawn Point Collision Fix
* **Dedicated Server Spawn Collision Fix**: Fixed an issue where multiple players connecting to a dedicated server would spawn on top of each other at identical coordinates.
* **Valheim 1.0.15 Alignment**: Updated game assembly references and internalized `Vapok.Valheim.Common` 3.13.1015.

### 2.0.6 - Splash Window Updates & Valheim 1.0.14 Alignment
* **Splash Window Updates**:
  * Telemetry is now unchecked when first loaded (Opt-In visibility)
  * Added Send Error Logs (Opt-Out)
  * Privacy Policy is now available directly in-game
  * Added Data Disclaimers on hover over checkboxes for transparency on what data is sent
* **Valheim 1.0.14 Alignment**: Updated game assembly references and internalized Vapok.Common 3.12.1014.


### 2.0.5 - Jewelcrafting Font Compatibility
* Fixed: Jewelcrafting packages it's own font which was overriding part of a vanilla font, causing the Splash screen to appear blank.
### 2.0.4 - Updated README with Telemetry Information
* Updated the README.md with Anonymous Telemetry information per request of mod stores.

### 2.0.3 - Unified Splash Screen & Telemetry Controls
* **Unified Startup Splash Screen**: Integrated with a centralized startup splash screen.
  * Added configurable `Show on Game Startup` which can be enabled or disabled in the configuration file.
* **Anonymous Telemetry**: 
  * Added configurable `Enable Anonymous Telemetry` configuration which can be enabled or disabled in the configuration file.
    * Defaults to enabled with auto-opt-in on launch. Uncheck to Opt-Out
    * ANONYMOUS DATA ONLY - I track version number and usage data. No personal data is ever collected. For more information, see the [Privacy Policy](https://vapok.io/privacy-policy/).

### 2.0.1 - Dependency & Compatibility Maintenance
* **Dependency Updates**: Updated Jotunn and BepInEx runtime package bindings.
* **Compatibility Maintenance**: Verified compatibility against the latest Valheim 1.0 release.
* **Documentation Improvements**: Standardized README, user guides, and technical patch documentation.

### 2.0.0 - Valheim 1.0 Release & Core Modernization
* Updated codebase and assembly references for Valheim 1.0.
* Rebuilt on .NET Framework 4.8 with updated Jotunn and Vapok.Valheim.Common shared libraries.
* Improved player spawn bounds safety and terrain collision detection.

</details>

<details>
<summary><b>1.0 Changelog History (Valheim Early Access)</b> (<i>click to expand</i>)</summary>

### 1.0.0 - Initial Release of RandomSpawnPointBruh
* Initial release providing configurable spawn point options for new players entering the world.
* Added support for Randomized Spawn Radius, Static Coordinates, and Vanilla spawn location.
* Added dedicated server configuration synchronization via ServerSync.

</details>
