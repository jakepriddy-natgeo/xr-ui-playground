using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace com.natgeo.xr {
	public class NGUAssetBuilder {

		public const string AssetBundlesOutputPath = "AssetBundles";

		private static string GetPlatformForAssetBundles(BuildTarget target)
		{
			switch(target)
			{
			case BuildTarget.Android:
				return "Android";
			case BuildTarget.iOS:
				return "iOS";
			case BuildTarget.WebGL:
				return "WebGL";
			case BuildTarget.StandaloneWindows:
			case BuildTarget.StandaloneWindows64:
				return "Windows";
			case BuildTarget.StandaloneOSX:
				return "OSX";
				// Add more build targets for your own.
				// If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
			default:
				return "NONE";
			}
		}

		private static BuildTarget GetBuildTargetFromEditorIndex(int selectedPlatformIndex) {
			switch(selectedPlatformIndex)
			{
			case 0:
				return BuildTarget.iOS;
			case 1:
				return BuildTarget.Android;
			default:
				return BuildTarget.iOS;
			}
		}

		public static void BuildAssetBundles(int selectedPlatformIndex)
		{
			// Choose the output path according to the build target.
			BuildTarget selectedBuildTarget = GetBuildTargetFromEditorIndex(selectedPlatformIndex);
			string outputPath = Path.Combine(AssetBundlesOutputPath,  GetPlatformForAssetBundles(selectedBuildTarget));
			if (!Directory.Exists(outputPath) )
				Directory.CreateDirectory (outputPath);

			//@TODO: use append hash... (Make sure pipeline works correctly with it.)
			BuildPipeline.BuildAssetBundles (outputPath, BuildAssetBundleOptions.ForceRebuildAssetBundle, selectedBuildTarget);
		}
	}
}
