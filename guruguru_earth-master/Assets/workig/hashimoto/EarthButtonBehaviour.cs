using UnityEngine;
using System.Collections;

public class EarthButtonBehaviour : MonoBehaviour
{

		GameObject actionTarget;

		// Use this for initialization
		void Start ()
		{
				actionTarget = GameObject.Find ("_mainBehaviour");
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}


		public void OnMouseDown ()
		{
				Debug.Log ("clicked");
				actionTarget.SendMessage ("OnTogglePanel", SendMessageOptions.DontRequireReceiver);

		}



}
