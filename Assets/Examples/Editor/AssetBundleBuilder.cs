using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;

public class AssetNode
{
	public List<AssetNode> parents = new List<AssetNode>();
	public string path;
	public int depth = 0;
}

public class AssetBundleBuilder : Editor 
{	
	//需要打包的资源路径，通常是prefab,lua,及其他数据。（贴图，动画，模型，材质等可以通过依赖自己关联上，不需要添加在该路径里，除非是特殊需要）
	//注意这里是目录，单独零散的文件，可以新建一个目录，都放在里面打包
	public static List<string> abResourcePath = new List<string>()
	{
		"Examples/Prefab",
		//"Examples/Lua",
	};
		
	[MenuItem("AssetBundle/BuildBundleWithBuildMap")]
	public static void BuildAssetBundle()
	{
		CollectDependcy();
		BuildAllBundle();
	}

	static void CollectDependcy()
	{
		for(int i = 0; i < abResourcePath.Count;i++)
		{
			string path = Application.dataPath + "/" + abResourcePath[i];
			if(!Directory.Exists(path))
			{
				Debug.LogError(string.Format("abResourcePath {0} not exist",abResourcePath[i]));
			}
			else
			{
				DirectoryInfo dir = new DirectoryInfo(path);
				FileInfo []files = dir.GetFiles("*", SearchOption.AllDirectories);			
				for (int j = 0; j < files.Length; j++)
				{		
					if(files[j].Name.EndsWith(".meta"))
						continue;
					//获得在文件在Assets下的目录，类似于Assets/Prefab/UI/xx.prefab
					string fileRelativePath = files[j].FullName.Substring(Application.dataPath.Length-6);
					AssetNode root = new AssetNode();
					root.path = fileRelativePath;
					GetDependcyRecursive(fileRelativePath,root);
				}	
			}
		}
		PrintDependcy();
	}

	private static List<AssetNode> _leafNodes = new List<AssetNode>();
	private static Dictionary<string,AssetNode> _allAssetNodes = new Dictionary<string, AssetNode>();

	static void GetDependcyRecursive(string path,AssetNode parentNode)
	{
		string []dependcy = AssetDatabase.GetDependencies(path,false);
		for(int i = 0; i < dependcy.Length; i++)
		{
			AssetNode node = null;
			_allAssetNodes.TryGetValue(dependcy[i],out node);
			if(node == null)
			{
				node = new AssetNode();
				node.path = dependcy[i];
				node.depth = parentNode.depth + 1;
				node.parents.Add(parentNode);
				_allAssetNodes[node.path] = node;
			}
			else
			{
				if(node.depth < parentNode.depth + 1)
				{
					node.depth = parentNode.depth + 1;
				}
				node.parents.Add(parentNode);
			}
			Debug.Log("dependcy path is " +dependcy[i] + " parent is " + parentNode.path);
			GetDependcyRecursive(dependcy[i],node);
		}
		if(dependcy.Length == 0)
		{			
			if(!_leafNodes.Contains(parentNode))
			{
				_leafNodes.Add(parentNode);
			}
		}
	}

	static void PrintDependcy()
	{
		for(int i = 0; i < _leafNodes.Count; i++)
		{
			PrintDependcyRecursive(_leafNodes[i]);
		}
	}

	static void PrintDependcyRecursive(AssetNode node)
	{
		for(int i = 0; i < node.parents.Count;i++)
		{
			PrintDependcyRecursive(node.parents[i]);
		}
		Debug.Log(node.path);
	}
		
	//按照依赖关系的深度，从最底层往上遍历打包，如果一个叶子节点有多个父节点，则该叶子节点被多个资源依赖，该叶子节点需要打包
	static void BuildAllBundle()
	{
		int maxDepth = GetMaxDepthOfLeafNodes();
		while(_leafNodes.Count > 0)
		{
			List<AssetNode> _curDepthNode = new List<AssetNode>();
			for(int i = 0; i < _leafNodes.Count;i++)
			{
				if(_leafNodes[i].depth == maxDepth)
				{
					Debug.Log("node is " + _leafNodes[i].path + " depth " + maxDepth);
					_curDepthNode.Add(_leafNodes[i]);
				}
			}
			//删除已经遍历过的叶子节点，并把这些叶子节点的父节点添加到新一轮的叶子节点中
			for(int i = 0; i < _curDepthNode.Count;i++)
			{
				_leafNodes.Remove(_curDepthNode[i]);
				foreach(AssetNode node in _curDepthNode[i].parents)
				{
					if(!_leafNodes.Contains(node))
					{
						_leafNodes.Add(node);
					}
				}
			}
			maxDepth -= 1;
		}
	}

	static int GetMaxDepthOfLeafNodes()
	{
		_leafNodes.Sort((x,y)=> 
			{
				return y.depth - x.depth;
			});
		return _leafNodes[0].depth;
	}
}