using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoulController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Soul _soul;
    private Vector3 _spawnPosition;

    private Camera _camera;
    private float _mouseZ;

    void Start()
    {
        _camera = Camera.main;
        GetComponentInChildren<SpriteRenderer>().sprite = _soul.SoulConfig.Sprite;

        GameUIManager.OnClearPot += ClearPotHandler;
    }

    private void ClearPotHandler()
    {
        Destroy(gameObject);
    }

    public void SetSoul(Soul soul) => _soul = soul;
    public void SetSpawnPosition(Vector3 position) => _spawnPosition = position;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _mouseZ = _camera.WorldToScreenPoint(transform.position).z;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var screenPosition = new Vector3(eventData.position.x, eventData.position.y, _mouseZ);
        var worldPosition = _camera.ScreenToWorldPoint(screenPosition);
        transform.position = new Vector3(worldPosition.x , Mathf.Clamp(worldPosition.y, _spawnPosition.y, _spawnPosition.y + 5), _spawnPosition.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var ray = _camera.ScreenPointToRay(eventData.position);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        foreach (var hit in hits)
        {
            var pot = hit.collider.GetComponent<PotController>();
            if(pot != null)
            {
                if (pot.SetSoul(_soul))
                {
                    Destroy(gameObject);
                    return;
                }
            }
        }

        transform.position = _spawnPosition;
    }

    private void OnDestroy()
    {
        GameUIManager.OnClearPot -= ClearPotHandler;
    }
}

public enum SinType
{
    pride,
    greed,
    lust,
    envy,
    gluttony,
    wrath,
    sloth
}
