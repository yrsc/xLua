using UnityEngine;
using System.Collections;
using UnityEditor;

public class AssetBundleImporter : AssetPostprocessor {

	static string prefix = "Assets/";
	public static int strlenOfAssets = prefix.Length;

	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)   
	{  
		//assetbundle can not contain .cs only
		foreach (var str in importedAssets)  
		{  
			//the file which in ABResources folder can be set as assetbundle
			if(!str.EndsWith(".cs") && str.StartsWith("Assets/Examples/ABResources"))  
			{  				
				AssetImporter importer = AssetImporter.GetAtPath(str);  
				importer.assetBundleName = str.Substring(strlenOfAssets);  
			}  
		}

		foreach (var str in movedAssets)  
		{  
			Debug.Log("str is " + str);
			if(!str.EndsWith(".cs") && str.StartsWith("Assets/Examples/ABResources"))  
			{  
				AssetImporter importer = AssetImporter.GetAtPath(str);  
				importer.assetBundleName = str.Substring(strlenOfAssets);  
			}  
		}  
	}  		

}
