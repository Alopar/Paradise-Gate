using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameUIManager _gameUIManager;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private Transform _heavenPoint;
    [SerializeField] private ParalaxController _backgroundParalaxController;
    
    [SerializeField, Space(10)] private float _speed;
    [SerializeField] private AnimationCurve _moveCurve;

    private Transform _rocket;

    private StressReceiver _stressReceiver;

    private Vector3 _maxHeightPosition;
    private Vector3 _minHeightPosition;
    private float _maxDistance;

    private bool _isFirstDown = true;
    private bool _isFollowRocket = false;

    private void Start()
    {
        _stressReceiver = GetComponent<StressReceiver>();
        
        _maxHeightPosition = new Vector3(_heavenPoint.position.x, _heavenPoint.position.y - 5, _heavenPoint.position.z);
        _minHeightPosition = new Vector3(_heavenPoint.position.x, _cameraPoint.position.y, _heavenPoint.position.z);
        _maxDistance = Vector3.Distance(_minHeightPosition, _maxHeightPosition);

        transform.position = new Vector3(_maxHeightPosition.x, _maxHeightPosition.y, _cameraPoint.position.z);

        StartCoroutine(Move(transform.position, _cameraPoint.position, 1f));
        
        GameUIManager.OnLaunchRocket += LaunchRocketHandler;
    }

    public void SetRocket(RocketController rocketController)
    {
        if (_rocket != null)
        {
            _rocket.GetComponent<RocketController>().OnExplosion -= ExplosionHandler;
            _rocket.GetComponent<RocketController>().OnBigExplosion -= ExplosionBigHandler;
        }

        rocketController.OnExplosion += ExplosionHandler;
        rocketController.OnBigExplosion += ExplosionBigHandler;
        _rocket = rocketController.GetComponent<Transform>();
    }

    private void ExplosionHandler()
    {
        _isFollowRocket = false;

        _stressReceiver.InduceStress(1);

        StartCoroutine(Move(transform.position, _cameraPoint.position, 1.5f));
    }

    private void ExplosionBigHandler()
    {
        _isFollowRocket = false;
        _stressReceiver.InduceStress(1);
    }

    private void Update()
    {
        if (!_isFollowRocket) return;

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(_rocket.position.y, _cameraPoint.position.y, _heavenPoint.position.y - 5), transform.position.z);
    }

    private void FixedUpdate()
    {
        var currentPosition = new Vector3(_heavenPoint.position.x, transform.position.y, _heavenPoint.position.z);
        var currentDistance = Vector3.Distance(_minHeightPosition, currentPosition);
        var currentDistanceDelta = currentDistance / _maxDistance;
        _backgroundParalaxController.SetDistanceDelta(currentDistanceDelta);

        _gameUIManager.SetFlyProgress(currentDistanceDelta);
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

            if (delayTimer <= 0)
            {
                var currentDistancePercent = Vector3.Distance(startPosition, transform.position) / totalDistance;
                var currentSpeed = _speed * _moveCurve.Evaluate(currentDistancePercent);
                transform.position = Vector3.MoveTowards(transform.position, endPosition, currentSpeed * Time.deltaTime);

                if (transform.position == _cameraPoint.position) break;
            }
         
            yield return new WaitForFixedUpdate();
        }

        if (_isFirstDown)
        {
            _isFirstDown = false;
            _gameUIManager.ShowTutorial();
        }
    }

    private void OnDestroy()
    {
        GameUIManager.OnLaunchRocket -= LaunchRocketHandler;
    }
}
