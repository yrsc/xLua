InputMgr = class("InputMgr")

local joyStickRegion = Vector3(Screen.width/2,Screen.height * 0.75,0)
function InputMgr:Update()
  if(Input.GetMouseButtonDown(0)) then
    if(Input.mousePosition.x < joyStickRegion.x and Input.mousePosition.y < joyStickRegion.y) then
      JoyStickController:SetClickPos(Input.mousePosition)
    end
    
  end
  
end
