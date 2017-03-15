GameMgr = class("GameMgr")

function GameMgr:Awake()
  local character = UnityLoader:Instantiate("Assets/Examples/ABResrouces/Prefab/Character/Cha_Bow_003.prefab")  
end

gameMgr = GameMgr:new()
return gameMgr