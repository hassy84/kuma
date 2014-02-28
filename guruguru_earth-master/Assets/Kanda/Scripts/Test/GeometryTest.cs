using UnityEngine;
using System.Collections;

public class GeometryTest : MonoBehaviour {

	public Transform pointer;
	public MeshFilter meshFilter;
	public Mesh geometry;

	private int nbLong = 360;
	public int lon = 0;
	public int lat = 0;
	public bool auto = true;

	void Start () {
		nbLong--;
	}

	// Update is called once per frame
	void Update () {
		if (geometry == null) {
			meshFilter = gameObject.GetComponent<MeshFilter>();
			geometry = meshFilter.mesh;
		}
		if (geometry == null) {
			return;
		}
		lon = lon % 360;
		lat = lat % 180;
		lon = Mathf.Abs(lon);
		lat = Mathf.Abs(lat);
//        pointer.position = geometry.vertices[lon + lat * (nbLong)];
		pointer.position = geometry.vertices[(lat * nbLong) + lon + lat];
		if (auto) {
			++lat;
		}
	}
}
