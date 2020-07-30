using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Actions.UnityImage
{
	[Category ("UnityEngine/UI/Image")]
	[Tooltip ("Amount of the Image shown when the Image.type is set to Image.Type.Filled.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Image-fillAmount.html")]
	public class SetFillAmount: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public FloatVariable m_FillAmount;

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
			m_Image.fillAmount = m_FillAmount.Value;
			return TaskStatus.Success;
		}
	}
}
