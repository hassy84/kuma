using UnityEngine;
using System.Collections;

public class DateButtonSelectBehaviour : MonoBehaviour
{

		public int myButtonName;

		private bool isToggleOn;

		public GameObject[] messageTarget = new GameObject[4];
		public UISprite targetSprite;


		// Use this for initialization
		void Start ()
		{
				for (int i=0; i<4; i++) {
						messageTarget [i] = GameObject.Find ("DateButton" + i);
				}

				if (myButtonName == 0) {
						isToggleOn = true;
						targetSprite.spriteName = "slider_btn_on";

				} else {
						isToggleOn = false;
						targetSprite.spriteName = "slider_btn_off";

				}



	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}


		public void ToggleOff ()
		{
				isToggleOn = false;
				targetSprite.spriteName = "slider_btn_off";
		}


		public void OnDateButtonSelected ()
		{
				isToggleOn = true;
				targetSprite.spriteName = "slider_btn_on";

				for (int i=0; i<4; i++) {
						if (i != myButtonName) {
								messageTarget [i].SendMessage ("ToggleOff", SendMessageOptions.DontRequireReceiver);

						}
				}
		}



}
