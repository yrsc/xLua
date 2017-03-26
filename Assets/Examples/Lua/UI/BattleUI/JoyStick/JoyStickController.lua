JoyStickController = class("JoyStickController")

local radius = 150
function JoyStickController:CreateJoyStick()
  assert(self.joyStick == nil,"joy stick resource not release")
  self.joyStick = UnityLoader:Instantiate(constant.RES_ROOT_PATH.."UI/BattleUI/StickBG.prefab")
  self.joyCircle = self.joyStick.transform:Find("StickCircle")
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
  self.joyCircle.transform.position = pos
end

function JoyStickController:SetPressDownPos(pos)
  local dir = pos - self.joyStick.transform.position   
  --ingore dir is zero
  if(dir.x == 0 and dir.y == 0) then
    return
  end
  dir = Vector3.Normalize(dir)
  local circlePos = self.joyStick.transform.position + dir * radius
  self.joyCircle.transform.position = circlePos
  CharacterController:Move(dir)
end

function JoyStickController:Reset()
    self.joyStick.transform.position = self.initPos
    self.joyCircle.transform.position = self.initPos
    CharacterController:Stop()
end


