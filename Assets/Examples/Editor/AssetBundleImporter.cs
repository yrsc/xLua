using UnityEngine;
using System.Collections;
using UnityEditor;

public class AssetBundleImporter : AssetPostprocessor {

	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)   
	{  
		//assetbundle can not contain .cs only
		foreach (var str in importedAssets)  
		{  
			if(!str.EndsWith(".cs"))  
			{  
				AssetImporter importer = AssetImporter.GetAtPath(str);  
				importer.assetBundleName = str;  
			}  
		}  

		foreach (var str in movedAssets)  
		{  
			if(!str.EndsWith(".cs"))  
			{  
				AssetImporter importer = AssetImporter.GetAtPath(str);  
				importer.assetBundleName = str;  
			}  
		}  
	}  


}
