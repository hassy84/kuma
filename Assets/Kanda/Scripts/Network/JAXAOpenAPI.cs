using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Jaxa;

public class JAXAOpenAPI : SingletonMonoBehaviour<JAXAOpenAPI> {

	public float[] GetCollectedData (DataType type) {
		return currentDatas[type];
	}

	public void CollectData (DataType type, DateTime fromDate) {
		CollectData(type, fromDate, fromDate);
	}

	public void CollectData (DataType type, DateTime fromDate, DateTime toDate) {
		StartCoroutine(CollectAllData(type, fromDate, toDate));
	}

	private const string API_TOKEN = "TOKEN_Lu2C_";
	private string apiRoot = "https://joa.epi.bz/api/";
	private string apiFormat = "json";
	private DateTime apiDataStart;
	private DateTime apiDataEnd;
	private const int MAX_PARALLEL_REQUEST = 32;
	private int pendingHttpRequests = 0;

	public bool abortCollection = false;
	public enum DataSource {SERVER, CSV, DUMMY};
	public DataSource dataSource = DataSource.SERVER;

	public int dataScale = 1;
	[HideInInspector]
	public int totalLongitudes = 360;
	[HideInInspector]
	public int totalLatitudes = 180;
	[HideInInspector]
	public float apiRange = 0.5f;

	private HTTPRequestWrapper http;
	private FileWrapper fileSystem;
	private JAXAOpenAPICSVWrapper csv;
	private Dictionary<DataType, float[]> currentDatas = new Dictionary<DataType, float[]>();

	public delegate void OnDataUpdateHandler(DataType type, float[] newData);
	public event OnDataUpdateHandler onDataUpdate;
	public delegate void OnDataCompleteHandler(DataType type, float[] newData);
	public event OnDataCompleteHandler onDataComplete;

	protected override void Awake () {
		totalLatitudes /= dataScale;
		totalLongitudes /= dataScale;
		apiRange *= dataScale;

		switch (dataSource) {
		case DataSource.SERVER:
			http = gameObject.AddComponent<HTTPRequestWrapper>();
			fileSystem =  new FileWrapper();
			fileSystem.SetDirectory(Application.persistentDataPath + "/");
			break;
		case DataSource.CSV:
			csv = gameObject.GetComponent<JAXAOpenAPICSVWrapper>();
			break;
		}

		foreach (DataType type in Enum.GetValues(typeof(DataType))) {
			currentDatas.Add(type, new float[(totalLongitudes + 1) * totalLatitudes]);
		}

		base.Awake();
	}

	private IEnumerator CollectAllData (DataType type, DateTime fromDate, DateTime toDate) {
		float[] results = currentDatas[type];
		for(var i=0; i<results.Length; ++i) {
			results[i] = -2f;
		}
		float lon = 0f;
		float lat = 0f;
		DateTime requestDate = fromDate;
		DateTime endDate = toDate;
		int retrievedDatas = 0;

		while (requestDate <= endDate) {
			// キャッシュファイルの場所
			var currentDataFilePath =
			    type.ToString() + requestDate.ToString("yyyy-MM-dd")
			    + "x" + dataScale.ToString() + ".cache";
			if (dataSource == DataSource.SERVER) {
				// キャッシュファイルから情報を復元
				if (fileSystem.CopyArrayFromFile(currentDataFilePath, ref results)) {
					int loadedCount = 0;
					foreach (float v in results) {
						if (v != -2) {
							++loadedCount;
//                        print(v);
						}
					}
					print("Loaded " + loadedCount.ToString() + " of " + type.ToString() + " on " +
					      requestDate.ToString("yyyy-MM-dd") + " from cache.");
				}
			}

			// データ保存用のメソッド。コールバックから呼ばれる
			Action<float, float, float> StoreData = (float _lon, float _lat, float val) => {
				pendingHttpRequests--;
				retrievedDatas++;

				int index = Mathf.FloorToInt(_lat * totalLongitudes + _lon);
				results[index] = val;

				if (retrievedDatas % 32 == 0) {
					SaveCache(currentDataFilePath, results);
				}
			};

			print("Retrieving " + type.ToString() + " on " + requestDate.ToString("yyyy-MM-dd"));
			lon = 0f;
			lat = 0f;
			while (lon <= totalLongitudes) {

				// 取得を中止しているとき
				if (abortCollection) {
					if (pendingHttpRequests <= 0) {
						print("Retrieve aborted");
						SaveCache(currentDataFilePath, results);
						return false;
					}
					yield return new WaitForSeconds(3f);
					continue;
				}

				// すでに初期値以外のデータが入っていた場合
				if (results[Mathf.FloorToInt(lat * totalLongitudes + lon)] != -2f) {
					retrievedDatas++;

				} else {

					if (pendingHttpRequests >= MAX_PARALLEL_REQUEST) {
						yield return new WaitForSeconds(5f);
						continue;
					}

					// HTTPRequestWrapperにデータの取得を依頼する
					pendingHttpRequests++;
					RetrieveData(type, requestDate, lon, lat,
					(float data, float longitude, float latitude) => {
						StoreData(longitude, latitude, data);
					},
					(string responceText, float longitude, float latitude) => {
						StoreData(longitude, latitude, -1f);
					});

					// 取得間隔調整用
					if (pendingHttpRequests >= 8) {
						yield return new WaitForSeconds(2.5f);
					} else if (pendingHttpRequests > 0) {
						yield return new WaitForSeconds(0.2f);
					}
				}

				// ビューがデータの更新を検知するためのイベント
				if (onDataUpdate != null) {
					onDataUpdate(type, results);
				}

				InformationMonitor.SetLabel("Progress (" + type.ToString() + "): ",
				                            (retrievedDatas * 100 / results.Length).ToString() + "% (" + retrievedDatas.ToString() + "/" +
				                            results.Length.ToString() + ")");

				lat++;
				if (lat >= totalLatitudes) {
					lon++;
					lat = 0;
					yield return null;
				}
			}
			SaveCache(currentDataFilePath, results);
			requestDate = requestDate.AddDays(1);
			yield return null;
		}
		if (onDataComplete != null) {
			onDataComplete(type, results);
		}
	}

	public void RetrieveData (DataType type, DateTime date
	                          , float longitude, float latitude
	                          , Action<float, float, float> onSuccess = null
	                                  , Action<string, float, float> onFail = null) {
		JSONObject json;
		float result;
		float lonAtAPI = Mathf.Floor(longitude - totalLongitudes / 2) * dataScale;
		float latAtAPI = Mathf.Floor(latitude - totalLatitudes / 2) * dataScale;
		switch (dataSource) {
		case DataSource.SERVER:
			var url = GenerateRequestURL(
			              type
			              , date
			              , lonAtAPI
			              , latAtAPI);
			http.Get(url,
			(string res) => {
				json = new JSONObject(res);
				result = json.GetField(type.ToString().ToLower()).f;
				onSuccess(result, longitude, latitude);
			},
			(string err) => {
				onFail(err, longitude, latitude);
			});
			break;
		case DataSource.CSV:
			result = csv.GetDataAt(date, type, lonAtAPI, latAtAPI);
			if (result == -1f) {
				onFail("No data", longitude, latitude);
			} else {
				onSuccess(result, longitude, latitude);
			}
			break;
		case DataSource.DUMMY:
			result = UnityEngine.Random.Range(-50f, 50f);
			onSuccess(result, longitude, latitude);
			break;
		}
	}

	public string GenerateRequestURL (
	    DataType dataType,
	    DateTime date,
	    float longitude,
	    float latitude) {
		return GenerateRequestURL(
		           dataType.ToString().ToLower(),
		           date.ToString("yyyy-MM-dd"),
		           longitude.ToString(),
		           latitude.ToString());
	}

	public string GenerateRequestURL (
	    string requestType,
	    string date,
	    string longitude,
	    string latitude) {
		return apiRoot
		       + requestType + "avg"
		       + "?token=" + API_TOKEN
		       + "&date=" + date
		       + "&lon=" + longitude
		       + "&lat=" + latitude
		       + "&range=" + apiRange.ToString()
		       + "&format=" + apiFormat;
	}

	private void SaveCache (string filePath, float[] arr) {
		if (dataSource == DataSource.SERVER) {
			if (fileSystem.WriteArrayToFile(filePath, arr)) {
				print("Cache updated.");
			}
		}
	}
}
