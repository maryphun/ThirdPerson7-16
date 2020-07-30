using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Actions.UnityImage
{
	[Category ("UnityEngine/UI/Image")]
	[Tooltip ("The sprite that is used to render this image.")]
	[HelpURL ("https://docs.unity3d.com/ScriptReference/Image-sprite.html")]
	public class SetSprite: Action
	{
		[Tooltip ("The game object to operate on.")]
		public GameObjectVariable m_gameObject;
		public SpriteVariable m_Sprite;

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
		
			m_Image.sprite = m_Sprite.Value;
			return TaskStatus.Success;
		}
	}
}
