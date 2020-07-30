using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DevionGames.BehaviorTrees
{
	[InitializeOnLoad]
	[System.Serializable]
	public class BehaviorSelection : ScriptableObject
	{
		public static System.Action selectionChanged;
		public static System.Action behaviorReloaded;
		private static BehaviorSelection m_Current;

		private static BehaviorSelection current {
			get {
				if (!m_Current) {
					m_Current = Resources.FindObjectsOfTypeAll<BehaviorSelection> ().FirstOrDefault ();
					if (!m_Current) {
						m_Current = ScriptableObject.CreateInstance<BehaviorSelection> ();
						m_Current.hideFlags = HideFlags.HideAndDontSave;
					}
				}
				return m_Current;
			}
		}

		[SerializeField]
		private Object m_ActiveObject;

		public static Object activeObject {
			get { 
				return BehaviorSelection.current.m_ActiveObject;
			}
			set { 
				if (value is GameObject || value is ScriptableObject) {
					BehaviorSelection.current.m_ActiveObject = value;
				} else {
					BehaviorSelection.current.m_ActiveObject = null;
				}
			}
		}

		public static GameObject activeGameObject {
			get { 
				return BehaviorSelection.activeObject as GameObject;
			}
		}

		public static BehaviorTemplate activeTemplate {
			get { 
				return (BehaviorSelection.activeBehavior is Behavior) ? (BehaviorSelection.activeBehavior as Behavior).m_Template : BehaviorSelection.activeObject as BehaviorTemplate;
			}
		}

		[SerializeField]
		private int m_ActiveBehaviorID;
		private IBehavior m_ActiveBehavior;

		public static IBehavior activeBehavior {
			get {
				return BehaviorSelection.current.m_ActiveBehavior != null && !BehaviorSelection.current.m_ActiveBehavior.Equals (null) ? BehaviorSelection.current.m_ActiveBehavior : null;
			}
			set { 
				bool chaned = BehaviorSelection.current.m_ActiveBehavior != value;
				BehaviorSelection.current.m_ActiveBehavior = value;
				if (value != null) {
					BehaviorSelection.current.m_ActiveBehaviorID = value.GetInstanceID ();
				}
				if (chaned && selectionChanged != null) {
					selectionChanged.Invoke ();
				}
			}
		}

        public new static void SetDirty() {
            if (BehaviorSelection.activeBehavior is Behavior)
            {
                EditorUtility.SetDirty((Behavior)BehaviorSelection.activeBehavior);
            }
            else if (BehaviorSelection.activeBehavior is BehaviorTemplate)
            {
                EditorUtility.SetDirty((BehaviorTemplate)BehaviorSelection.activeBehavior);
            }
        }

		public static BehaviorTree activeBehaviorTree {
			get { 
				return BehaviorSelection.activeBehavior != null && !BehaviorSelection.activeBehavior.Equals (null) ? BehaviorSelection.activeBehavior.GetBehaviorTree () : null;
			}
		}

		static BehaviorSelection ()
		{
			Selection.selectionChanged += OnSelectionChange;
			#if UNITY_2017_2_OR_NEWER
			EditorApplication.playModeStateChanged += OnPlaymodeStateChange;
			#else
			EditorApplication.playmodeStateChanged += OnPlaymodeStateChange;
			#endif
		}

		private static void OnSelectionChange ()
		{
			if (EditorPrefs.GetBool ("LockSelection") && BehaviorSelection.activeBehaviorTree != null) {
				return;
			}

			BehaviorSelection.activeBehavior = null;
			BehaviorSelection.activeObject = Selection.activeObject;
			if (Selection.activeGameObject != null) {
				BehaviorSelection.activeBehavior = Selection.activeGameObject.GetComponent<IBehavior> ();

			} else if (Selection.activeObject is BehaviorTemplate) {
				BehaviorSelection.activeBehavior = (Selection.activeObject as IBehavior);			
			}

		}

		#if UNITY_2017_2_OR_NEWER
		private static void OnPlaymodeStateChange (PlayModeStateChange state)
		
#else
		private static void OnPlaymodeStateChange ()
		#endif
		{
			ReloadPreviousBehavior ();
		}

		[UnityEditor.Callbacks.DidReloadScripts]
		private static void OnScriptsReloaded ()
		{
			if (!EditorApplication.isPlayingOrWillChangePlaymode) {
				ReloadPreviousBehavior ();
			}
		}

		private static void ReloadPreviousBehavior ()
		{
			if (BehaviorSelection.activeGameObject != null) {
				IBehavior[] behaviors = BehaviorSelection.activeGameObject.GetComponents<IBehavior> ();
				for (int i = 0; i < behaviors.Length; i++) {
					IBehavior behavior = behaviors [i]; 
					if (behavior.GetInstanceID () == BehaviorSelection.current.m_ActiveBehaviorID) {
						BehaviorSelection.activeBehavior = behavior;
						if (behaviorReloaded != null) {
							behaviorReloaded.Invoke ();
						}
						return;
					}
				}
			} else if (BehaviorSelection.activeTemplate != null) {
				BehaviorSelection.activeBehavior = BehaviorSelection.activeTemplate as IBehavior;
				if (behaviorReloaded != null) {
					behaviorReloaded.Invoke ();
				}
				return;
			}
			BehaviorSelection.activeBehavior = null;
		}

		public static void RecordUndo (string name)
		{
			Object target = BehaviorSelection.activeTemplate != null ? BehaviorSelection.activeTemplate : BehaviorSelection.activeBehavior as UnityEngine.Object;
			if (target != null) {
				Undo.RecordObject (target, name);
			}
		}

		public static void RecordCompleteUndo (string name)
		{
			Object target = BehaviorSelection.activeTemplate != null ? BehaviorSelection.activeTemplate : BehaviorSelection.activeBehavior as UnityEngine.Object;
			if (target != null) {
				Undo.RegisterCompleteObjectUndo (BehaviorSelection.activeTemplate != null ? BehaviorSelection.activeTemplate : BehaviorSelection.activeBehavior as UnityEngine.Object, name);
			}
		}
	}
}