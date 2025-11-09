# 1.1.2 - Improved Spawn Point Detection - Ashlands / Deep North
* Adds Boundary starts for Ashland and Deep North to speed up finding a random spawn point.
* Checks for Lava when looking for Ashland spawn points.
* Reduced Console and Log noise.
  * Debug logging is still chatty
* No longer return boundary point near a biome edge.
  * This prevents a Meadow Random Spawn Point from being right next to Plains and being stung to death.

# 1.1.1 - Fixing Dedicated Server Config Syncing
* A regression issue was introduced when switching to Jotunn preventing servers from dictating configs to clients.
  * This has been resolved.
* Appropriately added the BepInDependency Flags for graceful mod exit if missing dependencies.

# 1.1.0 - Removed ServerSync - Updated to Jotunn
* Updated for Valheim 0.221.4

# 1.0.4 - Mod Compatibilities
* Compatible with MultiplayerTweaks
* Compatible with ServerCharacters

# 1.0.3 - Update to 0.217.28
* Updates for Valheim 0.217.28

# 1.0.2 - Update to 0.217.24
* Updates for Valheim 0.217.24

# 1.0.1 - Bug Fix
* Fixing Error that occurs when starting Valheim if the mod is disabled.

# 1.0.0 - Initial Version of RandomSpawnPointBruh
* Provides settings to adjust the spawn points:
  * Randomize the initial Spawn Point of Players
    * Full control over the variables to adjust the randomization.
  * Set a Static Spawn Point coordinate
  * Use the Vanilla Spawn Point.
* Configs are Server Synced to allow admins to set settings.