using UnityEngine;
using System.Collections;
using UnityEditor;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class UpdateFile
{
	public Dictionary<string,string> files = new Dictionary<string, string>();
	public float resVersion = 0.0f;
}

public class VersionBuilder : Editor 
{
	public static string AssetBundle_Path = Application.dataPath + "/StreamingAssets/AssetBundle";
	public static string Root_Path = Application.dataPath + "/Examples";
	public static string ABResource_Path = Root_Path+ "/ABResources";

	private static string _versionMd5FilesPath = Application.dataPath + "/Examples/VersionFiles/";
	private static string _versionMd5FileName = "AssetbudleMd5File.txt";
	private static string _updateFileName = "UpdateFile.txt";

	static VersionBuilder()
	{
		#if UNITY_IOS
			_versionMd5FilesPath += "iOS/";
		#elif UNITY_ANDROID
			_versionMd5FilesPath += "Android/";
		#endif
		_versionMd5FileName = _versionMd5FilesPath + _versionMd5FileName;
		_updateFileName = _versionMd5FilesPath + _updateFileName;
	}

	[MenuItem("AssetBundle/SetWholeAssetBundleInABResources")]
	static void SetAssetBundle()
	{
		Debug.Log("Begin Set Assetbundle------------");
		DirectoryInfo dir = new DirectoryInfo(ABResource_Path);

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

	[MenuItem("AssetBundle/CleanBuildApp(we need a new app)")]
	static void BuildGamePackage()
	{
		CleanBuildAllBundle();
		CleanAndWriteNewVersion();
	}

	[MenuItem("AssetBundle/FastBuildApp(we need a new app)")]
	static void FastBuildGamePackage()
	{
		BuildAllBundle();
		CleanAndWriteNewVersion();
	}

	static void CleanAndWriteNewVersion()
	{
		CleanVersionFiles();
		if(GenerateAllBundleMd5Now())
		{
			WriteVersionMd5Files();
		}
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}


	[MenuItem("AssetBundle/CleanBuildAssetbundle(we need update assetbundles)")]
	static void BuildAssetbundle()
	{
		if(!File.Exists(_versionMd5FileName))
		{
			Debug.LogError("Can not find last version,you may CleanBuildApp first if you want to update assetbundles based on last version");
			return;
		}
		CleanBuildAllBundle();
		GenerateUpdateFiles();
	}

	[MenuItem("AssetBundle/FastBuildAssetbundle(we need update assetbundles)")]
	static void FastBuildAssetbundle()
	{
		if(!File.Exists(_versionMd5FileName))
		{
			Debug.LogError("Can not find last version,you may CleanBuildApp first if you want to update assetbundles based on last version");
			return;
		}
		BuildAllBundle();
		GenerateUpdateFiles();
	}
	static void GenerateUpdateFiles()
	{
		if(!LoadLastVersionMd5())
		{
			Debug.LogError("Load last version md5 file failed!");
			return;
		}
		if(!GenerateAllBundleMd5Now())
		{
			Debug.LogError("Generate new version md5 file failed!");
			return;
		}
		if(!LoadLastUpdateFile())
		{
			Debug.LogError("Load last update file failed!");
			return;
		}
		List<string> needUpdateFileList = GenerateDifferentFilesList();
		List<string> needDeleteFileList = GenerateNeedDeleteFilesList();
		if(needUpdateFileList.Count == 0 && needDeleteFileList.Count == 0)
		{
			Debug.LogError("nothing need update");
			return;
		}
		//set update files version
		float newVersion = GetNewResVersion();
		for(int i = 0; i < needUpdateFileList.Count;i++)
		{
			_updateFiles.files[needUpdateFileList[i]] = string.Format("{0:N1}",newVersion);
		}
		//delete unused assetbundles
		for(int i = 0; i < needDeleteFileList.Count; i++)
		{
			if(_updateFiles.files.ContainsKey(needDeleteFileList[i]))
			{
				_updateFiles.files.Remove(needDeleteFileList[i]);
			}
		}
		WriteUpdateFiles();
		WriteVersionMd5Files();

		//export update assetbundles
		ExportUpdateAssetbundle(needUpdateFileList);

		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	static void CleanVersionFiles()
	{
		if(Directory.Exists(_versionMd5FilesPath))
		{
			Directory.Delete(_versionMd5FilesPath,true);
		}
	}

	static void WriteVersionMd5Files()
	{
		if(!Directory.Exists(_versionMd5FilesPath))
		{
			Directory.CreateDirectory(_versionMd5FilesPath);
		}
		StringBuilder sb = new StringBuilder();
		foreach(KeyValuePair<string,string> kvp in _allBundleMd5Now)
		{
			string content = kvp.Key + "," + kvp.Value + "\n";
			sb.Append(content);
		}
		File.WriteAllText(_versionMd5FileName,sb.ToString(),System.Text.Encoding.UTF8);	
	}


	private static Dictionary<string,string> _allBundleMd5LastVersion = new Dictionary<string, string>();

	static bool LoadLastVersionMd5()
	{
		try
		{
			_allBundleMd5LastVersion.Clear();
			string[] content = File.ReadAllLines(_versionMd5FileName);
			if(content != null)
			{
				for(int i = 0; i < content.Length; i++)
				{
					if(!string.IsNullOrEmpty(content[i]))
					{
						string []kvp = content[i].Split(',');
						if(kvp == null || kvp.Length < 2)
						{
							Debug.LogError("Can not parse last md5 file with content " + content[i]);
							return false;
						}
						_allBundleMd5LastVersion[kvp[0]] = kvp[1];
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception("LoadLastVersionMd5 Error:" + ex.Message);
			return false;
		}
		return true;
	}

	private static Dictionary<string,string> _allBundleMd5Now = new Dictionary<string, string>();
	static bool GenerateAllBundleMd5Now()
	{
		_allBundleMd5Now.Clear();
		if(!Directory.Exists(AssetBundle_Path))
		{
			Debug.LogError(string.Format("assetbundle path {0} not exist",AssetBundle_Path));
			return false;
		}
		DirectoryInfo dir = new DirectoryInfo(AssetBundle_Path);
		var files = dir.GetFiles("*", SearchOption.AllDirectories);
		for (var i = 0; i < files.Length; ++i)
		{
			try
			{
				if(files[i].Name.EndsWith(".meta") || files[i].Name.EndsWith(".manifest") )
					continue;
				FileStream file = new FileStream(files[i].FullName, FileMode.Open);
				System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
				byte[] retVal = md5.ComputeHash(file);
				file.Close();
				StringBuilder sb = new StringBuilder();
				for (int j = 0; j < retVal.Length; j++)
				{
					sb.Append(retVal[j].ToString("x2"));
				}
				string fileRelativePath = files[i].FullName.Substring(AssetBundle_Path.Length+1);
				_allBundleMd5Now[fileRelativePath] = sb.ToString();
			}
			catch (Exception ex)
			{
				throw new Exception("GetMD5HashFromFile fail,error:" + ex.Message);
				return false;
			}
		}
		return true;
	}

	private static UpdateFile _updateFiles = new UpdateFile();
	static List<string> GenerateDifferentFilesList()
	{ 
		List<string> _needUpdateFileList = new List<string>();
		foreach(KeyValuePair<string,string> kvp in _allBundleMd5Now)
		{
			if(_allBundleMd5LastVersion.ContainsKey(kvp.Key))
			{
				if(_allBundleMd5LastVersion[kvp.Key] == kvp.Value)
				{
					continue;
				}
				else
				{
					_needUpdateFileList.Add(kvp.Key);
				}
			}
			else
			{
				_needUpdateFileList.Add(kvp.Key);
			}
		}
		return _needUpdateFileList;
	}

	static List<string> GenerateNeedDeleteFilesList()
	{ 
		List<string> _needDeleteFileList = new List<string>();
		foreach(KeyValuePair<string,string> kvp in _allBundleMd5LastVersion)
		{
			if(_allBundleMd5Now.ContainsKey(kvp.Key))
			{
				continue;
			}
			else
			{
				_needDeleteFileList.Add(kvp.Key);
			}
		}
		return _needDeleteFileList;
	}

	static bool LoadLastUpdateFile()
	{
		_updateFiles = new UpdateFile();
		if(File.Exists(_updateFileName))
		{
			try
			{				
				string[] content = File.ReadAllLines(_updateFileName);
				if(content != null)
				{
					for(int i = 0; i < content.Length - 1; i++)
					{
						if(!string.IsNullOrEmpty(content[i]))
						{
							string []kvp = content[i].Split(',');
							if(kvp == null || kvp.Length < 2)
							{
								Debug.LogError("Can not parse last update file with content " + content[i]);
								return false;
							}
							_updateFiles.files[kvp[0]] = kvp[1];
						}
					}
					_updateFiles.resVersion = float.Parse(content[content.Length - 1]);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Load UpdateFile Error:" + ex.Message);
				return false;
			}
		}
		return true;
	}

	static float GetNewResVersion()
	{
		if(_updateFiles != null)
			return _updateFiles.resVersion + 0.1f;
		return 0.0f;
	}
		
	private static string _exportPath = "";
	private static string _appVersion = "1.0";
	static void ExportUpdateAssetbundle(List<string> updatedAssetbundles)
	{		
		if(updatedAssetbundles.Count > 0)
		{
			_exportPath = Application.dataPath.Substring(0,Application.dataPath.LastIndexOf("/")+1) + "NewAssetbundles";
			#if UNITY_IOS
			_exportPath += "/iOS";
			#elif UNITY_ANDROID
			_exportPath += "/Android";
			#endif
			_exportPath = string.Format("{0}/{1}/{2}",_exportPath,_appVersion,GetNewResVersion());
			for(int i = 0; i < updatedAssetbundles.Count;i++)
			{
				string assetbundleDestPath = _exportPath + "/" + updatedAssetbundles[i];
				string assetbundleSrcPath = AssetBundle_Path + "/" + updatedAssetbundles[i];
				string destDir = Path.GetDirectoryName(assetbundleDestPath);
				if(!Directory.Exists(destDir))
				{
					Directory.CreateDirectory(destDir);
				}
				File.Copy(assetbundleSrcPath,assetbundleDestPath,true);
			}
		}
		string updateFileSrcPath = _updateFileName;
		string updateFileDestPath = _exportPath + "/" +Path.GetFileName(updateFileSrcPath);
		string updateFileDestDir = Path.GetDirectoryName(updateFileDestPath);
		if(!Directory.Exists(updateFileDestDir))
		{
			Directory.CreateDirectory(updateFileDestDir);
		}
		File.Copy(updateFileSrcPath,updateFileDestPath,true);
	}

	static void WriteUpdateFiles()
	{			
		StringBuilder sb = new StringBuilder();		
		foreach(KeyValuePair<string,string> kvp in _updateFiles.files)
		{
			string content = kvp.Key + "," + kvp.Value + "\n";
			sb.Append(content);
		}
		sb.Append(GetNewResVersion());
		File.WriteAllText(_updateFileName,sb.ToString(),System.Text.Encoding.UTF8);	

	}



	static void CleanBuildAllBundle()
	{
		if(Directory.Exists(AssetBundle_Path))
		{
			Directory.Delete(AssetBundle_Path,true);
		}
		BuildAllBundle();
	}

	static void BuildAllBundle()
	{		
		if(!Directory.Exists(AssetBundle_Path))
		{
			Directory.CreateDirectory(AssetBundle_Path);
		}

		ReplaceLuaWithTxt();
			
		BuildPipeline.BuildAssetBundles (AssetBundle_Path, BuildAssetBundleOptions.ChunkBasedCompression |
			BuildAssetBundleOptions.DeterministicAssetBundle,EditorUserBuildSettings.activeBuildTarget);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		Debug.Log("Finish Build AssetBundle With Platfrom " + EditorUserBuildSettings.activeBuildTarget);

		//GenerateUpdateFile();

	}

	static void ReplaceLuaWithTxt()
	{
		Debug.Log("Begin Replace Lua With Txt");

		string luaPath = Root_Path + "/Lua";
		DeleteLuaInABResource(luaPath,ABResource_Path);
		DirectoryInfo dir = new DirectoryInfo(luaPath);
		var files = dir.GetFiles("*.lua", SearchOption.AllDirectories);
		for (var i = 0; i < files.Length; ++i)
		{			
			EditorUtility.DisplayProgressBar("replace lua to txt", "replace lua to txt...", 1f * i / files.Length);
			var fileInfo = files[i];
			string str = File.ReadAllText(fileInfo.FullName);
			string txtDir = ABResource_Path + fileInfo.DirectoryName.Substring(Root_Path.Length);
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
		DeleteLuaInABResource(luaPath,ABResource_Path);
		Debug.Log("Finish Replace Lua With Txt");

	}

	static void DeleteLuaInABResource(string luaPath,string abResPath)
	{
		string luaInABResPath = abResPath + luaPath.Substring(Root_Path.Length);
		if(Directory.Exists(luaInABResPath))
		{
			Directory.Delete(luaInABResPath,true);
		}
	}

}
