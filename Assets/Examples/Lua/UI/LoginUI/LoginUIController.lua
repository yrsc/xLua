LoginUIController = class("LoginUIController")


function LoginUIController:OpenLoginPanel()
  if(self.loginUI ~= nil) then
    --self.loginUI:Show()
    print("show directly")
  else
    local loader = CS.xLuaSimpleFramework.SimpleLoader
    local loginGo = loader.InstantiateGameObject("Assets/Examples/ABResrouces/UI/Login.prefab")
    self.loginUI = loginGo:GetComponent("UIRootHandler").luaScript;
    self.loginUI:Init(self)
  end


end


loginUIController = LoginUIController:new()
return loginUIController

