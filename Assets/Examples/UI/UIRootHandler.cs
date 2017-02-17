using UnityEngine;
using System.Collections.Generic;

namespace xLuaSimpleFramework
{
	public class UIRootHandler : LuaBehaviour
	{
		private Dictionary<string,UIComponent> _uiComponents = new Dictionary<string, UIComponent>();
		public Dictionary<string,UIComponent> uiComponents
		{
			get{return _uiComponents;}
		}

		public void Resgister(UIComponent uiComponent)
		{			
			if(uiComponent != null)
			{
				_uiComponents[uiComponent.name] = uiComponent;
			}
		}

		/*
		void Start()
		{
			Dictionary<string,UIComponent>.Enumerator iter = _uiComponents.GetEnumerator();
			while(iter.MoveNext())
			{
				
			}
			base.Start();
		}*/
	}
}