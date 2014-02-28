using UnityEngine;
using System.Collections;

public class AutoRotateCamera : MonoBehaviour {

	public Transform center;
	public float speed = 3f;

	// Use this for initialization
	void Start () {
		if (center == null) {
			enabled = false;
		}
	}

	// Update is called once per frame
	void LateUpdate () {
		center.Rotate(Vector3.up * speed * Time.deltaTime);
		transform.LookAt(center);
	}
}
