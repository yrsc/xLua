using UnityEngine;
using System.Collections;

namespace xLuaSimpleFramework
{
	public class AssetbundleLoader
	{
		public static string ROOT_PATH = Application.dataPath + "/StreamingAssets/AssetBundle/";
		private const string MANIFEST_SUFFIX = ".manifest";

		private static AssetBundleManifest _manifest;

		static AssetbundleLoader()
		{
			#if UNITY_EDITOR
				ROOT_PATH = Application.dataPath + "/StreamingAssets/AssetBundle/";
			#elif UNITY_IPHONE
				ROOT_PATH = Application.dataPath + "/Raw/AssetBundle/";
			#elif UNITY_ANDROID
				ROOT_PATH = Application.streamingAssetsPath + "/AssetBundle/";
			#endif
		}

		static AssetBundle LoadAssetBundleDependcy(string path)
		{        
			if(_manifest == null)
			{
				LoadManifest();
			}
			if(_manifest != null)
			{
				string [] dependencies = _manifest.GetAllDependencies(path);
				if(dependencies.Length > 0)
				{
					//load all dependencies
					for(int i = 0; i < dependencies.Length; i++)
					{
						//Debug.Log(" load dependencies " + dependencies[i]);
						LoadAssetBundle(dependencies[i]);
					}

				}
				//load self
				return LoadAssetBundle(path);
			}
			return null;
		}

		static AssetBundle LoadAssetBundle(string path)
		{
			//all characters in assetbundle are lower characters
			path = path.ToLower();
			Debug.Log("path is " + path);
			AssetBundle bundle = AssetBundle.LoadFromFile(ROOT_PATH + path);
			return bundle;
		}

		public static T LoadRes<T>(string path) where T : Object
		{			
			AssetBundle bundle = LoadAssetBundleDependcy(path);
			if(bundle != null)
			{
				int assetNameStart = path.LastIndexOf("/")+1;
				int assetNameEnd = path.LastIndexOf(".");
				string assetName = path.Substring(assetNameStart,assetNameEnd - assetNameStart);
				T obj = bundle.LoadAsset(assetName) as T;
				bundle.Unload(false);
				return obj;
			}
			return null;
		}	

		public static T LoadRes<T>(string name,AssetBundle bundle) where T : Object
		{			
			T obj = bundle.LoadAsset<T>(name);
			return obj;
		}	

		static void LoadManifest()
		{
			AssetBundle bundle = AssetBundle.LoadFromFile(ROOT_PATH + "AssetBundle");
			UnityEngine.Object obj = bundle.LoadAsset("AssetBundleManifest");
			bundle.Unload(false);
			_manifest = obj as AssetBundleManifest;
			Debug.Log("Load Manifest Finished");
		}
	}
}