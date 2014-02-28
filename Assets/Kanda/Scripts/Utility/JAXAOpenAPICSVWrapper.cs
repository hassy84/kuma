using UnityEngine;
using System;
using System.Collections;
using Jaxa;

public class JAXAOpenAPICSVWrapper : BasicCSVReader {

	public enum CompleteMode {ZEROFILL, CUBIC, LINER};
	public CompleteMode completeMode = CompleteMode.CUBIC;
	public int fileDataScale = 1;
	private int dataScale = 1;
	private int nbLon;
	private int nbLat;
	private int nbDay;

	public float GetDataAt (
	    DateTime date
	    , DataType dataType
	    , float apiLon, float apiLat) {

		int lon = Mathf.RoundToInt(apiLon / fileDataScale) * fileDataScale;
		int lat = Mathf.RoundToInt(apiLat / fileDataScale) * fileDataScale;

		if (completeMode == CompleteMode.ZEROFILL) {
			if (lon == apiLon && lat == apiLat) {
				return GetCompletedDataAt(date, dataType, lon, lat);
			} else {
				return -1f;
			}
		} else if (completeMode == CompleteMode.CUBIC) {
			return GetCompletedDataAt(date, dataType, lon, lat);

		} else if (completeMode == CompleteMode.LINER) {
			float point00 = Mathf.Max(GetCompletedDataAt(date, dataType
			                                   , Mathf.FloorToInt(apiLon / fileDataScale) * fileDataScale
			                                   , Mathf.RoundToInt(apiLat / fileDataScale) * fileDataScale)
					, 0f);
			float point01 = Mathf.Max(GetCompletedDataAt(date, dataType
			                                   , Mathf.CeilToInt(apiLon / fileDataScale) * fileDataScale
			                                   , Mathf.RoundToInt(apiLat / fileDataScale) * fileDataScale)
					, 0f);
			float point10 = Mathf.Max(GetCompletedDataAt(date, dataType
			                                   , Mathf.RoundToInt(apiLon / fileDataScale) * fileDataScale
			                                   , Mathf.FloorToInt(apiLat / fileDataScale) * fileDataScale)
					, 0f);
			float point11 = Mathf.Max(GetCompletedDataAt(date, dataType
			                                   , Mathf.RoundToInt(apiLon / fileDataScale) * fileDataScale
			                                   , Mathf.CeilToInt(apiLat / fileDataScale) * fileDataScale)
					, 0f);
			float lonRate = (apiLon % fileDataScale) / fileDataScale;
			float latRate = (apiLat % fileDataScale) / fileDataScale;
			return ((point00 * (1 - lonRate)
			         + point01 * lonRate)
			        + (point10 * (1 - latRate)
			           + point11 * latRate)) / 2;
		}

		return -1f;
	}

	public float GetCompletedDataAt (
	    DateTime date
	    , DataType dataType
	    ,  float lon, float lat) {

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
		csvIndex += Mathf.FloorToInt((lat + 90) / fileDataScale) * nbLon;

		// Longitude
		csvIndex += Mathf.FloorToInt((lon + 180) / fileDataScale);

		csvIndex *= (Size - 1);
		csvIndex += (int)dataType;

		float result;
		string rawData = Contents[csvIndex];
		if (!rawData.StartsWith("ER_")
		        && float.TryParse(rawData, out result)) {
			return result;
		}
		return -1f;
	}

	protected override void Awake () {
		base.Awake();
		dataScale = JAXAOpenAPI.Instance.dataScale;
		nbLon = 370 / fileDataScale;
		nbLat = 190 / fileDataScale;
		nbDay = nbLon * nbLat;
	}

}
