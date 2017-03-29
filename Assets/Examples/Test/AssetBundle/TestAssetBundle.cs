using UnityEngine;
using System.Collections;
using xLuaSimpleFramework;

public class TestAssetBundle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	AssetBundle bundle = null;
	GameObject character = null;
	void OnGUI()
	{
		if(GUI.Button(new Rect(Screen.width/2,100,100,100),"create"))
		{
			bundle = AssetbundleLoader.LoadAssetBundleDependcy("Examples/ABResources/Prefab/Character/Cha_Bow_003.prefab");
			GameObject go = AssetbundleLoader.LoadRes<GameObject>("Cha_Bow_003",bundle);
			character = GameObject.Instantiate(go);

		}
		if(GUI.Button(new Rect(Screen.width/2,200,100,100),"destroy"))
		{
			if(character != null && bundle != null)
			{
				GameObject.Destroy(character);
				//bundle.Unload(true);
			}
			Resources.UnloadUnusedAssets();

		}
	}
}
