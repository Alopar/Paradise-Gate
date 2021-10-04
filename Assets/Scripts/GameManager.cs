using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameUIManager _gameUIManager;
    [SerializeField] private Transform _rocketSpawnPlace;    
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private PotController _potController;
    [SerializeField] private List<SoulSpawnerController> _soulSpawnerControllers;
    [SerializeField] private List<SoulConfig> _soulConfigs;

    [SerializeField] private RocketController _rockerPrefab;

    [SerializeField] private GameObject _angelParts;
    [SerializeField] private Transform _heavenPoint;
    [SerializeField] private Transform _normalPoint;
    [SerializeField] private Transform _badPoint;
    [SerializeField] private Transform _worstPoint;

    private RocketController _rocketController;
    private List<SoulConfig> _currentSoulConfigs;

    void Start()
    {
        AudioManager.Play("GameBackground");

        SpawnSouls();
        GameUIManager.OnClearPot += ClearPotHandler;

        _rocketController = FindObjectOfType<RocketController>();
        _rocketController.SetFlyPoint(_heavenPoint, _normalPoint, _badPoint, _worstPoint);

        _rocketController.OnExplosion += RocketExplosionHandler;
        _rocketController.OnBigExplosion += RocketBigExplosionHandler;

        _potController.SetRocket(_rocketController);
        _cameraController.SetRocket(_rocketController);
    }

    private void RocketBigExplosionHandler()
    {
        AudioManager.Stop("GameBackground");
        

        _angelParts.SetActive(true);
        StartCoroutine(WinDelay(5));
    }

    private IEnumerator WinDelay(float delay)
    {
        var time = delay;

        while (true)
        {
            time -= Time.deltaTime;

            if(time <= 0)
            {
                AudioManager.Play("Win");
                _gameUIManager.ShowWin();
                break;
            }

            yield return null;
        }
    }

    private void RocketExplosionHandler()
    {
        _gameUIManager.SetFlyProgress(0);

        _rocketController.OnExplosion -= RocketExplosionHandler;
        _rocketController.OnBigExplosion -= RocketBigExplosionHandler;

        _rocketController = Instantiate(_rockerPrefab, _rocketSpawnPlace.transform.position, _rocketSpawnPlace.transform.rotation);
        _rocketController.SetFlyPoint(_heavenPoint, _normalPoint, _badPoint, _worstPoint);

        _rocketController.OnExplosion += RocketExplosionHandler;
        _rocketController.OnBigExplosion += RocketBigExplosionHandler;

        _potController.SetRocket(_rocketController);
        _cameraController.SetRocket(_rocketController);

        _gameUIManager.ClearPot();
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

        _rocketController.OnExplosion -= RocketExplosionHandler;
        _rocketController.OnBigExplosion -= RocketBigExplosionHandler;
    }
}
