using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DevionGames.BehaviorTrees.Conditionals.UnityButton
{
	[Category ("UnityEngine/UI/Button")]
	public class OnClick : Conditional
	{
		[Tooltip ("GameObject to operate on.")]
		public GameObjectVariable m_gameObject;

		private bool m_Clicked;
		private GameObject m_PrevGameObject;
		private Button m_Button;

		public override void OnStart ()
		{
			this.m_Clicked = false;
			if (m_gameObject.Value != null && m_gameObject.Value != m_PrevGameObject) {
				m_PrevGameObject = m_gameObject.Value;
				m_Button = m_gameObject.Value.GetComponent<Button> ();
				m_Button.onClick.AddListener (Clicked);
			}
		}

		public override TaskStatus OnUpdate ()
		{
			return this.m_Clicked ? TaskStatus.Success : TaskStatus.Failure;
		}

		private void Clicked ()
		{

			this.m_Clicked = true;
		}
	}
}