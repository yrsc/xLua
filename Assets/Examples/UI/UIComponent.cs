using XLua;

namespace xLuaSimpleFramework
{
	[LuaCallCSharp]	
	public class UIComponent : LuaBehaviour 
	{
		public UIRootHandler rootHanlder;

		new void Awake()
		{
			base.Awake();
			if(rootHanlder != null)
			{
				rootHanlder.Resgister(this);
			}
		}			
	}
}