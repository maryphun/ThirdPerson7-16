using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Actions.UnityImage
{
	[Category ("UnityEngine/UI/Image")]
	[Tooltip ("Controls the origin point of the Fill process. Value means different things with each fill method.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Image-fillOrigin.html")]
	public class SetFillOrigin: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public IntVariable m_FillOrigin;

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
			m_Image.fillOrigin = m_FillOrigin.Value;
			return TaskStatus.Success;
		}
	}
}
