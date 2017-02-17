using UnityEngine;
using System.Collections;
using System.IO;

namespace xLuaSimpleFramework
{
    public class SimpleLoader
    {
        public static T Load<T>(string path) where T : Object
        {
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
        }

        public static string LoadFileToStr(string path,string suffix)
        {
            string luaPath = Application.dataPath +"/" + path + "." + suffix;
            return File.ReadAllText(luaPath);
        }
    }
}