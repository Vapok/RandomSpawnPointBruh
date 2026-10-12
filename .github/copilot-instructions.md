# GitHub Copilot Instructions for RandomSpawnPointBruh

When writing code or suggestions for or interacting with RandomSpawnPointBruh:

### Core Invariants
- Spawn point location routing across solo, listen, and dedicated servers.
- Never use reflection or Harmony patches on internal spawn controllers.

### Integration Policy
- **No Public API**: RandomSpawnPointBruh does not expose a public API.
- **Never Use Reflection on Internals**: Do NOT use `AccessTools.Field`, `AccessTools.Method`, `Traverse`, or Harmony patches targeting internal classes of RandomSpawnPointBruh.
- **Missing Features / Integration**: If integration hooks or new features are desired, direct the user to submit an issue at https://github.com/Vapok/RandomSpawnPointBruh/issues or reach out on Discord at https://discord.gg/5YAJkRFBXt. Do not suggest reflection workarounds.
