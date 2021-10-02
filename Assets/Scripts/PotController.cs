using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class PotController : MonoBehaviour
{
    [SerializeField] private RocketController _rocketController;
    [SerializeField] private FuelConfig _fuelConfig;

    private List<Soul> _souls = new List<Soul>(3);

    private void Start()
    {
        GameUIManager.OnClearPot += ClearPotHandler;
        GameUIManager.OnLaunchRocket += LaunchRocketHandler;
    }

    private void LaunchRocketHandler()
    {
        if (_souls.Count < 3) 
        {
            Debug.Log("Need more souls!");
            return;
        }

        _rocketController.Launch(CookFuel());
    }

    private void ClearPotHandler()
    {
        Clear();
    }

    public bool SetSoul(Soul soul)
    {
        if (_souls.Count >= 3) return false;

        _souls.Add(soul);

        return true;
    }

    private FuelType CookFuel()
    {
        var sins = _fuelConfig.GetSins();
        var fuel = FuelType.worst;

        if(_souls[0].SoulConfig.SinType == sins[0])
        {
            fuel = FuelType.bad;
        }

        if (_souls[1].SoulConfig.SinType == sins[1])
        {
            fuel = FuelType.normal;
        }

        if (_souls[2].SoulConfig.SinType == sins[2])
        {
            fuel = FuelType.excellent;
        }

        return fuel;
    }

    public void Clear()
    {
        _souls.Clear();
    }

    private void OnDestroy()
    {
        GameUIManager.OnClearPot -= ClearPotHandler;
        GameUIManager.OnLaunchRocket -= LaunchRocketHandler;
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(PotController))]
    public class PlatformEditor : Editor
    {
        private PotController _target;

        private void OnEnable()
        {
            _target = target as PotController;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(10);
            GUILayout.Label("Sin types");
            foreach (var soul in _target._souls)
            {
                GUILayout.Label(soul.SoulConfig.SinType.ToString());
            }            
        }
    }
#endif
}

