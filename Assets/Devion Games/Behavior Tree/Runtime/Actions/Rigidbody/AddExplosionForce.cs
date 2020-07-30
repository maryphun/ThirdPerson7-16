using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames.BehaviorTrees.Actions.UnityRigidbody
{
	[Category ("UnityEngine/Rigidbody")]
	[Tooltip ("Applies a force to a rigidbody that simulates explosion effects.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Rigidbody.AddExplosionForce.html")]
	public class AddExplosionForce: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		[Tooltip ("The force of the explosion (which may be modified by distance).")]
		public FloatVariable explosionForce;
		[Tooltip ("The centre of the sphere within which the explosion has its effect.")]
		public Vector3Variable explosionPosition;
		[Tooltip ("The radius of the sphere within which the explosion has its effect.")]
		public FloatVariable explosionRadius;
		[Tooltip ("Adjustment to the apparent position of the explosion to make it seem to lift objects.")]
		public FloatVariable upwardsModifier;
		[Tooltip ("The method used to apply the force to its targets.")]
		public ForceMode mode;

		private GameObject m_PrevGameObject;
		private Rigidbody m_Rigidbody;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_Rigidbody = m_gameObject.Value.GetComponent<Rigidbody> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_Rigidbody == null) {
				Debug.LogWarning ("Missing Component of type Rigidbody!");
				return TaskStatus.Failure;
			}
			m_Rigidbody.AddExplosionForce (explosionForce, explosionPosition, explosionRadius, upwardsModifier, mode);
			return TaskStatus.Success;
		}
	}
}
