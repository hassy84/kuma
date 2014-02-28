using UnityEngine;
using System.Collections;
using Jaxa;

public class SphereManipulator : MonoBehaviour {

	public JAXAOpenAPI api;
	public ProceduralSphere sphere;
	public DataType sphereDataType = DataType.SMC;
	public float sphereScale = -0.004f;

	private float lastUpdate = 0f;

	void Start () {
		api.onDataUpdate += OnDataUpdate;
		api.onDataComplete += OnDataComplete;
	}

	void OnDataUpdate (DataType type, float[] datas) {
		if (type == sphereDataType && lastUpdate < Time.time) {
			sphere.ChangeHeight(datas, sphereScale);
			lastUpdate = Time.time + 0.3f;
		}
	}

	void OnDataComplete (DataType type, float[] datas) {
		sphere.ChangeHeight(datas, sphereScale);
	}
}
