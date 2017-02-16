using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace XLuaTest
{
	public class UIComponent : LuaBehaviour 
	{
		public UIRootHandler rootHanlder;

		void Awake()
		{
			if(rootHanlder != null)
			{
				rootHanlder.Resgister(this);
			}

		}			
	}
}