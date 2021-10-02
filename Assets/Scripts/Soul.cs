using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    private SoulConfig _soulConfig;

    public Soul(SoulConfig soulConfig)
    {
        _soulConfig = soulConfig;
    }

    public SoulConfig SoulConfig => _soulConfig;
}
