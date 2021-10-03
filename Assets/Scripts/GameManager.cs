using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] private Transform _rocketSpawnPlace;    
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private PotController _potController;    
    [SerializeField] private List<SoulSpawnerController> _soulSpawnerControllers;
    [SerializeField] private List<SoulConfig> _soulConfigs;

    [SerializeField] private RocketController _rockerPrefab;

    [SerializeField] private Transform _heavenPoint;
    [SerializeField] private Transform _normalPoint;
    [SerializeField] private Transform _badPoint;
    [SerializeField] private Transform _worstPoint;

    private RocketController _rocketController;
    private List<SoulConfig> _currentSoulConfigs;

    void Start()
    {
        SpawnSouls();
        GameUIManager.OnClearPot += ClearPotHandler;

        _rocketController = FindObjectOfType<RocketController>();
        _rocketController.SetFlyPoint(_heavenPoint, _normalPoint, _badPoint, _worstPoint);
        _rocketController.OnExplosion += RocketExplosionHandler;

        _cameraController.SetRocket(_rocketController);
    }

    private void RocketExplosionHandler()
    {
        _rocketController.OnExplosion -= RocketExplosionHandler;

        _rocketController = Instantiate(_rockerPrefab, _rocketSpawnPlace.transform.position, _rocketSpawnPlace.transform.rotation);
        _rocketController.SetFlyPoint(_heavenPoint, _normalPoint, _badPoint, _worstPoint);
        _rocketController.OnExplosion += RocketExplosionHandler;

        _cameraController.SetRocket(_rocketController);

        _potController.Clear();
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
