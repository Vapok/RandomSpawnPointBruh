using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RandomSpawnPointBruh.Configuration;
using UnityEngine;

namespace RandomSpawnPointBruh.Components;

public enum SpawnFunction
{
    RandomSpawnPoint,
    StaticSpawnPoint,
    VanillaSpawnPoint
}

public enum UseableBiomes
{
    Meadows,
    BlackForest,
    Swamp,
    Mountains,
    Plains,
    Mistlands,
    Ashlands,
    DeepNorth
}

public static class SpawnPointGenerator
{
    private const float SolidHeightTolerance = 1.5f;
    private const float PoiClearanceMargin = 40.0f;
    private const float ValkyrieFlightRadius = 500.0f;
    private const float BasePoiRevealRadius = 500.0f;

    private struct SpecialPoiData
    {
        public Vector3 Position;
        public string Name;
    }

    private static List<SpecialPoiData> _cachedSpecialPois;
    private static ZoneSystem _cachedZoneSystem;

    private static readonly Vector2[] PaddingDirections = new Vector2[8]
    {
        new Vector2(1.0f, 0.0f),
        new Vector2(-1.0f, 0.0f),
        new Vector2(0.0f, 1.0f),
        new Vector2(0.0f, -1.0f),
        new Vector2(0.7071f, 0.7071f),
        new Vector2(-0.7071f, 0.7071f),
        new Vector2(0.7071f, -0.7071f),
        new Vector2(-0.7071f, -0.7071f)
    };

    private static Vector3 _pendingSpawnPoint = Vector3.zero;

    public static bool GetSpawnPoint(string startLocation, out Vector3 pos)
    {
        if (ZoneSystem.instance == null || !ZoneSystem.instance.LocationsGenerated)
        {
            RandomSpawnPointBruh.Log.Debug("Waiting for world locations to generate before selecting spawn point...");
            pos = Vector3.zero;
            return false;
        }

        bool startTemple = ZoneSystem.instance.GetLocationIcon(startLocation, out Vector3 originalPosition);

        if (!ConfigRegistry.Enabled.Value)
        {
            pos = originalPosition;
            RandomSpawnPointBruh.Log.Debug("Random Spawn Point Bruh! - Disabled");
            return startTemple;
        }

        switch (ConfigRegistry.SpawnMethod.Value)
        {
            case SpawnFunction.VanillaSpawnPoint:
                pos = originalPosition;
                return startTemple;
            case SpawnFunction.StaticSpawnPoint:
                pos = ConfigRegistry.CustomSpawnPoint.Value;
                return true;
            default:
                break;
        }

        Vector3 homePoint = Game.instance.m_playerProfile.GetHomePoint();
        if (homePoint != Vector3.zero)
        {
            RandomSpawnPointBruh.Log.Debug($"Player home point already set: {homePoint}");
            pos = homePoint;
            return true;
        }

        if (_pendingSpawnPoint != Vector3.zero)
        {
            if (ZNetScene.instance != null && ZNetScene.instance.IsAreaReady(_pendingSpawnPoint))
            {
                if (ValidateLoadedArea(ref _pendingSpawnPoint))
                {
                    pos = _pendingSpawnPoint;
                    _pendingSpawnPoint = Vector3.zero;
                    return true;
                }

                _pendingSpawnPoint = Vector3.zero;
            }
            else
            {
                RandomSpawnPointBruh.Log.Debug($"Waiting for sector around {_pendingSpawnPoint} to load...");
                pos = _pendingSpawnPoint;
                return true;
            }
        }

        RandomSpawnPointBruh.Log.Debug($"Spawn Method: {ConfigRegistry.SpawnMethod.Value}");

        if (GetRandomPointInBiome(GetBiome(ConfigRegistry.SpawnBiome.Value), out Vector3 randomPoint))
        {
            _pendingSpawnPoint = randomPoint;
            pos = _pendingSpawnPoint;
            return true;
        }

        pos = originalPosition;
        return startTemple;
    }

    private static bool ValidateLoadedArea(ref Vector3 candidate)
    {
        if (ZoneSystem.instance == null)
        {
            return true;
        }

        Stopwatch validationTimer = Stopwatch.StartNew();
        RandomSpawnPointBruh.Log.Debug($"Validating loaded sector for candidate {candidate}...");

        ZoneSystem.instance.GetGroundData(ref candidate, out Vector3 normal, out Heightmap.Biome foundBiome, out Heightmap.BiomeArea biomeArea, out Heightmap hmap);

        if (hmap != null && hmap.IsLava(candidate))
        {
            validationTimer.Stop();
            RandomSpawnPointBruh.Log.Debug($"Spawn Point rejected: inside lava ({validationTimer.Elapsed.TotalMilliseconds:F2}ms)");
            return false;
        }

        float solidHeight = ZoneSystem.instance.GetSolidHeight(candidate);
        float offsetFromGround = Math.Abs(solidHeight - candidate.y);
        if (offsetFromGround > SolidHeightTolerance)
        {
            validationTimer.Stop();
            RandomSpawnPointBruh.Log.Debug($"Spawn Point rejected: solid height offset ({offsetFromGround:F1}m) exceeds tolerance ({SolidHeightTolerance:F1}m) ({validationTimer.Elapsed.TotalMilliseconds:F2}ms)");
            return false;
        }

        candidate.y = solidHeight;

        Location activeLocation = Location.GetLocation(candidate, false);
        if (activeLocation != null)
        {
            validationTimer.Stop();
            RandomSpawnPointBruh.Log.Debug($"Spawn Point rejected: inside active POI location ({validationTimer.Elapsed.TotalMilliseconds:F2}ms)");
            return false;
        }

        float specialPoiBuffer = ConfigRegistry.SpecialPoiBufferDistance.Value;
        if (IsNearSpecialPoi(candidate, specialPoiBuffer, out string loadedNearName, out float loadedNearDist, out float loadedReqDist))
        {
            validationTimer.Stop();
            RandomSpawnPointBruh.Log.Debug($"Spawn Point rejected: TRIPPED SPECIAL POI BARRIER in loaded area! Distance: {loadedNearDist:F0}m to {loadedNearName} (Required safe barrier: {loadedReqDist:F0}m) ({validationTimer.Elapsed.TotalMilliseconds:F2}ms)");
            return false;
        }

        float separationDistance = ConfigRegistry.PlayerSeparationDistance.Value;
        if (separationDistance > 0f)
        {
            EffectArea placedNearPlayerBase = EffectArea.IsPointInsideArea(candidate, EffectArea.Type.PlayerBase, separationDistance);
            if (placedNearPlayerBase != null)
            {
                validationTimer.Stop();
                RandomSpawnPointBruh.Log.Debug($"Spawn Point rejected: within {separationDistance:F0}m of a player base ({validationTimer.Elapsed.TotalMilliseconds:F2}ms)");
                return false;
            }

            Vector3 checkPoint = candidate;
            bool tooCloseToWard = PrivateArea.m_allAreas.Any(x => x != null && x.IsInside(checkPoint, separationDistance));
            if (tooCloseToWard)
            {
                validationTimer.Stop();
                RandomSpawnPointBruh.Log.Debug($"Spawn Point rejected: within {separationDistance:F0}m of a player ward ({validationTimer.Elapsed.TotalMilliseconds:F2}ms)");
                return false;
            }

            bool tooCloseToActivePlayer = Player.GetAllPlayers().Any(p => p != null && Vector3.Distance(p.transform.position, checkPoint) < separationDistance);
            if (tooCloseToActivePlayer)
            {
                validationTimer.Stop();
                RandomSpawnPointBruh.Log.Debug($"Spawn Point rejected: within {separationDistance:F0}m of an active player ({validationTimer.Elapsed.TotalMilliseconds:F2}ms)");
                return false;
            }
        }

        float waterLevel = ZoneSystem.instance.m_waterLevel;
        float minDryElevation = (foundBiome == Heightmap.Biome.Swamp) ? (waterLevel + 0.5f) : (waterLevel + ConfigRegistry.MinAltitudeAboveWater.Value);
        if (foundBiome != Heightmap.Biome.Ocean && candidate.y < minDryElevation)
        {
            validationTimer.Stop();
            RandomSpawnPointBruh.Log.Debug($"Spawn Point rejected: altitude {candidate.y:F1}m below dry elevation threshold {minDryElevation:F1}m ({validationTimer.Elapsed.TotalMilliseconds:F2}ms)");
            return false;
        }

        validationTimer.Stop();
        RandomSpawnPointBruh.Log.Debug($"Random Spawn Point Verified at {candidate} in {validationTimer.Elapsed.TotalMilliseconds:F2}ms (SolidHeight={solidHeight:F1})");
        return true;
    }

    private static bool GetRandomPointInBiome(Heightmap.Biome biome, out Vector3 customSpawnPoint)
    {
        int maxSearchAttempts = ConfigRegistry.MaxSearchAttempts.Value;
        float configuredMin = ConfigRegistry.MinSearchRange.Value;
        float paddingDistance = ConfigRegistry.BiomePaddingDistance.Value;
        float minAltitudeAboveWater = ConfigRegistry.MinAltitudeAboveWater.Value;

        GetBiomeRangeBounds(biome, out float naturalMin, out float naturalMax);

        float effectiveMin = Mathf.Max(configuredMin, naturalMin);
        float effectiveMax = naturalMax;

        if (effectiveMin >= effectiveMax)
        {
            effectiveMin = naturalMin;
        }

        Stopwatch searchTimer = Stopwatch.StartNew();

        RandomSpawnPointBruh.Log.Debug($"Initiating random spawn search: Biome={biome}, EffectiveRange=[{effectiveMin:F0}m - {effectiveMax:F0}m], MinAltitude=+{minAltitudeAboveWater:F1}m, Padding={paddingDistance:F0}m, MaxAttempts={maxSearchAttempts}");

        System.Random rng = new System.Random(Guid.NewGuid().GetHashCode());

        for (int attempt = 1; attempt <= maxSearchAttempts; attempt++)
        {
            double angle;
            if (biome == Heightmap.Biome.AshLands)
            {
                angle = Math.PI + (rng.NextDouble() * Math.PI);
            }
            else if (biome == Heightmap.Biome.DeepNorth)
            {
                angle = rng.NextDouble() * Math.PI;
            }
            else
            {
                angle = rng.NextDouble() * Math.PI * 2.0;
            }

            Vector2 dir = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            float mag = (float)Math.Sqrt(rng.NextDouble());
            float actualRadius = Mathf.Lerp(effectiveMin, effectiveMax, mag);
            Vector2 randomPoint = dir * actualRadius;
            Vector3 spawnPoint = new Vector3(randomPoint.x, 0.0f, randomPoint.y);

            if (WorldGenerator.instance == null)
            {
                continue;
            }

            Heightmap.Biome foundBiome = WorldGenerator.instance.GetBiome(spawnPoint.x, spawnPoint.z);
            if (foundBiome != biome)
            {
                RandomSpawnPointBruh.Log.Debug($"Point {spawnPoint} (attempt {attempt}): Biome {foundBiome} != {biome}");
                continue;
            }

            Vector2s zoneId = ZoneSystem.GetZone(spawnPoint);
            Heightmap.BiomeArea biomeArea = WorldGenerator.instance.GetBiomeArea(zoneId);
            if (biomeArea != Heightmap.BiomeArea.Median)
            {
                RandomSpawnPointBruh.Log.Debug($"Point {spawnPoint} (attempt {attempt}): BiomeArea {biomeArea} != Median");
                continue;
            }

            if (paddingDistance > 0.0f && !IsBiomePadded(spawnPoint, biome, paddingDistance))
            {
                RandomSpawnPointBruh.Log.Debug($"Point {spawnPoint} (attempt {attempt}): Encroaches on neighboring biome within {paddingDistance:F0}m padding");
                continue;
            }

            if (ZoneSystem.instance != null && ZoneSystem.instance.m_locationInstances.TryGetValue(zoneId, out ZoneSystem.LocationInstance locationInstance))
            {
                float poiDistance = Vector3.Distance(spawnPoint, locationInstance.m_position);
                float safeRadius = (locationInstance.m_location != null ? locationInstance.m_location.m_interiorRadius : 30.0f) + PoiClearanceMargin;
                if (poiDistance < safeRadius)
                {
                    RandomSpawnPointBruh.Log.Debug($"Point {spawnPoint} (attempt {attempt}): Near POI (dist={poiDistance:F0}m, safeRadius={safeRadius:F0}m)");
                    continue;
                }
            }

            float specialPoiBuffer = ConfigRegistry.SpecialPoiBufferDistance.Value;
            if (IsNearSpecialPoi(spawnPoint, specialPoiBuffer, out string trippedPoiName, out float trippedPoiDist, out float requiredDist))
            {
                RandomSpawnPointBruh.Log.Debug($"Point {spawnPoint} (attempt {attempt}): TRIPPED SPECIAL POI BARRIER! Distance: {trippedPoiDist:F0}m to {trippedPoiName} (Required safe barrier: {requiredDist:F0}m). Rejecting candidate.");
                continue;
            }

            float groundHeight = WorldGenerator.instance.GetHeight(spawnPoint.x, spawnPoint.z);
            float waterLevel = ZoneSystem.instance != null ? ZoneSystem.instance.m_waterLevel : 30.0f;
            float minDryElevation = (biome == Heightmap.Biome.Swamp) ? (waterLevel + 0.5f) : (waterLevel + minAltitudeAboveWater);

            if (biome != Heightmap.Biome.Ocean && groundHeight < minDryElevation)
            {
                RandomSpawnPointBruh.Log.Debug($"Point {spawnPoint} (attempt {attempt}): Elevation {groundHeight:F1}m below dry threshold {minDryElevation:F1}m (waterLevel={waterLevel:F1})");
                continue;
            }

            spawnPoint.y = groundHeight;
            searchTimer.Stop();
            IsNearSpecialPoi(spawnPoint, specialPoiBuffer, out string closestName, out float closestDist, out float safeThreshold);
            float clearance = closestDist - safeThreshold;
            RandomSpawnPointBruh.Log.Debug($"Random Spawn Point candidate selected: {spawnPoint} in {searchTimer.Elapsed.TotalMilliseconds:F2}ms (Evaluated {attempt} points). Closest special POI: {closestName} at {closestDist:F0}m (Safe barrier: {safeThreshold:F0}m, Clearance: +{clearance:F0}m - Sensor PASS)");
            customSpawnPoint = spawnPoint;
            return true;
        }

        searchTimer.Stop();
        RandomSpawnPointBruh.Log.Debug($"Random point search exhausted after {maxSearchAttempts} evaluations in {searchTimer.Elapsed.TotalMilliseconds:F2}ms without finding a candidate");
        customSpawnPoint = Vector3.zero;
        return false;
    }

    private static void GetBiomeRangeBounds(Heightmap.Biome biome, out float naturalMin, out float naturalMax)
    {
        switch (biome)
        {
            case Heightmap.Biome.Meadows:
                naturalMin = 0f;
                naturalMax = 5000f;
                break;
            case Heightmap.Biome.BlackForest:
                naturalMin = 600f;
                naturalMax = 6000f;
                break;
            case Heightmap.Biome.Swamp:
                naturalMin = 2000f;
                naturalMax = 8000f;
                break;
            case Heightmap.Biome.Mountain:
                naturalMin = 1500f;
                naturalMax = 8500f;
                break;
            case Heightmap.Biome.Plains:
                naturalMin = 3000f;
                naturalMax = 8500f;
                break;
            case Heightmap.Biome.Mistlands:
                naturalMin = 5500f;
                naturalMax = 9500f;
                break;
            case Heightmap.Biome.AshLands:
                naturalMin = 7500f;
                naturalMax = 10000f;
                break;
            case Heightmap.Biome.DeepNorth:
                naturalMin = 7500f;
                naturalMax = 10000f;
                break;
            default:
                naturalMin = 0f;
                naturalMax = 10000f;
                break;
        }
    }

    private static List<SpecialPoiData> GetSpecialPois()
    {
        if (ZoneSystem.instance == null || !ZoneSystem.instance.LocationsGenerated || ZoneSystem.instance.m_locationInstances == null)
        {
            return null;
        }

        if (_cachedZoneSystem == ZoneSystem.instance && _cachedSpecialPois != null && _cachedSpecialPois.Count > 0)
        {
            return _cachedSpecialPois;
        }

        _cachedZoneSystem = ZoneSystem.instance;
        List<SpecialPoiData> positions = new List<SpecialPoiData>();

        foreach (ZoneSystem.LocationInstance loc in ZoneSystem.instance.m_locationInstances.Values)
        {
            if (loc.m_location != null && (loc.m_location.m_iconPlaced || loc.m_location.m_unique))
            {
                string poiName = loc.m_location.m_prefab != null ? loc.m_location.m_prefab.Name : "SpecialPOI";
                positions.Add(new SpecialPoiData { Position = loc.m_position, Name = poiName });
            }
        }

        if (positions.Count > 0)
        {
            _cachedSpecialPois = positions;
            RandomSpawnPointBruh.Log.Debug($"Discovered and cached {_cachedSpecialPois.Count} special POI positions for spawn protection");
        }

        return positions;
    }

    private static bool IsNearSpecialPoi(Vector3 candidatePoint, float bufferDistance, out string closestPoiName, out float closestPoiDistance, out float requiredSafeDistance)
    {
        closestPoiName = string.Empty;
        closestPoiDistance = float.MaxValue;
        requiredSafeDistance = 0f;

        List<SpecialPoiData> specialLocations = GetSpecialPois();
        if (specialLocations == null || specialLocations.Count == 0)
        {
            return false;
        }

        float revealRadius = ZoneSystem.instance.m_simulationDistance.TotalSimulationDistance > 0
            ? Mathf.Max(BasePoiRevealRadius, ZoneSystem.instance.m_simulationDistance.TotalSimulationDistance * 64.0f + 51.2f)
            : BasePoiRevealRadius;

        requiredSafeDistance = revealRadius + ValkyrieFlightRadius + bufferDistance;
        float safeDistSqr = requiredSafeDistance * requiredSafeDistance;
        bool tripped = false;

        for (int i = 0; i < specialLocations.Count; i++)
        {
            SpecialPoiData poi = specialLocations[i];
            float dx = candidatePoint.x - poi.Position.x;
            float dz = candidatePoint.z - poi.Position.z;
            float distSqr = dx * dx + dz * dz;

            if (distSqr < safeDistSqr)
            {
                tripped = true;
            }

            float dist = Mathf.Sqrt(distSqr);
            if (dist < closestPoiDistance)
            {
                closestPoiDistance = dist;
                closestPoiName = poi.Name;
            }
        }

        return tripped;
    }

    private static bool IsBiomePadded(Vector3 point, Heightmap.Biome targetBiome, float padding)
    {
        for (int i = 0; i < PaddingDirections.Length; i++)
        {
            float checkX = point.x + PaddingDirections[i].x * padding;
            float checkZ = point.z + PaddingDirections[i].y * padding;
            if (WorldGenerator.instance.GetBiome(checkX, checkZ) != targetBiome)
            {
                return false;
            }
        }

        return true;
    }

    private static Heightmap.Biome GetBiome(UseableBiomes biome)
    {
        switch (biome)
        {
            case UseableBiomes.BlackForest:
                return Heightmap.Biome.BlackForest;
            case UseableBiomes.Swamp:
                return Heightmap.Biome.Swamp;
            case UseableBiomes.Mountains:
                return Heightmap.Biome.Mountain;
            case UseableBiomes.Plains:
                return Heightmap.Biome.Plains;
            case UseableBiomes.Mistlands:
                return Heightmap.Biome.Mistlands;
            case UseableBiomes.Ashlands:
                return Heightmap.Biome.AshLands;
            case UseableBiomes.DeepNorth:
                return Heightmap.Biome.DeepNorth;
        }
        return Heightmap.Biome.Meadows;
    }
}