using UnityEngine;
using System.Collections;

public class UIControlBehaviour : MonoBehaviour
{


		private enum Status
		{
				Enlarging,
				Stay,
				Shrinking
		}

		private Status uiStatus;
		private bool isPanelOpening;

		private GameObject UIPanel;
		private GameObject UIPanelRenderer;

		public bool isHorizontal;

		public float panelInitialX;
		public float panelFinalX;
		public float panelInitialY;
		public float panelFinalY;

		public int changeSteps;




		private float panelStepX;
		private float panelStepY;
		private float sizeChangeStep;




		// Use this for initialization
		void Start ()
		{
				if (isHorizontal) {
						UIPanel = GameObject.Find ("Panel_H");
						GameObject.Find ("Panel_V").SetActive (false);
				} else {
						UIPanel = GameObject.Find ("Panel_V");
//						GameObject.Find ("Panel_H").SetActive (false);
				}


				panelStepX = (panelFinalX - panelInitialX) / changeSteps;
				panelStepY = (panelFinalY - panelInitialY) / changeSteps;
				//UIPanel.transform.localScale = new Vector3(0.01f, 0.0f1, 0.01f);
//				UIPanel.transform.localScale = new Vector3 (0, 0, 0);
//				Debug.Log (panelStepY);
				UIPanel.transform.localPosition = new Vector3 (panelInitialX, panelInitialY, 0);

				sizeChangeStep = 1.0f / changeSteps;
//				sizeChangeStep = 1 / 50;
				Debug.Log (sizeChangeStep);

				// initial situation
				isPanelOpening = false;
				uiStatus = Status.Stay;
				UIPanel.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
				UIPanel.SetActive (false);


		}
	
		// Update is called once per frame
		void Update ()
		{
				if (uiStatus == Status.Enlarging) {
						if (UIPanel.transform.localScale.x <= 1 - sizeChangeStep) {
//								UIPanelRenderer.transform.renderer.enabled = true;
								Vector3 tempScale = UIPanel.transform.localScale;
								tempScale.x += sizeChangeStep;
								tempScale.y += sizeChangeStep;
								tempScale.z += sizeChangeStep;
								UIPanel.transform.localScale = tempScale;
						} else {
								UIPanel.transform.localScale = new Vector3 (1, 1, 1);
						}

						if (isHorizontal) {
								Vector3 tempPos = UIPanel.transform.localPosition;
								tempPos.x += panelStepX;
								tempPos.y += panelStepY;
								UIPanel.transform.localPosition = tempPos;
				
//				if (tempPos.x <= panelFinalX && tempPos.y <= panelFinalY) {
//								if (tempPos.x <= panelFinalX) {
//										uiStatus = Status.Stay;
//										isPanelOpening = true;
//								}
						} else {
								Vector3 tempPos = UIPanel.transform.localPosition;
								tempPos.x += panelStepX;
								tempPos.y += panelStepY;
								UIPanel.transform.localPosition = tempPos;
				
//				if (tempPos.x >= panelFinalX && tempPos.y >= panelFinalY) {
//								if (tempPos.y >= panelFinalY) {
//										uiStatus = Status.Stay;
//										isPanelOpening = true;
//								}
						}
						if (UIPanel.transform.localScale.x >= 1.0f - sizeChangeStep * 2) {
								uiStatus = Status.Stay;
								isPanelOpening = true;
						}
				}

				if (uiStatus == Status.Shrinking) {
						if (UIPanel.transform.localScale.x > sizeChangeStep) {
								Vector3 tempScale = UIPanel.transform.localScale;
								tempScale.x -= sizeChangeStep;
								tempScale.y -= sizeChangeStep;
								tempScale.z -= sizeChangeStep;
								UIPanel.transform.localScale = tempScale;
						} else {
								UIPanel.transform.localScale = new Vector3 (sizeChangeStep, sizeChangeStep, sizeChangeStep);
						}

						if (isHorizontal) {
								Vector3 tempPos = UIPanel.transform.localPosition;
								tempPos.x -= panelStepX;
								tempPos.y -= panelStepY;
								UIPanel.transform.localPosition = tempPos;
				
//				if (tempPos.x >= panelInitialX && tempPos.y >= panelInitialY) {
//								if (tempPos.x >= panelInitialX) {
//										uiStatus = Status.Stay;
//										isPanelOpening = false;
//										UIPanel.SetActive (false);
//								}
						} else {
								Vector3 tempPos = UIPanel.transform.localPosition;
								tempPos.x -= panelStepX;
								tempPos.y -= panelStepY;
								UIPanel.transform.localPosition = tempPos;
				
//				if (tempPos.x <= panelInitialX && tempPos.y <= panelInitialY) {
//								if (tempPos.y <= panelInitialY) {
//										uiStatus = Status.Stay;
//										isPanelOpening = false;
//										UIPanel.SetActive (false);
//								}
						}
						if (UIPanel.transform.localScale.x <= sizeChangeStep * 2) {
								uiStatus = Status.Stay;
								isPanelOpening = false;
								UIPanel.SetActive (false);
						}
				}
		}


		void OnTogglePanel ()
		{
				if (uiStatus == Status.Stay && isPanelOpening == false) {
						uiStatus = Status.Enlarging;
						UIPanel.SetActive (true);
				} else if (uiStatus == Status.Stay && isPanelOpening == true) {
						uiStatus = Status.Shrinking;
				}



		}





}
