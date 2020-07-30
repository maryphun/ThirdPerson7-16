using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityAnimator
{
	[Category ("UnityEngine/Animator")]
	[Tooltip ("Plays a state. The state has to be on the same layer.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Animator.Play.html")]
	public class Play: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The name of the state to play.")]
		public StringVariable stateName;
		[Tooltip ("Layer index containing the destination state. If no layer is specified or layer is -1, the first state that is found with the given name or hash will be played.")]
		public IntVariable layer = -1;
		[Tooltip ("Start time of the current destination state. Value is in normalized time. If no explicit normalizedTime is specified or value is float.NegativeInfinity, the state will either be played from the start if it's not already playing, or will continue playing from its current time.")]
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
			m_Animator.Play (stateName.Value, layer.Value, normalizedTime.Value);
			return TaskStatus.Success;
		}
	}
}
