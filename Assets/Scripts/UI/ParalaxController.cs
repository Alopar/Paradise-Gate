using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxController : MonoBehaviour
{
    [SerializeField] private RectTransform _level0;
    [SerializeField] private RectTransform _level1;
    [SerializeField] private RectTransform _level2;
    [SerializeField] private RectTransform _level3;

    [SerializeField, Space(10), Range(0f, 1f)] private float _distanceDelta;

    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SetDistanceDelta(float delta)
    {
        _distanceDelta = delta;
        SetLevelPosition(_level0);
        SetLevelPosition(_level1);
        SetLevelPosition(_level2);
        SetLevelPosition(_level3);
    }

    private void SetLevelPosition(RectTransform level)
    {
        var maxDeltaY = level.rect.height - _rectTransform.rect.height;

        var baseX = 0;
        var baseY = 0 - _rectTransform.rect.height / 2;
        var currentY = baseY - (maxDeltaY * _distanceDelta);
        level.localPosition = new Vector2(baseX, currentY);
    }
}
