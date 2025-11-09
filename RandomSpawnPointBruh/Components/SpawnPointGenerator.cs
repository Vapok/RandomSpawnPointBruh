using System;
using System.Linq;
using RandomSpawnPointBruh.Configuration;
using UnityEngine;
using Vapok.Common.Tools;
using Random = UnityEngine.Random;

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

    private static bool _pointFound = false;
    private static bool _stopWaiting = false;
    private static bool _timersStarted = false;
    private static bool _timerFirstRun = false;
    private static Vector3 _newSpawnPoint = Vector3.zero;
    private static RepeatingTimer _spawnTimelimit;
    private static RepeatingTimer _logTimer;

    public static bool GetSpawnPoint(string startLocation, out Vector3 pos)
    {
        var startTemple = ZoneSystem.instance.GetLocationIcon(startLocation, out var originalPosition);

        if (!ConfigRegistry.Enabled.Value)
        {
            pos = originalPosition;
            RandomSpawnPointBruh.Log.Debug($"Random Spawn Point Bruh! - Disabled");
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
                if (_pointFound)
                {
                    RandomSpawnPointBruh.Log.Debug($"pointfound: {_pointFound} | newSpawnPoint: {_newSpawnPoint}");
                    
                    pos = new Vector3(_newSpawnPoint.x,_newSpawnPoint.y,_newSpawnPoint.z);
                    Game.instance.m_playerProfile.SetHomePoint(pos);
                    _newSpawnPoint = Vector3.zero;
                    _pointFound = false;
                    return true;
                }

                break;
        }

        _newSpawnPoint = Game.instance.m_playerProfile.GetHomePoint();
        
        //Spawn Point Already Set
        if (_newSpawnPoint != Vector3.zero)
        {
            pos = new Vector3(_newSpawnPoint.x,_newSpawnPoint.y,_newSpawnPoint.z);
            _newSpawnPoint = Vector3.zero;
            _pointFound = false;
            return true;
        }

        //Start Spawn Generation
        RandomSpawnPointBruh.Log.Debug($"Spawn Method: {ConfigRegistry.SpawnMethod.Value}");
        RandomSpawnPointBruh.Log.Debug($"Home Point: {Game.instance.m_playerProfile.GetHomePoint()}");

        
        if (GetRandomPointInBiome(GetBiome(ConfigRegistry.SpawnBiome.Value), out var randomPoint))
        {
            pos = randomPoint;
            return true;
        }
        
        pos = originalPosition;
        return startTemple;
    }

    private static bool GetRandomPointInBiome(Heightmap.Biome biome, out Vector3 customSpawnPoint)
    {
        if (_pointFound)
        {
            customSpawnPoint = _newSpawnPoint;
            return true;
        }
        var maxRangeIncreases = ConfigRegistry.MaxRangeIncreases.Value;
        var maxPointsInRange = ConfigRegistry.MaxPointsInRange.Value;

        var minSearchRange = ConfigRegistry.MinSearchRange.Value;
        var maxSearchRange = ConfigRegistry.MaxSearchRange.Value;
        var stop = false;
        
        var rangeTries = 0;
        var randomMinRange = Random.Range(minSearchRange, maxSearchRange/2);
        var randomMaxRange = Random.Range(randomMinRange + ConfigRegistry.RangeSeparationFactor.Value, maxSearchRange);
        var radiusRange = new Tuple<float, float>(randomMinRange, randomMaxRange);

        while (rangeTries < maxRangeIncreases || stop)
        {
            rangeTries++;

            var tries = 0;
            while (tries < maxPointsInRange || stop)
            {
                tries++;

                var randomPoint = Random.insideUnitCircle;
                var mag = randomPoint.magnitude;
                var normalized = randomPoint.normalized;
                var actualMag = Mathf.Lerp(radiusRange.Item1, radiusRange.Item2, mag);
                randomPoint = normalized * actualMag;
                var spawnPoint = new Vector3(randomPoint.x, 0, randomPoint.y);

                if (biome == Heightmap.Biome.AshLands && spawnPoint.z > ConfigRegistry.AshlandsStart.Value)
                {
                    continue;
                }

                if (biome == Heightmap.Biome.DeepNorth && spawnPoint.z < ConfigRegistry.DeepNorthStart.Value)
                {
                    continue;
                }

                var zoneId = ZoneSystem.GetZone(spawnPoint);
                
                RandomSpawnPointBruh.Log.Info($"Spawning Zone {zoneId} for Point {spawnPoint} - This might take a moment.");

                _stopWaiting = false;
                _timersStarted = false;
                _timerFirstRun = false;

                _spawnTimelimit = new RepeatingTimer(TimeSpan.FromSeconds(5),() =>
                {
                    if (!_timersStarted) return;
                    if (!_timerFirstRun)
                    {
                        _timerFirstRun = true;
                        return;
                    }
                            
                    RandomSpawnPointBruh.Log.Info($"Zone spawn timer exceeded. Cancelling...");
                    _stopWaiting = true;
                            
                });

                _logTimer = new RepeatingTimer(TimeSpan.FromSeconds(1), () =>
                {
                    RandomSpawnPointBruh.Log.Info($"Waiting for Zone to Load...");
                });
                
                
                while (!ZoneSystem.instance.SpawnZone(zoneId, ZoneSystem.SpawnMode.Client, out _) && !_stopWaiting)
                {
                    if (!_timersStarted)
                    {
                        RandomSpawnPointBruh.Log.Debug($"Timers Not Started... Starting...");
                        
                        _logTimer.Start();
                        _spawnTimelimit.Start();
                        _timersStarted = true;
                    }
                }
                _logTimer.Stop();
                _spawnTimelimit.Stop();

                Heightmap.Biome foundBiome = Heightmap.Biome.None;
                Heightmap.BiomeArea biomeArea = Heightmap.BiomeArea.Everything;
                Heightmap hmap = null;

                try
                {
                    ZoneSystem.instance.GetGroundData(ref spawnPoint, out var normal, out foundBiome, out biomeArea, out hmap);
                }
                catch (Exception)
                {
                    RandomSpawnPointBruh.Log.Debug($"Zone Didn't Load: {zoneId}");
                }

                var groundHeight = spawnPoint.y;

                RandomSpawnPointBruh.Log.Debug($"Checking biome at ({randomPoint}): {foundBiome} (try {tries})");
                if (foundBiome != biome)
                {
                    // Wrong biome
                    RandomSpawnPointBruh.Log.Debug($"Wrong Biome: {foundBiome}");
                    continue;
                }

                RandomSpawnPointBruh.Log.Debug($"Checking Biome Area at ({randomPoint}): {foundBiome} (try {tries})");
                if (biomeArea != Heightmap.BiomeArea.Median)
                {
                    // Wrong Biome Area
                    RandomSpawnPointBruh.Log.Debug($"Wrong Biome Area: {biomeArea}");
                    continue;
                }

                //Check for Lava
                if (hmap != null && hmap.IsLava(spawnPoint))
                {
                    RandomSpawnPointBruh.Log.Debug("Spawn Point rejected: is in Lava");
                    continue;
                }


                var solidHeight = ZoneSystem.instance.GetSolidHeight(spawnPoint);
                var offsetFromGround = Math.Abs(solidHeight - groundHeight);
                if (offsetFromGround > 5)
                {
                    // Don't place too high off the ground (on top of tree or something?
                    RandomSpawnPointBruh.Log.Debug($"Spawn Point rejected: too high off of ground (groundHeight:{groundHeight}, solidHeight:{solidHeight})");
                    continue;
                }

                // But also don't place inside rocks
                spawnPoint.y = solidHeight;

                var placedNearPlayerBase = EffectArea.IsPointInsideArea(spawnPoint, EffectArea.Type.PlayerBase, 100f);
                if (placedNearPlayerBase)
                {
                    // Don't place near player base
                    RandomSpawnPointBruh.Log.Debug("Spawn Point rejected: too close to player base");
                    continue;
                }

                RandomSpawnPointBruh.Log.Debug($"Wards: {PrivateArea.m_allAreas.Count}");
                var tooCloseToWard = PrivateArea.m_allAreas.Any(x => x.IsInside(spawnPoint, 100f));
                if (tooCloseToWard)
                {
                    RandomSpawnPointBruh.Log.Debug("Spawn Point rejected: too close to player ward");
                    continue;
                }
                   
                var waterLevel = ZoneSystem.instance.m_waterLevel;
                if (biome != Heightmap.Biome.Ocean && waterLevel > groundHeight + 1.0f)
                {
                    // Too deep, try again
                    RandomSpawnPointBruh.Log.Debug($"Spawn Point rejected: too deep underwater (waterLevel:{waterLevel}, groundHeight:{groundHeight})");
                    continue;
                }

                RandomSpawnPointBruh.Log.Info($"Random Spawn Point Success! (ground={groundHeight} water={waterLevel} placed={spawnPoint})");
                _newSpawnPoint = spawnPoint;
                stop = true;
                _pointFound = true;
                break;
            }

            if (_pointFound)
            {
                break;
            }
            radiusRange = new Tuple<float, float>(radiusRange.Item1 + ConfigRegistry.RangeIncrement.Value, radiusRange.Item2 + ConfigRegistry.RangeIncrement.Value);
        }

        if (_pointFound)
        {
            customSpawnPoint = _newSpawnPoint;
            return true;
        }
        customSpawnPoint = Vector3.zero;
        return false;
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