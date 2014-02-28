using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HTTPRequestWrapper : MonoBehaviour {

	// http://stackoverflow.com/questions/8951489/unity-get-post-wrapper
//    public string lastURL;
	public int pendingRequests = 0;
	private class DownloadQueue {
		public DownloadQueue (string url, Action<string> onSuccess, Action<string>onFail) {
			this.url = url;
			this.onSuccess = onSuccess;
			this.onFail = onFail;
		}
		public string url;
		public Action<string> onSuccess;
		public Action<string> onFail;
		public WWW www;
		public bool done = false;
	}
	private Queue<DownloadQueue> downloadQueue = new Queue<DownloadQueue>();
	private DownloadQueue[] downloadingList = new DownloadQueue[1];

	public WWW Get (string url, Action<WWW> callback = null) {
		WWW www = new WWW(url);
		StartCoroutine(WaitForRequest(www, callback));
		return www;
	}

	public void Get (string url, Action<string> onSuccess, Action<string> onFail = null) {
//        StartCoroutine(WaitForRequest(url, onSuccess, onFail));
		downloadQueue.Enqueue(new DownloadQueue(url, onSuccess, onFail));
	}

	private void Awake () {
		progress = new float[downloadingList.Length];
		InformationMonitor.SetLabel("Pending: ", 0);
	}

	public float[] progress;
	private float gcInterval = 60f;
	private float nextGC = 60f;
	private float checkInterval = 0.2f;
	private float nextCheck = 0f;
	public void LateUpdate () {
		if (nextCheck > Time.time) {
			return;
		}
		for (int i=0; i<downloadingList.Length; i++) {
			if (downloadingList[i] == null || downloadingList[i].done) {
				if (downloadQueue.Count > 0) {
					downloadingList[i] = downloadQueue.Dequeue();
					downloadingList[i].www = new WWW(downloadingList[i].url);
					pendingRequests++;
				}

			} else {
				progress[i] = downloadingList[i].www.progress;
				if (downloadingList[i].www.isDone) {
					var www = downloadingList[i].www;
					if (string.IsNullOrEmpty(www.error)
					        && !string.IsNullOrEmpty(www.text)) {
						string res = string.Copy(www.text);
						Debug.Log("Success!: " + www.text + "\n" + www.url);
						downloadingList[i].onSuccess(res);
					} else {
						string err = "NULL RESPONCE";
						if (!string.IsNullOrEmpty(www.error)) {
							err = string.Copy(www.error);
						}
						Debug.Log("Error: " + www.error + "\n" + www.url);
						downloadingList[i].onFail(err);
					}
					www.Dispose();
					downloadingList[i].www = null;
					downloadingList[i].done = true;
					pendingRequests--;
				}
			}
		}

		InformationMonitor.SetLabel("Pending: ", pendingRequests);

		nextCheck = checkInterval + Time.time;
		if (nextGC < Time.time) {
			GC.Collect();
			nextGC = gcInterval + Time.time;
		}
	}

	public WWW Post (string url, Dictionary<string, string> post) {
		WWWForm form = new WWWForm();
		foreach(KeyValuePair<string, string> post_arg in post) {
			form.AddField(post_arg.Key, post_arg.Value);
		}
		WWW www = new WWW(url, form);
		StartCoroutine(WaitForRequest(www));
		return www;
	}

	private IEnumerator WaitForRequest(string url, Action<string> onSuccess, Action<string> onFail) {
		using (WWW www = new WWW(url)) {
			yield return www;

			if (!string.IsNullOrEmpty(www.error)
			        || string.IsNullOrEmpty(www.text)) {
				Debug.Log("Error: " + www.error + "\n" + www.url);
				onFail(www.error);
			} else {
				Debug.Log("Success!: " + www.text + "\n" + www.url);
				onSuccess(www.text);
			}
		}
	}

	private IEnumerator WaitForRequest(WWW www, Action<WWW> callback = null) {
//        lastURL = www.url;
		yield return www;

		if (www.error == null) {
			Debug.Log("Success!: " + www.text + "\n" + www.url);
		} else {
			Debug.Log("Error: " + www.error + "\n" + www.url);
		}
		if (callback != null) {
			callback(www);
		}
	}

	public string EncodeURL (string url) {
		return url.Replace("?", "%3F").
		       Replace("&", "%26").
		       Replace("=", "%3D");
	}
}
