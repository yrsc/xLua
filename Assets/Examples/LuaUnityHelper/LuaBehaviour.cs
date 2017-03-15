using UnityEngine;
using System.Collections;
using System;
using XLua;

namespace xLuaSimpleFramework
{	
	[LuaCallCSharp]
	public class LuaBehaviour : MonoBehaviour {

		protected Action<LuaTable> _luaStart;
		protected Action<LuaTable> _luaUpdate;
		protected Action<LuaTable> _luaOnDestroy;
		protected Action<LuaTable> _luaOnEnable;
		protected Action<LuaTable> _luaOnDisable;

		protected LuaTable _scriptEnv;
		public LuaTable luaScript
		{
			get{return _scriptEnv;}
		}

		public string luaScrpitPath;
	
		protected void Awake()
		{						
			if(!string.IsNullOrEmpty(luaScrpitPath))
			{

				LuaEnv luaEnv = LuaBoot.luaEnv;
				//_scriptEnv = luaEnv.NewTable();
				//LuaTable meta = luaEnv.NewTable();
				//meta.Set("__index",luaEnv.Global);
				//_scriptEnv.SetMetaTable(meta);
				//meta.Dispose();
				//_scriptEnv.Set("self", this);
				string luaStr = SimpleLoader.LoadLua(luaScrpitPath);
			//	string luaStr = string.Format("require('{0}')",luaScrpitPath);
			//	Debug.Log("lua str is " + luaStr);
				_scriptEnv = null;
				object []ret = luaEnv.DoString(luaStr, "LuaBehaviour");
				if(ret != null)
				{
					_scriptEnv = ret[0] as LuaTable;
					_scriptEnv.Set("monobehaviour",this);
					Action<LuaTable> luaAwake = _scriptEnv.Get<Action<LuaTable>>("Awake");
					_scriptEnv.Get("Start", out _luaStart);
					_scriptEnv.Get("Update", out _luaUpdate);
					_scriptEnv.Get("OnDestroy", out _luaOnDestroy);
					_scriptEnv.Get("OnEnable", out _luaOnEnable);
					_scriptEnv.Get("OnDisable", out _luaOnDisable);
					if (luaAwake != null)
					{
						luaAwake(_scriptEnv);
					}				
				}
			

			}

		}

		protected void OnEnable()
		{
			if(_luaOnEnable != null)
			{
				_luaOnEnable(_scriptEnv);
			}
		}

		protected void OnDisable()
		{
			if(_luaOnDisable != null)
			{
				_luaOnDisable(_scriptEnv);
			}
		}


		// Use this for initialization
		protected void Start () {
			if(_luaStart != null)
			{
				_luaStart(_scriptEnv);
			}
		}
		
		// Update is called once per frame
		protected void Update () 
		{
			if(_luaUpdate != null)
			{
				_luaUpdate(_scriptEnv);
			}
		}

		protected void OnDestroy()
		{
			if (_luaOnDestroy != null)
			{
				_luaOnDestroy(_scriptEnv);
			}
			_luaOnDestroy = null;
			_luaStart = null;
			_luaUpdate = null;
			_luaOnEnable = null;
			_luaOnDisable = null;
			if(_scriptEnv != null)
			{
				_scriptEnv.Dispose();
			}
			//injections = null;
		}
	}
}