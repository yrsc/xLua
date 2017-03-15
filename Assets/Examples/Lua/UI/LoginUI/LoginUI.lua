LoginUI = class("LoginUI")

function LoginUI:Awake()
  print("lua awake")
  print(self)
end

function LoginUI:Init(controller)
  self.controller = controller  
end


function LoginUI:OnBtnClick(btn)	
  return function()
    if(btn == self.loginBtn) then
      if self.userNameInput.text == "123" and self.pwdInput.text == "123" then    
        print("login succeed")
      else
        print("username and pwd should be 123 while your username is "..self.userNameInput.text.." and pwd is"..self.pwdInput.text)        
        --local loader = CS.xLuaSimpleFramework.SimpleLoader
        --local cube = loader.InstantiateGameObject("Assets/Examples/ABResrouces/Models/Cube.prefab")                
        --cube.transform.position = CS.UnityEngine.Vector3(10,10,10)        
    --[[elseif(btn == rightBtn) then
        descLabel.text = "rightBtn"
        local levelMgr = CS.xLuaSimpleFramework.LevelLoadMgr
        levelMgr.LoadLevelSync("Battle")
    elseif(btn == midBtn) then
        descLabel.text = "midBtn"]]
      end
      
    end
	end
end

function LoginUI:Start()
  print("lua start")
  self:GetUIComponents()
  
    --[[flag,rightBtn = uiComponents:TryGetValue("RightBtn", nil)
    assert(flag,"right Btn is null")
    rightBtn:GetComponent("Button").onClick:AddListener(OnBtnClick(rightBtn))
    flag,midBtn = uiComponents:TryGetValue("MidBtn", nil)
    assert(flag,"Mid Btn is null")
    midBtn:GetComponent("Button").onClick:AddListener(OnBtnClick(midBtn))    
    flag,descLabel = uiComponents:TryGetValue("DesLabel", nil)
    assert(flag,"descLabelis null")    
    descLabel = descLabel:GetComponent("Text")]]
end

function LoginUI:GetUIComponents()
  local uiComponents = self.monobehaviour.uiComponents
  local flag = false;
  --get login btn
  flag,self.loginBtn = uiComponents:TryGetValue("LoginBtn", nil)
  assert(flag,"loginBtn Btn is null")
  self.loginBtn:GetComponent("Button").onClick:AddListener(self:OnBtnClick(self.loginBtn))
  --get username input
  flag,self.userNameInput = uiComponents:TryGetValue("UerNameInput", nil)  
  assert(flag,"UserNameInput is null")
  self.userNameInput = self.userNameInput:GetComponent("InputField")
  --get pwd input
  flag,self.pwdInput = uiComponents:TryGetValue("PwdInput", nil)  
  assert(flag,"PwdInput is null")
  self.pwdInput = self.pwdInput:GetComponent("InputField")
end


function LoginUI:Update()
end

loginUI = LoginUI:new()

return loginUI
--require("Test")
--require("LoginUIController")
--for n in pairs(_G) do print(n) end
--print(loginUIController)
--[[
function Start()
    local uiComponents = self.uiComponents
    local flag = false;
    -------- get userName
    flag,userName = uiComponents:TryGetValue("UerNameInput", nil)
    assert(flag,"UerNameInput is null")
    userName = userName:GetComponent("InputField")
    -------- get pwd
    flag,pwd = uiComponents:TryGetValue("PwdInput", nil)
    assert(flag,"PwdInput is null")
    pwd = pwd:GetComponent("InputField")
    -------- get login btn
    flag,loginBtn = uiComponents:TryGetValue("LoginBtn", nil)
    --loginBtn:GetComponent("Button").onClick:AddListener(loginUIController.OnBtnClick(loginBtn))
end

]]

