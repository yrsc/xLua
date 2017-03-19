require ("UI/BattleUI/JoyStick/JoyStickController")

BattleUIController = class("BattleUIController")

function BattleUIController:OpenBattleUI()
    local battleuiGo = UnityLoader:Instantiate("Assets/Examples/ABResrouces/UI/BattleUI/BattleUI.prefab")
    self.battleUI = battleuiGo:GetComponent("UIRootHandler").luaScript;
    self.battleUI:Init(self)
    joyStick = JoyStickController:CreateJoyStick()
    joyStick.transform:SetParent(battleuiGo.transform)
end
