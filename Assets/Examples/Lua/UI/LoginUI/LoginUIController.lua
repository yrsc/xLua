LoginUIController = class("LoginUIController")

local cs =  CS.xLuaSimpleFramework

function LoginUIController:OpenLoginPanel()
  local loginGo = UnityLoader:Instantiate(constant.RES_ROOT_PATH.."UI/Login/Login.prefab")
  self.loginUI = loginGo:GetComponent("UIRootHandler").luaScript;
  self.loginUI:Init(self)
end

function LoginUIController:OnLogin()
  local levelLoader = cs.LevelLoadMgr
  levelLoader.LoadLevelSync("Battle")
end


