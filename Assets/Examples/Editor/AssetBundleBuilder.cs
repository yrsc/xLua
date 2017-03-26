using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.IO;

public class AssetBundleBuilder : Editor 
{
	public static string AssetBundle_Path = Application.dataPath + "/StreamingAssets/AssetBundle";
	public static string Root_Path = Application.dataPath + "/Examples";
	[MenuItem("AssetBundle/SetWholeAssetBundleInABResources")]
	static void SetAssetBundle()
	{
		Debug.Log("Begin Set Assetbundle------------");
		string assetbunleResPath = Root_Path+ "/ABResources";
		DirectoryInfo dir = new DirectoryInfo(assetbunleResPath);

		var files = dir.GetFiles("*", SearchOption.AllDirectories);

		for (var i = 0; i < files.Length; ++i)
		{			
			EditorUtility.DisplayProgressBar("设置AssetBndleName名称", "正在设置AssetBundleName名称中...", 1f * i / files.Length);
			var fileInfo = files[i];
			if(!fileInfo.FullName.EndsWith(".meta"))
			{
				string releativePath = fileInfo.FullName.Substring(fileInfo.FullName.IndexOf("Assets"));
				AssetImporter importer = AssetImporter.GetAtPath(releativePath);  
				importer.assetBundleName = releativePath.Substring(AssetBundleImporter.strlenOfAssets);
			}
		}
		EditorUtility.ClearProgressBar();
		Debug.Log("Finish Set Assetbundle------------");

	}

	[MenuItem("AssetBundle/Clean BuildAllBundle")]
	static void CleanBuildAllBundle()
	{
		if(Directory.Exists(AssetBundle_Path))
		{
			Directory.Delete(AssetBundle_Path,true);
		}
		BuildAllBundle();
	}

	[MenuItem("AssetBundle/BuildAllBundle")]
	static void BuildAllBundle()
	{		
		if(!Directory.Exists(AssetBundle_Path))
		{
			Directory.CreateDirectory(AssetBundle_Path);
		}

		ReplaceLuaWithTxt();

		BuildTarget target = BuildTarget.Android;
		#if UNITY_IPHONE
		target = BuildTarget.iOS;
		#endif
		BuildPipeline.BuildAssetBundles (AssetBundle_Path, BuildAssetBundleOptions.ChunkBasedCompression |
			BuildAssetBundleOptions.DeterministicAssetBundle, target);

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		Debug.Log("Finish Build AssetBundle");
	}

	static void ReplaceLuaWithTxt()
	{
		Debug.Log("Begin Replace Lua With Txt");

		string luaPath = Root_Path + "/Lua";
		string abResPath = Root_Path+ "/ABResources";
		DeleteOldLua(luaPath,abResPath);
		DirectoryInfo dir = new DirectoryInfo(luaPath);
		var files = dir.GetFiles("*.lua", SearchOption.AllDirectories);
		for (var i = 0; i < files.Length; ++i)
		{			
			EditorUtility.DisplayProgressBar("replace lua to txt", "replace lua to txt...", 1f * i / files.Length);
			var fileInfo = files[i];
			string str = File.ReadAllText(fileInfo.FullName);
			string txtDir = abResPath + fileInfo.DirectoryName.Substring(Root_Path.Length);
			if(!Directory.Exists(txtDir))
			{
				Directory.CreateDirectory(txtDir);
			}
			string luaName = fileInfo.Name;
			string txtName = luaName.Replace(".lua",".txt");
			string writePath = txtDir + "/" + txtName;
			File.WriteAllText(writePath,str,System.Text.Encoding.UTF8);				
		}
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		EditorUtility.ClearProgressBar();
		DeleteOldLua(luaPath,abResPath);
		Debug.Log("Finish Replace Lua With Txt");

	}

	static void DeleteOldLua(string luaPath,string abResPath)
	{
		string luaInABResPath = abResPath + luaPath.Substring(Root_Path.Length);
		if(Directory.Exists(luaInABResPath))
		{
			Directory.Delete(luaInABResPath,true);
		}
	}


}
