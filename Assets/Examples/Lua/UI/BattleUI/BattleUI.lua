BattleUI = class("BattleUI")

function BattleUI:Init(controller)
  self.controller = controller
end

battleUI = BattleUI:new()
return battleUI