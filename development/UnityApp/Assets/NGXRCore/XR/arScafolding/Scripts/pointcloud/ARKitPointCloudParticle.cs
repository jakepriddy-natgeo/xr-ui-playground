using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;


namespace com.natgeo.xr {
	public class ARKitPointCloudParticle : MonoBehaviour {
		public ParticleSystem pointCloudParticlePrefab;
		public int maxPointsToShow;
		public float particleSize = 1.0f;
		private Vector3[] m_PointCloudData;
		private bool frameUpdated = false;
		private ParticleSystem currentPS;
		private ParticleSystem.Particle [] particles;

		// Use this for initialization
		void Start () {
			
		}
		public void Init() {
			UnityARSessionNativeInterface.ARFrameUpdatedEvent += ARFrameUpdated;
			currentPS = Instantiate (pointCloudParticlePrefab);
			currentPS.transform.parent = transform;
			frameUpdated = false;
		}

		public void ARFrameUpdated(UnityARCamera camera)
		{
			m_PointCloudData = camera.pointCloudData;
			frameUpdated = true;
		}

		// Update is called once per frame
		void Update () {
			if (frameUpdated) {
				if (m_PointCloudData != null && m_PointCloudData.Length > 0) {
					int numParticles = Mathf.Min (m_PointCloudData.Length, maxPointsToShow);
					ParticleSystem.Particle[] particles = new ParticleSystem.Particle[numParticles];
					int index = 0;
					foreach (Vector3 currentPoint in m_PointCloudData) {     
						particles [index].position = currentPoint;
						particles [index].startColor = new Color (1.0f, 1.0f, 1.0f);
						particles [index].startSize = particleSize;
						index++;
					}
					currentPS.SetParticles (particles, numParticles);
				} else {
					ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1];
					particles [0].startSize = 0.0f;
					currentPS.SetParticles (particles, 1);
				}
				frameUpdated = false;
			}
		}

		public void DeInit() {
			UnityARSessionNativeInterface.ARFrameUpdatedEvent -= ARFrameUpdated;
			frameUpdated = false;
			currentPS.Clear ();
			Destroy (currentPS);
		}
	}
}
