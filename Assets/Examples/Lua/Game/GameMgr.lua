require("Boot/BaseDependcy")
require("UI/BattleUI/BattleUIController")
require("Game/InputMgr")
require("Game/CharacterController")

GameMgr = class("GameMgr")

function GameMgr:Awake()
  CharacterController:CreateCharacter()
  BattleUIController:OpenBattleUI()
end

function GameMgr:Update()
  InputMgr:Update()
end

gameMgr = GameMgr:new()
return gameMgr