﻿using XLua;
//using UnityEngine.UI;

namespace xLuaSimpleFramework
{
	[LuaCallCSharp]	
	public class UIComponent : LuaBehaviour 
	{
		public UIRootHandler rootHanlder;

		new void Awake()
		{
			if(rootHanlder != null)
			{
				rootHanlder.Resgister(this);
			}
		}			
	}
}