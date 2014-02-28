using UnityEngine;
using System.Collections;

public class buttonTestScript : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}


		// to be public because  ButtonScript will access this method from outside
		public void OnButtonClickDummy ()
		{
				Debug.Log ("botton clicked at dummy");
		}


}
