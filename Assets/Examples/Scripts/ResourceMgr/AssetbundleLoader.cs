﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace xLuaSimpleFramework
{
	public class AssetbundleLoader
	{
		public static string ROOT_PATH = Application.dataPath + "/StreamingAssets/AssetBundle/";
		private const string MANIFEST_SUFFIX = ".manifest";
		public static string Download_Path = Application.persistentDataPath + "/AssetBundle/";
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

		private static Dictionary<string,AssetBundle> _assetbundleDic = new Dictionary<string, AssetBundle>();

		public static AssetBundle LoadAssetBundleDependcy(string path)
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
			AssetBundle bundle = null;
			//cache bundles to ignore load the same bundle
			_assetbundleDic.TryGetValue(path,out bundle);
			if(bundle != null)
			{
				return bundle;
			}
			string downloadBundlePath = Download_Path+path;
			if(File.Exists(downloadBundlePath))
			{
				bundle = AssetBundle.LoadFromFile(downloadBundlePath);
			}
			else
			{
				bundle = AssetBundle.LoadFromFile(ROOT_PATH + path);
			}
			_assetbundleDic[path] = bundle;
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