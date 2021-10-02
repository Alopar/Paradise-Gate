using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    [SerializeField] private Transform _heavenPoint;
    [SerializeField] private Transform _normalPoint;
    [SerializeField] private Transform _badPoint;
    [SerializeField] private Transform _worstPoint;

    [SerializeField, Space(10)] private float _speed;
    [SerializeField, Range(0.1f, 5f)] private float _accelerationTime = 2;
    [SerializeField] private AnimationCurve _accelerationCurve;

    private Vector3 _finishPoint;

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
        // TODO: explosion animation
    }
}

public enum FuelType
{
    worst,
    bad,
    normal,
    excellent
}
