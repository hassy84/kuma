using UnityEngine;
using System;
using System.Collections;
using Jaxa;

public class JAXAOpenAPICollector : MonoBehaviour {

	private JAXAOpenAPI api;
	public DataType[] requestTypes;
	public DateTime[] availableDates;
	public int dateIndex = 0;

	IEnumerator Start () {
		availableDates = new DateTime[] {
			new DateTime(2012, 10, 1)
			, new DateTime(2013, 1, 1)
			, new DateTime(2013, 4, 1)
			, new DateTime(2013, 7, 1)
		};
		while (JAXAOpenAPI.Instance == null) {
			yield return null;
		}
		api = JAXAOpenAPI.Instance;
		foreach (var type in requestTypes) {
			api.CollectData(type, availableDates[dateIndex]);
		}
	}
}
