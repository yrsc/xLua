local userName
local pwd
local loginBtn

loginUI = {1,2,3}
print(loginUI)
--require("Test")
require("LoginUIController")
for n in pairs(_G) do print(n) end
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

