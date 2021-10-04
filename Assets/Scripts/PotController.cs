using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class PotController : MonoBehaviour
{

    [SerializeField] private Image _firstGlif;
    [SerializeField] private Image _secondGlif;
    [SerializeField] private Image _thirdGlif;
    [SerializeField] private FuelConfig _fuelConfig;
    
    private RocketController _rocketController;
    private List<Soul> _souls = new List<Soul>(3);

    private void Start()
    {
        Clear();

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

    public void SetRocket(RocketController rocketController)
    {
        _rocketController = rocketController;
    }

    public bool SetSoul(Soul soul)
    {
        if (_souls.Count >= 3) return false;

        AudioManager.Play("Splash");

        _souls.Add(soul);
        SetGlifs();

        return true;
    }

    public void SetGlifs()
    {
        if(_souls.Count >= 1)
        {
            _firstGlif.gameObject.SetActive(true);
            _firstGlif.sprite = _souls[0].SoulConfig.Glife;
        }
        else
        {
            _firstGlif.gameObject.SetActive(false);
        }

        if (_souls.Count >= 2)
        {
            _secondGlif.gameObject.SetActive(true);
            _secondGlif.sprite = _souls[1].SoulConfig.Glife;
        }
        else
        {
            _secondGlif.gameObject.SetActive(false);
        }

        if (_souls.Count >= 3)
        {
            _thirdGlif.gameObject.SetActive(true);
            _thirdGlif.sprite = _souls[2].SoulConfig.Glife;
        }
        else
        {
            _thirdGlif.gameObject.SetActive(false);
        }
    }

    private FuelType CookFuel()
    {
        var sins = _fuelConfig.GetSins();
        var fuel = FuelType.worst;

        if(_souls[0].SoulConfig.SinType == sins[0])
        {
            fuel = FuelType.bad;

            if (_souls[1].SoulConfig.SinType == sins[1])
            {
                fuel = FuelType.normal;

                if (_souls[2].SoulConfig.SinType == sins[2])
                {
                    fuel = FuelType.excellent;
                }
            }
        }

        return fuel;
    }

    public void Clear()
    {
        _souls.Clear();
        SetGlifs();
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

