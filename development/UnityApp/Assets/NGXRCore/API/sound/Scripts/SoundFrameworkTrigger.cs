using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.natgeo.xr {
	public class SoundFrameworkTrigger : MonoBehaviour {

		[SerializeField]
		private AudioClip clip;

		[SerializeField]
		private AudioSource sourceTarget;

		[SerializeField]
		private SoundFrameworkHandler.PlayType playType; 

		[SerializeField]
		private SoundFrameworkHandler.PlayOverlap playOverlap;

		[SerializeField]
		private float volume = 1.0f;

		[SerializeField]
		private float startDelay = 0.0f;

		public bool playOnAwake = false;

		public UnityEvent OnClipStart;
		public UnityEvent OnClipStop;

		void Awake() {
			if (sourceTarget == null) {
				sourceTarget = GetComponent<AudioSource> ();
			}

			if (sourceTarget == null) {
				Debug.Log (this.name + " No Audio Source Found, Creating One");
				sourceTarget = new AudioSource ();
			}
		}

		void OnEnable() {
			if (playOnAwake) {
				PlayClip ();
			}
		}

		// Use this for initialization
		void Start () {
			
		}
		
		public void PlayClip() {
			if (startDelay > 0) {
				StartCoroutine (DelayPlay());
			} else {
				Reproduce ();
			}
		}

		private IEnumerator DelayPlay() {
			yield return new WaitForSeconds (startDelay);
			Reproduce ();
		}

		private void Reproduce() {
			SoundFrameworkHandler.Instance.PlaySource (sourceTarget, playType, clip, volume, playOverlap, OnClipStart, OnClipStop);
		}

		public void StopClip() {
			CancelDelayedPlay ();
			SoundFrameworkHandler.Instance.StopSource (sourceTarget);
		}

		public void CancelDelayedPlay() {
			StopAllCoroutines ();
		}
	}
}
