using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Actions.UnityImage
{
	[Category ("UnityEngine/UI/Image")]
	[Tooltip ("Adjusts the image size to make it pixel-perfect.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Image.SetNativeSize.html")]
	public class SetNativeSize: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;

		private GameObject m_PrevGameObject;
		private Image m_Image;

		public override void OnStart ()
		{
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_Image = m_gameObject.Value.GetComponent<Image> ();
			}
		}

		public override TaskStatus OnUpdate ()
		{
			if (m_Image == null) {
				Debug.LogWarning ("Missing Component of type Image!");
				return TaskStatus.Failure;
			}
			m_Image.SetNativeSize ();
			return TaskStatus.Success;
		}
	}
}
