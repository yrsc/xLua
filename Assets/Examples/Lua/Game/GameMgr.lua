require("Boot/BaseDependcy")
require("UI/BattleUI/BattleUIController")
require("Game/InputMgr")

GameMgr = class("GameMgr")

function GameMgr:Awake()
  --TODO:use charactermgr create the character
  local character = UnityLoader:Instantiate("Assets/Examples/ABResrouces/Prefab/Character/Cha_Bow_003.prefab")    
  BattleUIController:OpenBattleUI()
end

function GameMgr:Update()
  InputMgr:Update()
end

gameMgr = GameMgr:new()
return gameMgr