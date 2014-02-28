using UnityEngine;
using System.Collections;

public class SliceToggleBehaviour : MonoBehaviour
{



		public UISprite targetSprite;
		private bool isToggleOn;

		public GameObject messageTarget;




		// Use this for initialization
		void Start ()
		{
				isToggleOn = false;
				targetSprite.spriteName = "btn_slice";
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}



		public void OnToggleButtonClicked ()
		{
				if (isToggleOn) {
						isToggleOn = false;
						targetSprite.spriteName = "btn_slice";
						//TODO if require
						//messageTarget.SendMessage ("dummyMessage", SendMessageOptions.DontRequireReceiver);
				} else {
						isToggleOn = true;
						targetSprite.spriteName = "btn_slice_on";
						//TODO if require
						//messageTarget.SendMessage ("dummyMessage", SendMessageOptions.DontRequireReceiver);
				}
		}


}
