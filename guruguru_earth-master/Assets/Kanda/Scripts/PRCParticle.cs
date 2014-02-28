using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Jaxa;

public class PRCParticle : MonoBehaviour {

	public JAXAOpenAPI api;
	public DataType sphereDataType = DataType.SMC;
	public float radius = 1f;
	float[] _prcData = new float[0];

	// Longitude |||
	int nbLong = 360;
	// Latitude ---
	int nbLat = 180;
	int dataScale = 1;

	public float currentTime = 0f;
	float emitInterval = 1f;
	float nextEmit = 0f;
	float timeLength = 100f;

	void Start () {
		dataScale = JAXAOpenAPI.Instance.dataScale;
		nbLong /= dataScale;
		nbLat /= dataScale;
		nbLong--;

		api.onDataUpdate += OnDataUpdate;
		api.onDataComplete += OnDataUpdate;

		particleSystem.startSize = 0.015f * dataScale;
	}

	void LateUpdate () {
		if (_prcData.Length <= 0 || nextEmit > Time.time) {
			return;
		}
		currentTime = ++currentTime % timeLength;
		EmitParticles(_prcData, timeLength - currentTime);
		nextEmit = Time.time + emitInterval;
	}

	void OnDataUpdate (DataType type, float[] datas) {
		if (type == sphereDataType) {
			_prcData = datas;
		}
	}

	public void EmitParticles (float[] prcData, float time) {

		MeshFilter filter = GetComponent<MeshFilter>();
		Mesh mesh = filter.mesh;
		var vertices = mesh.vertices;
		var positions = new List<Vector3>();

		#region Vertices
		float _pi = Mathf.PI;
		float _2pi = _pi * 2f;

		for( int lat = 0; lat < nbLat; lat++ ) {
			float a1 = _pi * (float)(lat+1) / (nbLat+1);
			float sin1 = Mathf.Sin(a1);
			float cos1 = Mathf.Cos(a1);

			for( int lon = 0; lon <= nbLong; lon++ ) {
				float a2 = _2pi * (float)(lon == nbLong ? 0 : lon) / nbLong;
				float sin2 = Mathf.Sin(a2);
				float cos2 = Mathf.Cos(a2);

				if (prcData[lat * nbLong + lon] > time) {
					positions.Add(vertices[lat * nbLong + lon]);
				}
			}
		}
		#endregion

		for (int i=0; i<positions.Count; ++i) {
			particleSystem.Emit(positions[i] * radius, Vector3.zero, particleSystem.startSize, 5f, particleSystem.startColor);
		}
	}
}
