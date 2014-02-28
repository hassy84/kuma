using UnityEngine;
using System.Collections;

public class autoRotate : MonoBehaviour
{

		Vector3 myCenter;


		// Use this for initialization
		void Start ()
		{
				myCenter = transform.position;
		}
	
		// Update is called once per frame
		void Update ()
		{
				transform.RotateAround (myCenter, transform.up, 45 * Time.deltaTime);
		}
}
