using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace com.natgeo.xr {
	[RequireComponent(typeof(Animator))]
	public class AssetSequenceAnimate : MonoBehaviour {
		[SerializeField] private bool autoStart = true;

		private Animator animator;

		public enum AnimatorTriggerType {
			Trigger,
			Integer,
			Float,
			Bool
		}

		[System.Serializable]
		public struct SequenceInfo
		{
			public float startDelay;
			public AnimatorTriggerType triggerType;
			public string triggerName;
			public string triggerValue;
			public UnityEvent OnStartAnimation;
			public UnityEvent OnFinishAnimation;
		}

		public List<SequenceInfo> sequences;

		private int sequencePlayhead = 0;
		private bool animating = false;

		private UnityEvent currentOnFinishAnimation;


		private void StartAnimate() {
			if (sequences == null)
				return;
			SequenceInfo sInfo = sequences [sequencePlayhead];

			currentOnFinishAnimation = sInfo.OnFinishAnimation;

			if (sInfo.OnStartAnimation != null) {
				sInfo.OnStartAnimation.Invoke ();
			}

			switch (sInfo.triggerType) {
			case AnimatorTriggerType.Bool:
				bool valBool = Convert.ToBoolean(sInfo.triggerValue);
				animator.SetBool (sInfo.triggerName, valBool);
				break;
			case AnimatorTriggerType.Float:
				float valFloat = 0;
				if (float.TryParse(sInfo.triggerValue, out valFloat))
				{
					animator.SetFloat (sInfo.triggerName, valFloat);
				}
				else
				{
					Debug.LogError(this.name + " Error parsing value for animator FLOAT");
				}
				break;
			case AnimatorTriggerType.Integer:
				int valInt = 0;
				if (int.TryParse(sInfo.triggerValue, out valInt))
				{
					animator.SetInteger (sInfo.triggerName, valInt);
				}
				else
				{
					Debug.LogError(this.name + " Error parsing value for animator INTEGER");
				}
				break;
			case AnimatorTriggerType.Trigger:
				animator.SetTrigger (sInfo.triggerName);
				break;
			}
			animating = true;
		}

		void StartNextAnimation() {
			sequencePlayhead++;

            if (sequencePlayhead < sequences.Count)
                StartAnimator();
            
            else sequencePlayhead = 0;
		}

		IEnumerator DelayWait(float delay) {
			yield return new WaitForSeconds (delay);
			StartAnimate ();
		}


		void Start() {

			animator = GetComponent<Animator> ();

			if (!autoStart)
				return;

			StartAnimator ();
		}


		void Update () {
			if (animating) {
				bool isPlaying = (animator.GetCurrentAnimatorStateInfo (0).normalizedTime > 1 && !animator.IsInTransition (0));
				if (isPlaying == false) {
					animating = false;
					if (currentOnFinishAnimation != null) {
						currentOnFinishAnimation.Invoke ();
					}
						StartNextAnimation ();

				}
			}
		}


        public void StartAnimator ()
		{
			if (sequences == null)
				return;

			SequenceInfo sInfo = sequences [sequencePlayhead];
			StartCoroutine(DelayWait(sInfo.startDelay));
		}
	}
}
