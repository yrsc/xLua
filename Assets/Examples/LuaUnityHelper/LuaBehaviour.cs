﻿using UnityEngine;
using System.Collections;
using System;
using XLua;

namespace xLuaSimpleFramework
{	
	[LuaCallCSharp]
	public class LuaBehaviour : MonoBehaviour {

		protected Action _luaStart;
		protected Action _luaUpdate;
		protected Action _luaOnDestroy;
		protected Action _luaOnEnable;
		protected Action _luaOnDisable;

		protected LuaTable _scriptEnv;

		public string luaScrpitPath;
	
		protected void Awake()
		{			
			if(!string.IsNullOrEmpty(luaScrpitPath))
			{

				LuaEnv luaEnv = LuaBoot.luaEnv;
				_scriptEnv = luaEnv.NewTable();
				LuaTable meta = luaEnv.NewTable();
				meta.Set("__index",luaEnv.Global);
				_scriptEnv.SetMetaTable(meta);
				meta.Dispose();
				_scriptEnv.Set("self", this);
            //    string luaPathDir = Constant.luaRootPath + luaScrpitPath;
//                string luaStr = SimpleLoader.LoadFileToStr(luaPathDir,"lua");
				string luaStr = string.Format("require('{0}')",luaScrpitPath);
				Debug.Log("lua str is " + luaStr);
				luaEnv.DoString(luaStr, "LuaBehaviour",_scriptEnv);
				Action luaAwake = _scriptEnv.Get<Action>("Awake");
				_scriptEnv.Get("Start", out _luaStart);
				_scriptEnv.Get("Update", out _luaUpdate);
				_scriptEnv.Get("OnDestroy", out _luaOnDestroy);
				_scriptEnv.Get("OnEnable", out _luaOnEnable);
				_scriptEnv.Get("OnDisable", out _luaOnDisable);
				if (luaAwake != null)
				{
					luaAwake();
				}
			}

		}

		protected void OnEnable()
		{
			if(_luaOnEnable != null)
			{
				_luaOnEnable();
			}
		}

		protected void OnDisable()
		{
			if(_luaOnDisable != null)
			{
				_luaOnDisable();
			}
		}


		// Use this for initialization
		protected void Start () {
			if(_luaStart != null)
			{
				_luaStart();
			}
		}
		
		// Update is called once per frame
		protected void Update () 
		{
			if(_luaUpdate != null)
			{
				_luaUpdate();
			}
		}

		protected void OnDestroy()
		{
			if (_luaOnDestroy != null)
			{
				_luaOnDestroy();
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