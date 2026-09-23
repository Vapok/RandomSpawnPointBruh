# 2.1.1 - Configuration Sync & Stability Updates
* **Library Configuration Synchronization (`Vapok.Valheim.Common`)**:
  * Updated internalized `Vapok.Valheim.Common` to 3.21.1015.
  * Resolves dedicated server issue where admin-only synchronized configurations were stuck in `ReadOnly = true` mode, preventing authorized server admins from editing mod settings in `ConfigDrawers`.
  * Synchronizes admin status immediately upon receiving `ZNet.RPC_AdminList`.

# 2.1.0 - Special POI Protection, Dynamic Biome Ranging & Biome Starting Kits
* **Biome Starting Kits & Auto-Provisions**:
  * Implemented `BiomeStartingKit` configuration and `StartingKitManager` component to deliver customizable equipment kits based on touchdown biome (`Heightmap.Biome`).
  * Added server-synced settings under `[Starting Kits (Synced)]`: `Enable Starting Kits` (default false), `Clear Vanilla Starting Items` (default true), `Auto-Equip Gear` (default true), and `Auto-Consume Foods and Meads` (default true).
  * Implemented coroutine-managed touchdown evaluation in `StartingKitManager`: queued natively on character creation via `Humanoid.GiveDefaultItems` without touching player `m_customData`. Defers item grant and consumption until `!Game.instance.WaitingForRespawn()`, `!player.InIntro()`, `!Hud.instance.m_loadingScreen.gameObject.activeInHierarchy`, and character is grounded. Prevents premature consumption or status effect loss during intro skip respawn cycles, and inherently prevents re-awarding on subsequent deaths, respawns, or re-joins.
  * Added dedicated server and headless execution guards (`GUIManager.IsHeadless()`) to prevent UI, console commands, or coroutine execution on headless runtimes.
  * Added targeted Harmony prefix on `Humanoid.GiveDefaultItem` to suppress only vanilla default items (`ArmorRagsLegs`, `ArmorRagsChest`, `Torch`) on new player profiles, fully coexisting with other mods that hook or append default items.
  * Targeted item removal on landing cleans up vanilla rags/torch without clearing third-party mod inventory items.
  * Auto-consume dynamically detects all consumables (`m_itemType == Consumable`), drinking meads/potions and eating foods via vanilla `Player.ConsumeItem` upon touchdown.
* **Special POI Protection & Configurable Buffer**:
  * Implemented `SpecialPoiBufferDistance` setting (`50f` to `500f`, default `100f`, synced).
  * In `SpawnPointGenerator`, implemented `GetSpecialPois()` and `IsNearSpecialPoi(...)` querying `ZoneSystem.instance.m_locationInstances` for locations where `m_location.m_iconPlaced == true` or `m_location.m_unique == true` (Haldor `Vendor_BlackForest`, Hildir `Hildir_camp`, Bog Witch `BogWitch_Camp`).
  * Enforced an exclusion barrier: $\text{Barrier} = \text{Base POI Reveal Radius } (500\text{m}) + \text{Valkyrie Flight Radius } (500\text{m}) + \text{SpecialPoiBufferDistance}$.
  * Guarded against premature evaluation during world generation by verifying `ZoneSystem.instance.LocationsGenerated` before generating candidate points and caching discovered locations.
* **Valkyrie Flight Minimap Exploration Suppression**:
  * Added Harmony prefix patch on `Minimap.UpdateExplore` (`Patches/Minimap.cs`) to return `false` while `player.InIntro()` is active, preventing fog-of-war uncovering along the flight corridor until touchdown.
* **Dynamic Biome Distance Ranging & Mathematical Overhaul**:
  * Replaced legacy sequential search radius stepping with direct mathematical evaluation via `WorldGenerator.instance` (`GetBiome`, `GetBiomeArea`, `GetHeight`).
  * Added `GetBiomeRangeBounds` defining native biome radii bands across all biomes, with polar hemisphere angular clamping for Ashlands ($\sin < 0$) and Deep North ($\sin > 0$).
* **Terrain, Water & Elevation Verification**:
  * Added `MinAltitudeAboveWater` configuration setting to prevent waterline, surf, or marshland submergence.
  * Tightened `SolidHeightTolerance` to $1.5\text{m}$ to ensure solid surface alignment.
* **Player & Base Separation Safeguards**:
  * Added `PlayerSeparationDistance` configuration checking `EffectArea.Type.PlayerBase`, `PrivateArea.m_allAreas` (wards), and active players via `Player.GetAllPlayers()`.
* **Default & Biome-Specific Starting Kit Controls**:
  * Added `UseBiomeSpecificStartingKit` configuration setting to toggle between landing-biome kits and a global default kit.
  * Added `DefaultStarterKit` setting to designate the fallback or global starting loadout.
* **Dependency & Framework Updates**:
  * Updated internalized `Vapok.Valheim.Common` to 3.19.1015.
  * Updated `JotunnLib` dependency to 2.30.2.

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
