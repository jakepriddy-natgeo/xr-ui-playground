using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.Events;


namespace com.natgeo.xr {
	public class ARKitCalibrationHandler : MonoBehaviour {

		static ARKitCalibrationHandler _instance;

		public static ARKitCalibrationHandler Instance
		{
			get
			{
				if (!_instance)
				{
					Debug.LogWarning("No ARKit Calibration Handler instance available. Creating one.");
					GameObject obj = new GameObject("ARKit Calibration Handler");
					_instance = obj.AddComponent<ARKitCalibrationHandler>();
				}
				return _instance;
			}
		}


		public delegate void CalibrationStartedEvent();
		public delegate void CoordinateReadyEvent(Vector3 position, Quaternion rotation);
		public delegate void CalibrationStopEvent();

		public CalibrationStartedEvent calibrationStartedEvent;
		public CoordinateReadyEvent firstCoordinateReadyEvent;
		public CoordinateReadyEvent coordinateReadyEvent;
		public CalibrationStopEvent calibrationStopEvent;

		private List<float> activeResultTypes;

		private bool isFirstCoordEvent = true;

		protected void Awake()
		{   
			// Prevent having more than one instance in a scene
			if (_instance)
			{ 
				Debug.LogWarning("There is already an instance of ARKitCalibrationHandler.");
				Destroy(gameObject);
				return;
			}

			// The ARKitCalibrationHandler instance should be persistent between scenes
			//DontDestroyOnLoad(gameObject);

			_instance = this;
		}

		public void StartCalibration(List<float> arKitResultTypes) {
			if (calibrationStartedEvent != null)
				calibrationStartedEvent.Invoke ();

			activeResultTypes = arKitResultTypes;

			StartCoroutine (ARHitTest ());
		}

		IEnumerator ARHitTest() {

			while (true)
			{
				CheckStagePosition();
				yield return null;
			}
				
		}

		private void CheckStagePosition() {
			Vector3 screenPos = Camera.main.ScreenToViewportPoint(new Vector2(Screen.width/2, Screen.height/2));
			ARPoint point = new ARPoint
			{
				x = screenPos.x,
				y = screenPos.y
			};

			foreach (float resultType in activeResultTypes)
			{
				List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, (ARHitTestResultType)resultType);
				if (hitResults.Count > 0)
				{
					foreach (var hitResult in hitResults)
					{
						// GOT HIT

						if (isFirstCoordEvent) {
							isFirstCoordEvent = false;
							if (firstCoordinateReadyEvent != null) {
								firstCoordinateReadyEvent.Invoke (UnityARMatrixOps.GetPosition (hitResult.worldTransform), UnityARMatrixOps.GetRotation (hitResult.worldTransform));
							}
						} else {
							if (coordinateReadyEvent != null)
								coordinateReadyEvent.Invoke (UnityARMatrixOps.GetPosition(hitResult.worldTransform), UnityARMatrixOps.GetRotation(hitResult.worldTransform));
						}
						break;
					}
				}
			}
		}

		public void StopCalibration() {
			StopAllCoroutines ();

			if (calibrationStopEvent != null)
				calibrationStopEvent.Invoke ();

			isFirstCoordEvent = true;
		}

		/*

	    [SerializeField] private GameObject target;
	    [SerializeField] private GameObject targetingPanel;
	    [SerializeField] private LayerMask targetColliderMask;
	    [SerializeField] private StageCalibrationTarget calibrationTarget;
	    [SerializeField] private CanvasGroup[] instructionPanels;
		[SerializeField] private Transform mainAnchor;
	    [SerializeField] private PointCloudParticleExample particleLogic;

	    private bool largeStagePlaced = false;
		private bool targetingValidLocation = false;
	    private Transform mainCamTrans;
	    private bool tappedOk = false;

	    private void Start()
	    {
	        mainCamTrans = Camera.main.transform;
	    }

	    public override void ExitStage(CallbackHandler onFinished)
	    {
	        gameObject.SetActive(false);
			target.SetActive(false);
	        targetingPanel.SetActive(false);
	        for (int i = 0; i < instructionPanels.Length; i++)
	        {
	            instructionPanels[i].gameObject.SetActive(false);
	        }
	        gameObject.SetActive(false);
	        particleLogic.particleSize = 0f;
	//        UnityARSessionNativeInterface.ARAnchorAddedEvent -= UnityARSessionNativeInterface_ARAnchorAddedEvent;
	//        UnityARSessionNativeInterface.ARFrameUpdatedEvent -= ARFrameUpdated;
	//        UnityARSessionNativeInterface.ARAnchorRemovedEvent += UnityARSessionNativeInterface_ARAnchorRemovedEvent;
			if(onFinished != null) onFinished();
	    }

	    public override void StartStage()
	    {
			target.SetActive(false);
	        largeStagePlaced = false;
	        targetingPanel.SetActive(false);
	        tappedOk = false;
	        StartCoroutine(StageFlow());
	    }


	    bool HitTestWithResultType(ARPoint point, ARHitTestResultType resultTypes)
	    {
	        List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultTypes);
	        if (hitResults.Count > 0)
	        {
	            foreach (var hitResult in hitResults)
	            {
	                Debug.Log("Got hit!");
	                //targetCircle.position = UnityARMatrixOps.GetPosition(hitResult.worldTransform);
	                //targetCircle.rotation = UnityARMatrixOps.GetRotation(hitResult.worldTransform);
	                return true;
	            }
	        }
	        return false;
	    }

	    // Update is called once per frame
	    void Update()
	    {
	        if (Input.GetKeyDown(KeyCode.Space))
	        {
				AdvanceToNextStage();
	        }
	    }

	    private IEnumerator StageFlow()
	    {

	        // Scanning message
	        instructionPanels[0].gameObject.SetActive(true);

	        // Place car message
	        targetingValidLocation = false;
	        targetingPanel.SetActive(true);

	        while (!largeStagePlaced)
	        {
	            CheckLargeStagePosition();
	            yield return null;
	        }
	        targetingPanel.SetActive(false);
			AdvanceToNextStage();

	    }

	    private void CheckLargeStagePosition()
	    {
	        Vector3 screenPos = Camera.main.ScreenToViewportPoint(new Vector2(Screen.width/2, Screen.height/2));
	        ARPoint point = new ARPoint
	        {
	            x = screenPos.x,
	            y = screenPos.y
	        };
	        // prioritize reults types
	        ARHitTestResultType[] resultTypes = {
	            ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
	            // if you want to use infinite planes use this:
	            //ARHitTestResultType.ARHitTestResultTypeExistingPlane,
	            ARHitTestResultType.ARHitTestResultTypeHorizontalPlane,
	            //ARHitTestResultType.ARHitTestResultTypeFeaturePoint
	        };

	        foreach (ARHitTestResultType resultType in resultTypes)
	        {
	            if (!largeStagePlaced)
	            {
	                List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface().HitTest(point, resultType);
	                if (hitResults.Count > 0)
	                {
	                    foreach (var hitResult in hitResults)
	                    {
	                        // GOT HIT
	                        LargeStageHit(UnityARMatrixOps.GetPosition(hitResult.worldTransform), UnityARMatrixOps.GetRotation(hitResult.worldTransform));
	                        break;
	                    }
	                }
	            }
	        }
	    }

	    public void TappedOk()
	    {
	        instructionPanels[1].gameObject.SetActive(false);
	        tappedOk = true;
			iosHapticHandler.Instance.Trigger(iosHapticHandler.iOSFeedbackType.ImpactLight);
			Debug.Log ("WORK");
	    }

	    private void LargeStageHit(Vector3 position, Quaternion rotation)
	    {
			target.transform.position = position;
			target.transform.rotation = rotation;
			Vector3 forward = target.transform.position - mainCamTrans.position;
	        forward.y = 0;
			target.transform.forward = forward;

	        // MODIFIED
			target.gameObject.SetActive(true);
	        targetingValidLocation = true;
	        if (!tappedOk)
	        {
	            instructionPanels[1].gameObject.SetActive(true);
	        }
	        instructionPanels[0].gameObject.SetActive(false);

	    }

	    private void LargeStageMiss()
	    {
			target.SetActive(false);
	        targetingValidLocation = false;
	    }

	    public void OnFinishTapped()
	    {
	        if (targetingValidLocation)
	        {
	            //AdvanceToCarSelection();
	            largeStagePlaced = true;

	            Debug.Log("StagePlaced");
	        }

	        targetingPanel.SetActive(false);
			AdvanceToNextStage();
	    }

	    private void PlaceLargeStage()
	    {
			mainAnchor.transform.position = target.transform.position;
			mainAnchor.transform.rotation = target.transform.rotation;
			iosHapticHandler.Instance.Trigger(iosHapticHandler.iOSFeedbackType.Success);
	    }

	    private void AdvanceToNextStage()
	    {
	        PlaceLargeStage();
	        largeStagePlaced = true;
			StageSequenceManager.Instance.GoToNextStage();
	    }
	    */
	}
}