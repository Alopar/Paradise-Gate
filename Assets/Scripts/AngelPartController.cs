using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelPartController : MonoBehaviour
{
    [SerializeField] private Transform _center;
    private Rigidbody _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.AddTorque(new Vector3(Random.value, Random.value, Random.value), ForceMode.Impulse);
        _rigidbody.AddExplosionForce(1000, _center.position, 10);
    }
}
