using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSpawnerController : MonoBehaviour
{
    [SerializeField] private GameObject _placeIndicator;
    [SerializeField] private SoulController _soulPrefab;

    void Start()
    {
        _placeIndicator.SetActive(false);
    }

    public void SpawnSoul(SoulConfig soulConfig)
    {
        var soul = new Soul(soulConfig);
        var soulView = Instantiate(_soulPrefab, transform.position, transform.rotation);
        soulView.SetSoul(soul);
        soulView.SetSpawnPosition(transform.position);
    }
}