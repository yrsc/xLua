JoyStickController = class("JoyStickController")

local radius = 150
function JoyStickController:CreateJoyStick()
  assert(self.joyStick == nil,"joy stick resource not release")
  self.joyStick = UnityLoader:Instantiate("Assets/Examples/ABResrouces/UI/BattleUI/StickBG.prefab")
  self.initPos = self.joyStick.transform.position
  return self.joyStick
end

function JoyStickController:SetClickPos(pos)
  local dir = pos - self.initPos
  
  --ingore dir is zero
  if(dir.x == 0 and dir.y == 0) then
    return
  end
  
  local bgPos = pos - Vector3.Normalize(dir) * radius
  self.joyStick.transform.position = bgPos
end
