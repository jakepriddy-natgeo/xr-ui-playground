using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace com.natgeo.xr {
	
	public class SoundFrameworkHandler : MonoBehaviour
	{
		static SoundFrameworkHandler _instance;

		public static SoundFrameworkHandler Instance
		{
			get
			{
				if (!_instance)
				{
					Debug.LogWarning("No Sound Framework Handler instance available. Creating one.");
					GameObject obj = new GameObject("Sound Framework Handler");
					_instance = obj.AddComponent<SoundFrameworkHandler>();
				}
				return _instance;
			}
		}

		public enum PlayType
		{
			OneShot,
			Loop
		}

		public enum PlayOverlap
		{
			NoOverlap,
			Multiple
		}

		private Dictionary<AudioSource,UnityEvent> endAudioCallbacks = new Dictionary<AudioSource,UnityEvent> ();

		protected void Awake()
		{   
			// Prevent having more than one instance in a scene
			if (_instance)
			{ 
				Debug.LogWarning("There is already an instance of SoundFrameworkHandler.");
				Destroy(gameObject);
				return;
			}

			// The SoundFrameworkHandler instance should be persistent between scenes
			//DontDestroyOnLoad(gameObject);

			_instance = this;
		}



		public void PlaySource(AudioSource source, PlayType type, AudioClip clip, float volume = 1.0f, PlayOverlap sourceOverlap = PlayOverlap.Multiple, UnityEvent onStartHandler = null, UnityEvent onEndHandler = null) {

			if (source.isPlaying && sourceOverlap == PlayOverlap.NoOverlap) {
				return;
			}

			if (onStartHandler != null) {
				onStartHandler.Invoke ();
			}

			if (onEndHandler != null)
				endAudioCallbacks.Add (source, onEndHandler);
			source.volume = volume;

			switch (type) {
			case PlayType.Loop:
				source.loop = true;
				source.clip = clip;
				source.Play();
				break;
			case PlayType.OneShot:
				source.loop = false;
				source.PlayOneShot(clip);
				break;
			}
		}

		public void StopSource(AudioSource source) {
			if (source.isPlaying) {
				source.Stop ();
			}
		}

		private void ExecuteCallback(AudioSource source) {
			UnityEvent callback = endAudioCallbacks [source];
			if (callback != null) {
				callback.Invoke ();
				endAudioCallbacks.Remove (source);
			}
		}

		private void Update() {
			AudioSource callbackKey = null;
			foreach (KeyValuePair<AudioSource,UnityEvent>endAudioCallback in endAudioCallbacks) {

				AudioSource source = endAudioCallback.Key;
				if (source == null)
					continue;
				if (!source.isPlaying) {
					callbackKey = endAudioCallback.Key;
					break;
				}
			}

			if (callbackKey != null) {
				ExecuteCallback (callbackKey);
			}
		}
	}
}