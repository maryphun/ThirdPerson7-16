using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityAnimator
{
	[Category ("UnityEngine/Animator")]
	[Tooltip ("Creates a dynamic transition between the current state and the destination state. Both states have to be on the same layer.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Animator.CrossFade.html")]
	public class CrossFade: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The name of the destination state.")]
		public StringVariable stateName;
		[Tooltip ("The duration of the transition. Value is in source state normalized time.")]
		public FloatVariable transitionDuration;
		[Tooltip ("Layer index containing the destination state. If no layer is specified or layer is -1, the first state that is found with the given name or hash will be played.")]
		public IntVariable layer;
		[Tooltip ("Start time of the current destination state. Value is in source state normalized time, should be between 0 and 1. If no explicit normalizedTime is specified or normalizedTime value is float.NegativeInfinity, the state will either be played from the start if it's not already playing, or will continue playing from its current time and no transition will happen.")]
		public FloatVariable normalizedTime = float.NegativeInfinity;

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
			m_Animator.CrossFade (stateName.Value, transitionDuration, layer, normalizedTime);
			return TaskStatus.Success;
		}
	}
}
