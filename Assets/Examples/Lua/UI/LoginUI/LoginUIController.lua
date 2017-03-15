LoginUIController = class("LoginUIController")

local cs =  CS.xLuaSimpleFramework

function LoginUIController:OpenLoginPanel()
  local loginGo = UnityLoader:Instantiate("Assets/Examples/ABResrouces/UI/Login.prefab")
  self.loginUI = loginGo:GetComponent("UIRootHandler").luaScript;
  self.loginUI:Init(self)
end

function LoginUIController:OnLogin()
  local levelLoader = cs.LevelLoadMgr
  levelLoader.LoadLevelSync("Battle")
end


