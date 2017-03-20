InputMgr = class("InputMgr")

local joyStickRegion = Vector3(Screen.width/2,Screen.height * 0.75,0)

function InputMgr:Update()
  if(Input.GetMouseButtonDown(0)) then
    if(Input.mousePosition.x < joyStickRegion.x and Input.mousePosition.y < joyStickRegion.y) then
      JoyStickController:SetClickPos(Input.mousePosition)
    end    
  elseif(Input.GetMouseButton(0)) then
    JoyStickController:SetPressDownPos(Input.mousePosition)
  elseif(Input.GetMouseButtonUp(0)) then
    JoyStickController:Reset()
  end  
end
