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
