using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private Transform _heavenPoint;
    
    [SerializeField, Space(10)] private float _speed;
    [SerializeField] private AnimationCurve _moveCurve;

    private Transform _rocket;

    private StressReceiver _stressReceiver;

    private bool _isFollowRocket = false;

    private void Start()
    {
        _stressReceiver = GetComponent<StressReceiver>();

        transform.position = new Vector3(_cameraPoint.position.x, _heavenPoint.position.y, _cameraPoint.position.z);

        StartCoroutine(Move(transform.position, _cameraPoint.position, 1f));
        
        GameUIManager.OnLaunchRocket += LaunchRocketHandler;
    }

    private void Update()
    {
        if (!_isFollowRocket) return;

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(_rocket.position.y, _cameraPoint.position.y, _heavenPoint.position.y - 2), transform.position.z);
    }

    private void ExplosionHandler()
    {
        _isFollowRocket = false;
        
        _rocket.GetComponent<RocketController>().OnExplosion -= ExplosionHandler;

        _stressReceiver.InduceStress(1);

        StartCoroutine(Move(transform.position, _cameraPoint.position, 1.5f));
    }

    private void LaunchRocketHandler()
    {
        _isFollowRocket = true;
    }

    private IEnumerator Move(Vector3 startPosition, Vector3 endPosition, float delay)
    {
        var delayTimer = delay;
        var totalDistance = Vector3.Distance(startPosition, endPosition);
        while (true)
        {
            delayTimer -= Time.deltaTime;

            if(delayTimer <= 0)
            {
                var currentDistancePercent = Vector3.Distance(startPosition, transform.position) / totalDistance;
                var currentSpeed = _speed * _moveCurve.Evaluate(currentDistancePercent);
                transform.position = Vector3.MoveTowards(transform.position, endPosition, currentSpeed * Time.deltaTime);

                if (transform.position == _cameraPoint.position) break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void SetRocket(RocketController rocketController)
    {
        rocketController.OnExplosion += ExplosionHandler;
        _rocket = rocketController.GetComponent<Transform>();
    }

    private void OnDestroy()
    {
        GameUIManager.OnLaunchRocket -= LaunchRocketHandler;
    }
}
