using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Soul", menuName = "Soul")]
public class SoulConfig : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private SinType _sinType;

    public Sprite Sprite => _sprite;
    public SinType SinType => _sinType;
}
