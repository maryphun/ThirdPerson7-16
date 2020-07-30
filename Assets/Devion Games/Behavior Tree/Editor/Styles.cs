using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace DevionGames.BehaviorTrees
{
	public static class Styles
	{
		public const float gridMinorSize = 12f;
		public const float gridMajorSize = 120f;
		public static Color gridMinorColor;
		public static Color gridMajorColor;

		public static Texture2D logo;

		public static GUISkin skin;
		public static GUIStyle graphBackground;
		public static GUIStyle graphSelection;
		public static GUIStyle toolbarActiveButton;
		public static GUIStyle sidebarBackground;
		public static GUIStyle wrappedLabel;
		public static GUIStyle inspectorTitle;
		public static GUIStyle inspectorTitleText;
		public static GUIStyle elementBackground;
		public static GUIStyle notificationText;
		public static GUIStyle descriptionText;

		public static GUIStyle pinBox;
		public static GUIStyle pinBackground;
		public static GUIStyle pinInactive;
		public static GUIStyle pinActive;

		public static Texture2D visibilityOn;
		public static Texture2D visibilityOff;
		public static Texture2D playOn;
		public static Texture2D playOff;
		public static Texture2D pauseOn;
		public static Texture2D pauseOff;
		public static Texture2D step;
		public static Texture2D popupIcon;
		public static Texture2D helpIcon;
		public static Texture2D errorIcon;
		public static Texture2D warnIcon;
		public static Texture2D infoIcon;
		public static Texture2D worldIcon;
		public static Texture2D breakpoint;

		static Styles ()
		{
			Styles.logo = Resources.Load<Texture2D> ("BehaviorLogo");

			Styles.gridMinorColor = EditorGUIUtility.isProSkin ? new Color (0f, 0f, 0f, 0.18f) : new Color (0f, 0f, 0f, 0.1f);
			Styles.gridMajorColor = EditorGUIUtility.isProSkin ? new Color (0f, 0f, 0f, 0.28f) : new Color (0f, 0f, 0f, 0.15f);

			Styles.skin = Resources.Load<GUISkin> ("BehaviorTreeSkin");

			Styles.pinBox = Styles.skin.GetStyle ("Pin Box");
			Styles.pinBackground = Styles.skin.GetStyle ("Pin Background");
			Styles.pinInactive = Styles.skin.GetStyle ("Pin Inactive");
			Styles.pinActive = Styles.skin.GetStyle ("Pin Active");

			Styles.graphBackground = new GUIStyle ("flow background");
			Styles.graphSelection = new GUIStyle ("SelectionRect");
			Styles.toolbarActiveButton = new GUIStyle (EditorStyles.toolbarButton);
			Styles.toolbarActiveButton.normal = EditorStyles.toolbarButton.active;
			Styles.sidebarBackground = new GUIStyle ("Label");
			Styles.wrappedLabel = new GUIStyle ("label") {
				fixedHeight = 0,
				wordWrap = true
			};
			Styles.inspectorTitle = new GUIStyle ("IN Foldout") {
				overflow = new RectOffset (0, 0, -3, 0),
				fixedWidth = 0
			};
			Styles.inspectorTitleText = new GUIStyle ("IN TitleText");
			Styles.elementBackground = new GUIStyle ("PopupCurveSwatchBackground") {
				padding = new RectOffset ()
			};
			Styles.notificationText = new GUIStyle ("Label") {
				fontStyle = FontStyle.Bold,
				fontSize = 18,
				wordWrap = true,
			};
			Styles.notificationText.normal.textColor = new Color (0.8f, 0.8f, 0.8f, 1f);
			Styles.descriptionText = new GUIStyle ("Label") {
				fontStyle = FontStyle.Bold,
				alignment = TextAnchor.LowerLeft,
				fontSize = 12,
				wordWrap = true,
			};
			Styles.descriptionText.normal.textColor = new Color (0.8f, 0.8f, 0.8f, 1f);

			Styles.visibilityOn = EditorGUIUtility.FindTexture ("animationvisibilitytoggleon");
			Styles.visibilityOff = EditorGUIUtility.FindTexture ("animationvisibilitytoggleoff"); 
			Styles.playOff = EditorGUIUtility.FindTexture ("PlayButton");
			Styles.playOn = EditorGUIUtility.FindTexture ("PlayButton On");
			Styles.pauseOff = EditorGUIUtility.FindTexture ("PauseButton");
			Styles.pauseOn = EditorGUIUtility.FindTexture ("PauseButton On");
			Styles.step = EditorGUIUtility.FindTexture ("StepButton");
			Styles.popupIcon = EditorGUIUtility.FindTexture ("_popup");
			Styles.helpIcon = EditorGUIUtility.FindTexture ("_help");
			Styles.errorIcon = EditorGUIUtility.FindTexture ("d_console.erroricon.sml");
			Styles.warnIcon = EditorGUIUtility.FindTexture ("console.warnicon");
			Styles.infoIcon = EditorGUIUtility.FindTexture ("console.infoicon");
			Styles.worldIcon = EditorGUIUtility.FindTexture ("d_BuildSettings.Web.Small");
			Styles.breakpoint = EditorGUIUtility.FindTexture ("d_PauseButton Anim");

		}
	}
}