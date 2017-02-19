using UnityEngine;
using System.Collections;
using System.IO;
using XLua;

namespace xLuaSimpleFramework
{
	[LuaCallCSharp]
    public class SimpleLoader
    {
        public static T Load<T>(string path) where T : Object
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
        }

		public static string LoadLua(string path,string rootPath = Constant.luaRootPath)
		{
			string luaPath = Application.dataPath + "/" + rootPath + path + ".lua";
			return LoadFileToStr(luaPath);
		}

        public static string LoadFileToStr(string path)
        {
			return File.ReadAllText(path);
        }
			
		public static GameObject LoadGameObject(string path)
		{
			return UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
		}

		public static GameObject InstantiateGameObject(string path)
		{
			GameObject go = LoadGameObject(path);
			if(go != null)
				return GameObject.Instantiate(go);
			return null;
		}
    }
}