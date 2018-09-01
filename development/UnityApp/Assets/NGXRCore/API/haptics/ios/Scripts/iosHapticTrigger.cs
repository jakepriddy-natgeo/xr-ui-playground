using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.natgeo.xr {
	public class iosHapticTrigger : MonoBehaviour {

		public enum Magnitud {
			Soft,
			Medium,
			Hard,
			Double
		}

		[Tooltip("Continuous Offset sets the time between feedbacks on a continues haptic feedback chain")]
		[SerializeField] private float continuosOffset = 0.1f;

		private bool isContinuos = false;

		private bool isRunning = false;

		[Tooltip("Continuous Magnitude sets the type of haptic feedback magnitude on a continues haptic feedback chain")]
		[SerializeField]
		private Magnitud continuesMagnitud = Magnitud.Medium;

		public void TriggerContinous(float time) {
			if (isRunning) {
				return;
			}

			isRunning = true;
			isContinuos = true;
			StartCoroutine (StopContinues(time));
			RunHaptics ();
		}

		IEnumerator StopContinues(float time) {
			yield return new WaitForSeconds (time);
			isContinuos = false;
			isRunning = false;
		}

		private void RunHaptics() {
			if (isContinuos) {
				TriggerOneShot (continuesMagnitud);
				StartCoroutine (ContinousTrigger ());
			} else {

			}
		}


		IEnumerator ContinousTrigger() {
			yield return new WaitForSeconds (continuosOffset);
			if (isContinuos)
				RunHaptics ();
		}

		private void TriggerOneShot(Magnitud magnitude) {
			switch (magnitude) {
			case Magnitud.Soft:
				TriggerOneShot_Light();
				break;
			case Magnitud.Medium:
				TriggerOneShot_Medium ();
				break;
			case Magnitud.Hard:
				TriggerOneShot_Hard();
				break;
			case Magnitud.Double:
				TriggerOneShot_Double ();
				break;
			}
		}

		public void TriggerOneShot_Light() {
			iosHapticHandler.Instance.Trigger (iosHapticHandler.iOSFeedbackType.ImpactLight);
		}
		public void TriggerOneShot_Medium() {
			iosHapticHandler.Instance.Trigger (iosHapticHandler.iOSFeedbackType.ImpactMedium);
		}
		public void TriggerOneShot_Hard() {
			iosHapticHandler.Instance.Trigger (iosHapticHandler.iOSFeedbackType.ImpactHeavy);
		}
		public void TriggerOneShot_Double() {
			iosHapticHandler.Instance.Trigger (iosHapticHandler.iOSFeedbackType.Warning);
		}

	}
}