CharacterController = class("CharacterController")

function CharacterController:CreateCharacter()
  self.model = UnityLoader:Instantiate(constant.RES_ROOT_PATH.."Prefab/Character/Cha_Bow_003.prefab")    
  self.speed = 3
  self.Animator = self.model:GetComponent("Animator")
end

function CharacterController:Move(dir)
  self.model.transform.position = self.model.transform.position + Vector3(dir.x,0,dir.y) * Time.deltaTime * self.speed
  self.model.transform.forward = Vector3(dir.x,0,dir.y)
  self.Animator:Play("walk")
end

function CharacterController:Stop()
  self.Animator:CrossFade("stand",0.5)
end

