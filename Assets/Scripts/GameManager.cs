using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<SoulSpawnerController> _soulSpawnerControllers;
    [SerializeField] private List<SoulConfig> _soulConfigs;

    private List<SoulConfig> _currentSoulConfigs;

    void Start()
    {
        SpawnSouls();
        GameUIManager.OnClearPot += ClearPotHandler;
    }

    private void ClearPotHandler()
    {
        SpawnSouls();
    }

    private void SpawnSouls()
    {
        _currentSoulConfigs = new List<SoulConfig>();
        foreach (var spawn in _soulSpawnerControllers)
        {
            while (true)
            {
                var config = _soulConfigs[Random.Range(0, _soulConfigs.Count)];
                if (_currentSoulConfigs.Find(c => c == config) == null)
                {
                    spawn.SpawnSoul(config);
                    _currentSoulConfigs.Add(config);
                    break;
                }
            }
        }
    }

    private void OnDestroy()
    {
        GameUIManager.OnClearPot -= ClearPotHandler;
    }
}
