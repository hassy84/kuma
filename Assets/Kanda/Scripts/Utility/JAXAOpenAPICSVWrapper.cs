using UnityEngine;
using System;
using System.Collections;
using Jaxa;

public class JAXAOpenAPICSVWrapper : BasicCSVReader {

	private int dataScale = 1;
	private int nbLon;
	private int nbLat;
	private int nbDay;

	public float GetDataAt (
	    DateTime date
	    , DataType dataType
	    ,  float apiLon, float apiLat) {

		int csvIndex;
		// Day
		if (date >= new DateTime(2013, 7, 1)) {
			csvIndex = 3 * nbDay;
		} else if (date >= new DateTime(2013, 4, 1)) {
			csvIndex = 2 * nbDay;
		} else if (date >= new DateTime(2014, 1, 1)) {
			csvIndex = 1 * nbDay;
		} else if (date >= new DateTime(2012, 10, 1)) {
			csvIndex = 0 * nbDay;
		} else {
			Debug.LogError("Unexpected date " + date.ToString("yyyy-MM-dd"));
			csvIndex = 0;
		}

		// Latitude
		csvIndex += Mathf.FloorToInt((apiLat + 90) / dataScale) * nbLon;

		// Longitude
		csvIndex += Mathf.FloorToInt((apiLon + 180) / dataScale);

		var rawData = Contents[csvIndex];
		float result;
		if (!rawData.StartsWith("ER_")
		        && float.TryParse(rawData, out result)) {
			return result;
		}
		return -1f;
	}

	protected override void Awake () {
		base.Awake();
		dataScale = JAXAOpenAPI.Instance.dataScale;
		nbLon = 370 / dataScale;
		nbLat = 190 / dataScale;
		nbDay = nbLon * nbLat;
	}

}
