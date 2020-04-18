using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour {

    private Rigidbody _rigidBody;
	void Start () {
		_rigidBody = GetComponent<Rigidbody>();
	}
	
	void Update () {
        _rigidBody.AddTorque(Vector3.right,ForceMode.Acceleration);
	}
}
