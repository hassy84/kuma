using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class FileWrapper {

	private string folderName = "/";

	public void SetDirectory (string dir) {
		folderName = dir;
	}

	public bool CopyArrayFromFile (string fileName, ref float[] dst) {
		if (folderName.Length == 0) {
			folderName = "/";
		}
		string filePath = folderName + fileName;
		if (File.Exists(filePath)) {
			byte[] savedBytes = File.ReadAllBytes(filePath);
			Buffer.BlockCopy(savedBytes, 0, dst, 0, savedBytes.Length);
			return true;
		} else {
			return false;
		}
	}

	bool inUse = false;
	public bool WriteArrayToFile (string fileName, float[] src) {
//        foreach (float v in src) {
//            if (v != -2) {
//                UnityEngine.Debug.Log("fine");
//                break;
//            }
//        }
		if (inUse) {
			return false;
		} else {
			inUse = true;
		}
		var byteArray = new byte[sizeof(float) * src.Length];
		Buffer.BlockCopy(src, 0, byteArray, 0, byteArray.Length);
		string filePath = folderName + fileName;
//        if (File.Exists(filePath)) {
//        }
		File.WriteAllBytes(filePath, byteArray);
		inUse = false;
		return true;
	}

	public bool Exists (string fileName) {
		return File.Exists(folderName + fileName);
	}
}
