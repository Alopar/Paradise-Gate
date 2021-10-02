using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fuel", menuName = "Fuel")]
public class FuelConfig : ScriptableObject
{
    [SerializeField] private SinType _firstSin;
    [SerializeField] private SinType _secondSin;
    [SerializeField] private SinType _thirdSin;

    public SinType[] GetSins()
    {
        var sins = new SinType[3];
        sins[0] = _firstSin;
        sins[1] = _secondSin;
        sins[2] = _thirdSin;

        return sins;
    }
}