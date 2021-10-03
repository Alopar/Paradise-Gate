using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    [SerializeField] private GameObject _brokenRocket;
    [SerializeField] private ParticleSystem _flameStream;

    [SerializeField, Space(10)] private float _speed;
    [SerializeField, Range(0.1f, 5f)] private float _accelerationTime = 2;
    [SerializeField] private AnimationCurve _accelerationCurve;

    private Transform _heavenPoint;
    private Transform _normalPoint;
    private Transform _badPoint;
    private Transform _worstPoint;

    private Vector3 _finishPoint;

    public event Action OnExplosion;

    public void SetFlyPoint(Transform heavenPoint, Transform normalPoint, Transform badPoint, Transform worstPoint)
    {
        _heavenPoint = heavenPoint;
        _normalPoint = normalPoint;
        _badPoint = badPoint;
        _worstPoint = worstPoint;
    }

    public void Launch(FuelType fuelType)
    {
        switch (fuelType)
        {
            case FuelType.worst:
                _finishPoint = _worstPoint.position;
                break;
            case FuelType.bad:
                _finishPoint = _badPoint.position;
                break;
            case FuelType.normal:
                _finishPoint = _normalPoint.position;
                break;
            case FuelType.excellent:
                _finishPoint = _heavenPoint.position;
                break;
        }

        _flameStream.Play();
        StartCoroutine(Fly());
    }

    private IEnumerator Fly()
    {
        var accelerationTimer = _accelerationTime;
        while (true)
        {
            accelerationTimer = Mathf.Clamp(accelerationTimer - Time.deltaTime, 0, _accelerationTime);
            var accelerationTimePercent = (_accelerationTime - accelerationTimer) / _accelerationTime;
            var accelaration = Mathf.Clamp(_accelerationCurve.Evaluate(accelerationTimePercent), 0, 1);
            var currentSpeed = (_speed * accelaration) * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, _finishPoint, currentSpeed);

            if(transform.position == _finishPoint) break;

            yield return new WaitForFixedUpdate();
        }

        Explosion();
    }

    private void Explosion()
    {
        OnExplosion?.Invoke();

        var brokenRocket = Instantiate(_brokenRocket, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}

public enum FuelType
{
    worst,
    bad,
    normal,
    excellent
}
