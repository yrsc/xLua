local leftBtn
local rightBtn
local midBtn
local descLabel

function Awake()
    print("Lua Awake")
end

function OnBtnClick(btn)	
  return function()
    if(btn == leftBtn) then
        descLabel.text = "leftBtn"
        local loader = CS.xLuaSimpleFramework.SimpleLoader
        local cube = loader.InstantiateGameObject("Assets/Examples/ABResrouces/Models/Cube.prefab")        
        cube.transform.position = CS.UnityEngine.Vector3(10,10,10)        
    elseif(btn == rightBtn) then
        descLabel.text = "rightBtn"
        local levelMgr = CS.xLuaSimpleFramework.LevelLoadMgr
        levelMgr.LoadLevelSync("Battle")
    elseif(btn == midBtn) then
        descLabel.text = "midBtn"
    end
	end
end


function Start()
    print("Lua Start")
    local uiComponents = self.uiComponents
    local flag = false;
    flag,leftBtn = uiComponents:TryGetValue("LeftBtn", nil)
    assert(flag,"left Btn is null")
    leftBtn:GetComponent("Button").onClick:AddListener(OnBtnClick(leftBtn))
    flag,rightBtn = uiComponents:TryGetValue("RightBtn", nil)
    assert(flag,"right Btn is null")
    rightBtn:GetComponent("Button").onClick:AddListener(OnBtnClick(rightBtn))
    flag,midBtn = uiComponents:TryGetValue("MidBtn", nil)
    assert(flag,"Mid Btn is null")
    midBtn:GetComponent("Button").onClick:AddListener(OnBtnClick(midBtn))    
    flag,descLabel = uiComponents:TryGetValue("DesLabel", nil)
    assert(flag,"descLabelis null")    
    descLabel = descLabel:GetComponent("Text")

end


function Update()
    
end