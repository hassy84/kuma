using UnityEngine;
using System.Collections;

public class CSVTest : MonoBehaviour {
	public BasicCSVReader reader;
	public int recordSize;
	public int recordCount;
	public string[] contents;

	void Start () {
		contents = reader.Contents;
		recordSize = reader.Size;
		recordCount = reader.Count;
	}
}
