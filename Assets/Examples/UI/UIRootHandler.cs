using System.Collections.Generic;
using XLua;

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
	}
}