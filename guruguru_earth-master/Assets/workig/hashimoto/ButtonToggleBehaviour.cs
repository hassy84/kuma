using UnityEngine;
using System.Collections;

public class ButtonToggleBehaviour : MonoBehaviour
{



		public UISprite targetSprite;
		private bool isToggleOn;

		public GameObject messageTarget;




		// Use this for initialization
		void Start ()
		{
				isToggleOn = false;
				targetSprite.spriteName = "btn_off";
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}



		public void OnToggleButtonClicked ()
		{
				if (isToggleOn) {
						isToggleOn = false;
						targetSprite.spriteName = "btn_off";
						//TODO if require
						//messageTarget.SendMessage ("dummyMessage", SendMessageOptions.DontRequireReceiver);
				} else {
						isToggleOn = true;
						targetSprite.spriteName = "btn_on";
						//TODO if require
						//messageTarget.SendMessage ("dummyMessage", SendMessageOptions.DontRequireReceiver);
				}
		}


}
