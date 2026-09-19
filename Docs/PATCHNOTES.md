# 2.0.7 - Dedicated Server Spawn Point Collision Fix
* **Deterministic Pseudo-Random Generation Fix**:
  * Resolved an issue where multiple players joining a dedicated server would spawn on top of each other at identical coordinates.
  * Root Cause: Valheim's world generation routines (`AltBiomeWorldData.GenerateAltBiomes()` and `WorldGenerator`) invoke `UnityEngine.Random.InitState(...)` with the world seed upon client connection and do not restore the previous RNG state. Because `SpawnPointGenerator` relied on `UnityEngine.Random`, all connecting clients shared the exact same initial RNG state, producing identical coordinate sequences across clients.
  * Resolution: Removed `using Random = UnityEngine.Random;` and migrated `SpawnPointGenerator.GetRandomPointInBiome` to a local `System.Random` instance seeded uniquely per call with `Guid.NewGuid().GetHashCode()`.
  * Implemented non-deterministic polar coordinate calculations with uniform area distribution ($\theta \in [0, 2\pi)$, $\text{mag} = \sqrt{u}$) ensuring distinct, non-colliding random spawn locations for every player.

# 2.0.6 - Splash Window Updates & Valheim 1.0.14 Alignment
* **Splash Window Updates**:
  * Updated telemetry default to unchecked on first launch (Opt-In).
  * Added Send Error Logs toggle (Opt-Out) to capture anonymous crash diagnostics and error reports.
  * Added in-game scrollable Privacy Policy overlay with responsive mouse wheel support.
  * Added interactive tooltip data disclaimers on checkbox hover.
* **Valheim 1.0.14 Alignment**:
  * Aligned publicized game assembly and UnityEngine references to Valheim 1.0.14.
  * Updated internalized Vapok.Common dependency to 3.12.1014.

# 2.0.5 - Jewelcrafting Font Compatibility
* **Compatibility Fix**: Fixed issue where Jewelcrafting packages its own font which was overriding part of a vanilla font, causing the Splash screen to appear blank.
* **Vapok.Common Dependency Bump**: Updated internalized dependency to `Vapok.Valheim.Common` 3.11.1012.

# 2.0.4 - Updated README with Telemetry Information
* **Documentation Update**: Updated the README.md with Anonymous Telemetry and Privacy section per request of mod stores.
* **Vapok.Common Dependency Bump**: Updated internalized dependency to `Vapok.Valheim.Common` 3.9.1012.

# 2.0.3 - Unified Splash Screen & Telemetry Controls
* **Unified Startup Splash Screen & Telemetry**:
  * Updated `Vapok.Valheim.Common` dependency reference to `v3.5.1012`.
  * Registered mod metadata with centralized `ModSplashManager`.
  * Added `ShowSplashOnStartup` and `Enable Anonymous Telemetry` configuration bindings to `ConfigRegistry`.

# 2.0.1 - Dependency & Compatibility Maintenance
* **Runtime & Dependency Updates**:
  * Synchronized package manifest and project references with Jotunn `2.30.0` and BepInEx `5.4.2350`.
  * Verified build pipeline and ILRepack bundling with `Vapok.Valheim.Common` `3.2.1012`.
* **Compatibility & Documentation**:
  * Validated spawn location selection hooks and dedicated server synchronization against current Valheim 1.0 builds.
  * Standardized mod documentation, changelog tiers, and release staging.

# 2.0.0 - Valheim 1.0 Release & Core Modernization
* **Valheim 1.0 Compatibility**:
  * Updated assembly references for Valheim 1.0 (`1.0.12`), BepInEx 5.4.2350, and Jotunn 2.30.0.
  * Rebuilt on .NET Framework 4.8.
  * Bundled `Vapok.Valheim.Common` 3.2.1012 via ILRepack.
* **Spawn Intercept Patches**:
  * Updated Harmony patches intercepting player world initialization and initial spawn point determination (`Player.SetCustomSpawnPoint`, `Game.FindSpawnPoint`).
  * Ensured safe coordinate bounds validation and heightmap terrain collision resolution to prevent players from spawning beneath terrain.
  * Synchronized spawn configuration rules via Jotunn ServerSync across dedicated servers.

# 1.0.0 - Initial Release of RandomSpawnPointBruh
* Initial release of configurable player spawn positioning mechanics.
* Added support for Random Spawn Radius, Static Vector3 Coordinates, and Vanilla spawn behavior.
* Implemented ServerSync enforcement for multiplayer dedicated servers.
