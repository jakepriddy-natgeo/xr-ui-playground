using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.iOS;

namespace com.natgeo.xr
{
	public class ARKitPlanesController : MonoBehaviour {

		public GameObject planePrefab;

		private Dictionary<string, ARPlaneAnchorGameObject> planeAnchorMap;

		// Use this for initialization
		void Start () {
			
		}

		public void Init() {
			planeAnchorMap = new Dictionary<string,ARPlaneAnchorGameObject> ();
			UnityARSessionNativeInterface.ARAnchorAddedEvent += AddAnchor;
			UnityARSessionNativeInterface.ARAnchorUpdatedEvent += UpdateAnchor;
			UnityARSessionNativeInterface.ARAnchorRemovedEvent += RemoveAnchor;
		}

		public void AddAnchor(ARPlaneAnchor arPlaneAnchor)
		{
			GameObject go = CreatePlaneInScene (arPlaneAnchor);
			go.transform.parent = transform;
			ARPlaneAnchorGameObject arpag = new ARPlaneAnchorGameObject ();
			arpag.planeAnchor = arPlaneAnchor;
			arpag.gameObject = go;
			planeAnchorMap.Add (arPlaneAnchor.identifier, arpag);
		}

		public void RemoveAnchor(ARPlaneAnchor arPlaneAnchor)
		{
			if (planeAnchorMap.ContainsKey (arPlaneAnchor.identifier)) {
				ARPlaneAnchorGameObject arpag = planeAnchorMap [arPlaneAnchor.identifier];
				GameObject.Destroy (arpag.gameObject);
				planeAnchorMap.Remove (arPlaneAnchor.identifier);
			}
		}

		public void UpdateAnchor(ARPlaneAnchor arPlaneAnchor)
		{
			if (planeAnchorMap.ContainsKey (arPlaneAnchor.identifier)) {
				ARPlaneAnchorGameObject arpag = planeAnchorMap [arPlaneAnchor.identifier];
				UpdatePlaneWithAnchorTransform (arpag.gameObject, arPlaneAnchor);
				arpag.planeAnchor = arPlaneAnchor;
				planeAnchorMap [arPlaneAnchor.identifier] = arpag;
			}
		}


		public void UnsubscribeEvents()
		{
			UnityARSessionNativeInterface.ARAnchorAddedEvent -= AddAnchor;
			UnityARSessionNativeInterface.ARAnchorUpdatedEvent -= UpdateAnchor;
			UnityARSessionNativeInterface.ARAnchorRemovedEvent -= RemoveAnchor;
		}

		public void DeInit() {
			UnsubscribeEvents();

			foreach (ARPlaneAnchorGameObject arpag in GetCurrentPlaneAnchors()) {
				Destroy (arpag.gameObject);
			}

			planeAnchorMap.Clear ();
		}

		public List<ARPlaneAnchorGameObject> GetCurrentPlaneAnchors()
		{
			return planeAnchorMap.Values.ToList ();
		}

		private GameObject CreatePlaneInScene(ARPlaneAnchor arPlaneAnchor)
		{
			GameObject plane;
			if (planePrefab != null) {
				plane = GameObject.Instantiate(planePrefab);
			} else {
				plane = new GameObject (); //put in a blank gameObject to get at least a transform to manipulate
			}

			plane.name = arPlaneAnchor.identifier;

			return UpdatePlaneWithAnchorTransform(plane, arPlaneAnchor);

		}

		private GameObject UpdatePlaneWithAnchorTransform(GameObject plane, ARPlaneAnchor arPlaneAnchor)
		{

			//do coordinate conversion from ARKit to Unity
			plane.transform.position = UnityARMatrixOps.GetPosition (arPlaneAnchor.transform);
			plane.transform.rotation = UnityARMatrixOps.GetRotation (arPlaneAnchor.transform);

			MeshFilter[] mfs = plane.GetComponentsInChildren<MeshFilter> ();

			foreach (MeshFilter mf in mfs) {
				//since our plane mesh is actually 10mx10m in the world, we scale it here by 0.1f
				mf.gameObject.transform.localScale = new Vector3(arPlaneAnchor.extent.x * 0.1f ,arPlaneAnchor.extent.y * 0.1f ,arPlaneAnchor.extent.z * 0.1f );

				//convert our center position to unity coords
				mf.gameObject.transform.localPosition = new Vector3(arPlaneAnchor.center.x,arPlaneAnchor.center.y, -arPlaneAnchor.center.z);
			}

			return plane;
		}
	}
}