using UnityEngine;
using System.Collections;

public class BasicCSVReader : SingletonMonoBehaviour<BasicCSVReader> {

	public enum State : int {NotReady, Reading, Ready, Timeout}
	private State _status = State.NotReady;

	public TextAsset CSVFile;
	private int _recordSize  = 0;
	private int _countRecord  = 0;
	private string[] _contents;

	public int Size {
		get {
			return _recordSize;
		}
	}
	public int Count {
		get {
			return _countRecord;
		}
	}
	public string[] Contents {
		get {
			return _contents;
		}
	}
	public State Status {
		get {
			return _status;
		}
	}
	// public string[] debugParameterStringTable;

	public void AnalyzeTextAsset () {
		_status = State.Reading;
		_contents = CSVFile.text.Split(',', '\n');
		for (int i = 0; i < _contents.Length; i++) {
			_contents[i] = _contents[i].Replace("\"", string.Empty);
		}

		foreach (char c in CSVFile.text) {
			if (c == ',') {
				_recordSize ++;
			} else if (c == '\n') {
				_recordSize ++;
				break;
			}
		}
		_countRecord  = (_contents.Length - 1) / _recordSize ;
		// debugParameterStringTable = _contents;
		_status = State.Ready;
	}

	protected override void Awake () {
		if (CSVFile != null) {
			AnalyzeTextAsset();
		}
		base.Awake();
	}
}
