UnityLoader = class("UnityLoader")

local loader = CS.xLuaSimpleFramework.SimpleLoader

function UnityLoader:Instantiate(path)
  return loader.InstantiateGameObject(path)
end
