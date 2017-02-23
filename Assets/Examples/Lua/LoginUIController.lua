print("load loginUIController")
print(loginUI)
--local loginUI = require("LoginUI")
--[[
function OnBtnClick(btn)	
  return function()
    if(btn == loginUI.loginBtn) then
        print("Login Click")
    end
	end
end
]]