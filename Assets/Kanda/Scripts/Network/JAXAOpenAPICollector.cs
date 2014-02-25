using UnityEngine;
using System;
using System.Collections;
using Jaxa;

public class JAXAOpenAPICollector : MonoBehaviour {

	private JAXAOpenAPI api;
	public DataType[] requestTypes;

	IEnumerator Start () {
		while (JAXAOpenAPI.Instance == null) {
			yield return null;
		}
		api = JAXAOpenAPI.Instance;
		foreach (var type in requestTypes) {
			api.CollectData(type, new DateTime(2012, 10, 1));
		}
	}
}
