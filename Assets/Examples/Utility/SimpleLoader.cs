using UnityEngine;
using System.Collections;
using System.IO;
using XLua;

namespace xLuaSimpleFramework
{
	[LuaCallCSharp]
    public class SimpleLoader
    {
		public static string RES_ROOT_PATH = "Assets/";
        public static T Load<T>(string path) where T : Object
        {
			#if UNITY_EDITOR && !LOAD_ASSETBUNDLE_INEDITOR
				path = RES_ROOT_PATH + path;
            	return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
			#else				
				return AssetbundleLoader.LoadRes<T>(path);
			#endif
        }

		public static string LoadLua(string path,string rootPath = Constant.luaRootPath)
		{
			#if UNITY_EDITOR && !LOAD_ASSETBUNDLE_INEDITOR
				string luaPath = Application.dataPath + "/" + rootPath + path + ".lua";
				return LoadFileToStr(luaPath);
			#else
				string luaPath = "Examples/ABResources/Lua/" + path + ".txt";
				//the characters of assetbundle path are lower characters
			//	luaPath = luaPath.ToLower();
				TextAsset content = AssetbundleLoader.LoadRes<TextAsset>(luaPath);
				return content.text;
			#endif
		}

        public static string LoadFileToStr(string path)
        {
			return File.ReadAllText(path);
        }

		public static GameObject InstantiateGameObject(string path)
		{
			GameObject go = Load<GameObject>(path);
			if(go != null)
				return GameObject.Instantiate(go);
			return null;
		}			
    }
}