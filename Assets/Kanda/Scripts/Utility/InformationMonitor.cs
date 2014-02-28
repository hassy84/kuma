using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InformationMonitor : SingletonMonoBehaviour<InformationMonitor> {

    public Dictionary<string, string> param = new Dictionary<string, string>();

    private static bool monitorEnable = true;

    void OnEnable() {
        monitorEnable = true;
    }

    void OnDisable() {
        monitorEnable = false;
        param.Clear();
    }

    Vector2 scrollPositions = new Vector2(0, 0);
    void OnGUI() {
        scrollPositions = GUILayout.BeginScrollView(scrollPositions,  GUILayout.Width(400), GUILayout.Height(400));
        foreach ( string key in param.Keys) {
            string message = string.Format("{0} : {1}", key, param[key]);
            GUILayout.Label(message, GUILayout.Height(20));
        }
        GUILayout.EndScrollView();
    }

    public static void SetLabel(string key, string message) {
        if ( !monitorEnable )
            return;

        if (! Instance.param.ContainsKey(key )) {
            Instance.param.Add(key, message);
        } else {
            Instance.param[key] = message;
        }
    }

    public static void SetLabel(string key, object message) {
        SetLabel(key, message.ToString());
    }
}
