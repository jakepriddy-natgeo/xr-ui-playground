using UnityEditor;

namespace com.natgeo.xr {
	public class NGUEditorPrefs {

		public static string[] STEP_1_availablePlatformTargets = new string[]
		{
			"iOS", "Android"
		};


		public static int STEP_1_selectedPlatform {
			get {
				return EditorPrefs.GetInt("STEP_1_selectedPlatform", 0);
			}
			set {
				EditorPrefs.SetInt("STEP_1_selectedPlatform", value);
			}
		}
	}
}