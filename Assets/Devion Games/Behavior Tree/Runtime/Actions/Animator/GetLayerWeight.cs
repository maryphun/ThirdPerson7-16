using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityAnimator
{
	[Category ("UnityEngine/Animator")]
	[Tooltip ("Gets the layer's current weight.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Animator.GetLayerWeight.html")]
	public class GetLayerWeight: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The layer's index.")]
		public IntVariable layerIndex;
		[Shared]
		public FloatVariable m_GetLayerWeight;

		private GameObject m_PrevGameObject;
		private Animator m_Animator;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_Animator = m_gameObject.Value.GetComponent<Animator> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_Animator == null) {
				Debug.LogWarning ("Missing Component of type Animator!");
				return TaskStatus.Failure;
			}
			m_GetLayerWeight.Value = m_Animator.GetLayerWeight (layerIndex);
			return TaskStatus.Success;
		}
	}
}
