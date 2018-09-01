using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;

namespace com.natgeo.xr {
	public class NGUPublisherSystem : EditorWindow
	{
		static Texture brandTexture;
		static Texture brandIcon;

		bool STEP_1_showPosition = true;

		// Lets poke Maps for the distance between two cities.
		static string url = "http://natgeo-asset-bundler.cseevolve.com/api/ngo_get_projectmetas/ALPHA_AssetBundler";
		static UnityWebRequest www;

		private void OnEnable()
		{
            brandTexture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/NGXRCore/AssetBundleBuilder/UI/editor_banner.png");
            brandIcon = AssetDatabase.LoadAssetAtPath<Texture> ("Assets/NGXRCore/AssetBundleBuilder/UI/editor_icon_20.png");
		}


		void OnGUI () {
			//STYLES
			GUIStyle blackBGStyle = new GUIStyle ();
			blackBGStyle.normal.background = MakeTex(10,10,Color.black);
			blackBGStyle.border = new RectOffset ();



			//Anchors
			//float content_x = 10;
			//float content_width = position.width - 20;

			//UI
			EditorGUILayout.BeginVertical ();
			EditorGUILayout.BeginHorizontal (blackBGStyle);
			GUILayout.FlexibleSpace ();
			GUILayout.Box (brandTexture,blackBGStyle);
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal ();
			EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);
			EditorGUILayout.EndVertical ();


			EditorGUILayout.BeginVertical ();
			STEP_1_showPosition = EditorGUILayout.Foldout (STEP_1_showPosition, "STEP 1: Build AssetBundles");
			if(STEP_1_showPosition) {
				EditorGUILayout.Space ();
				NGUEditorPrefs.STEP_1_selectedPlatform = EditorGUILayout.Popup("Platform Target", NGUEditorPrefs.STEP_1_selectedPlatform, NGUEditorPrefs.STEP_1_availablePlatformTargets);
				EditorGUILayout.Space ();
				if (GUILayout.Button ("Build Asset Bundles")) {
					BuildAssetBundles ();
				}
			}
			EditorGUILayout.EndVertical ();

			/*GUI.BeginGroup (new Rect (10,70,content_width,200));
			STEP_1_showPosition = EditorGUILayout.Foldout (STEP_1_showPosition, "STEP 1: Build AssetBundles");
			if(STEP_1_showPosition) {
				EditorGUI.Vector3Field(new Rect(10, 20, content_width, 40), "Position", Vector3.zero);
			}
			EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);
			GUI.EndGroup ();*/
		}

		void OnInspectorUpdate()
		{
			Repaint();
		}




		static void Request()
		{
			www = UnityWebRequest.Get(url);
			www.SetRequestHeader("Content-Type", "application/json");
			www.SetRequestHeader("api-key", "pQUBeKPOxfaE9brYMpbCDg");
			www.SendWebRequest();
		}

		static void EditorUpdate()
		{
			while (!www.isDone)
				return;

			if (www.isNetworkError)
				Debug.Log(www.error);
			else
				Debug.Log(www.downloadHandler.text);

			EditorApplication.update -= EditorUpdate;
		}

		[MenuItem ("NatGeo/Publish to Eureka")]
		static void UpdateFromSpreadsheet()
		{
			NGUPublisherSystem window = EditorWindow.GetWindow<NGUPublisherSystem> ();
			GUIContent titleContent = new GUIContent ("Eureka", brandIcon);
			window.titleContent = titleContent;
			window.minSize = new Vector2 (300.0f, 400.0f);

			//EditorApplication.update += EditorUpdate;
			//Request();
		}


		public void BuildAssetBundles ()
		{
			NGUAssetBuilder.BuildAssetBundles(NGUEditorPrefs.STEP_1_selectedPlatform);
		}


		//UTILITIES

		private Texture2D MakeTex(int width, int height, Color col)
		{
			Color[] pix = new Color[width*height];

			for(int i = 0; i < pix.Length; i++)
				pix[i] = col;

			Texture2D result = new Texture2D(width, height);
			result.SetPixels(pix);
			result.Apply();

			return result;
		}
	}
}