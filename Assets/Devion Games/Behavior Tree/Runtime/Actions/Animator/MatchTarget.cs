using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityAnimator
{
	[Category ("UnityEngine/Animator")]
	[Tooltip ("Automatically adjust the gameobject position and rotation so that the AvatarTarget reaches the matchPosition when the current state is at the specified progress.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Animator.MatchTarget.html")]
	public class MatchTarget: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The position we want the body part to reach.")]
		public Vector3Variable matchPosition;
		[Tooltip ("The rotation in which we want the body part to be.")]
		public Vector3Variable matchRotation;
		[Tooltip ("The body part that is involved in the match.")]
		public AvatarTarget targetBodyPart;
		[Tooltip ("Structure that contains weights for matching position and rotation.")]
		public MatchTargetWeightMask weightMask;
		[Tooltip ("Start time within the animation clip (0 - beginning of clip, 1 - end of clip).")]
		public FloatVariable startNormalizedTime;
		[Tooltip ("End time within the animation clip (0 - beginning of clip, 1 - end of clip), values greater than 1 can be set to trigger a match after a certain number of loops. Ex: 2.3 means at 30% of 2nd loop.")]
		public FloatVariable targetNormalizedTime = 1f;

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
			m_Animator.MatchTarget (matchPosition, Quaternion.Euler (matchRotation), targetBodyPart, weightMask, startNormalizedTime, targetNormalizedTime);
			return TaskStatus.Success;
		}
	}
}
