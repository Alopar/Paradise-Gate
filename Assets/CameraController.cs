using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private Transform _heavenPoint;
    [SerializeField] private Transform _rocket;
    
    [SerializeField, Space(10)] private float _speed;
    [SerializeField] private AnimationCurve _moveCurve;

    private bool _isFollowRocket = false;

    private void Start()
    {
        transform.position = new Vector3(_cameraPoint.position.x, _heavenPoint.position.y, _cameraPoint.position.z);

        StartCoroutine(Move(transform.position, _cameraPoint.position));

        GameUIManager.OnLaunchRocket += LaunchRocketHandler;
    }

    private void Update()
    {
        if (!_isFollowRocket) return;

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(_rocket.position.y, _cameraPoint.position.y, _heavenPoint.position.y - 2), transform.position.z);
    }

    private void LaunchRocketHandler()
    {
        _isFollowRocket = true;
    }

    private IEnumerator Move(Vector3 startPosition, Vector3 endPosition)
    {
        var totalDistance = Vector3.Distance(startPosition, endPosition);
        while (true)
        {   
            var currentDistancePercent = Vector3.Distance(startPosition, transform.position) / totalDistance;
            var currentSpeed = _speed * _moveCurve.Evaluate(currentDistancePercent);
            transform.position = Vector3.MoveTowards(transform.position, endPosition, currentSpeed * Time.deltaTime);

            if(transform.position == _cameraPoint.position) break;

            yield return new WaitForFixedUpdate();
        }
    }

    private void OnDestroy()
    {
        GameUIManager.OnLaunchRocket -= LaunchRocketHandler;
    }
}
