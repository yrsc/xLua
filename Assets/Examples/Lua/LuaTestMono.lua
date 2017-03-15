<<<<<<< HEAD
local leftBtn
local rightBtn
local midBtn
local descLabel

=======
>>>>>>> 3b46bd3e662f71264cc276b911b29826ee566b57
function Awake()
    print("Lua Awake")
end

<<<<<<< HEAD
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
=======
local leftBtn
local rightBtn
local midBtn
function OnBtnClick(btn)	
	return function()
		--print(btn.."left btn "..leftBtn)
		if(btn == leftBtn) then print("left btn be clicked!") end
>>>>>>> 3b46bd3e662f71264cc276b911b29826ee566b57
	end
end


<<<<<<< HEAD
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

=======
function Start()
    print("Lua Start")
    local uiComponents = self.uiComponents
	local flag = false;
    flag,leftBtn = uiComponents:TryGetValue("LeftBtn", nil)
    assert(flag,"left Btn is null")
    leftBtn:GetComponent("Button").onClick:AddListener(OnBtnClick(leftBtn))
    --hi()
    --[[
    local flag,rightBtn = uiComponents:TryGetValue("RightBtn", nil)
    assert(flag,"right Btn is null")
    rightBtn:GetComponent("Button").onClick:AddListener(onBtnClick(rightBtn.name))
    local flag,midBtn = uiComponents:TryGetValue("MidBtn", nil)
    assert(flag,"Mid Btn is null")
    midBtn:GetComponent("Button").onClick:AddListener(onBtnClick(midBtn.name))
    ]]
>>>>>>> 3b46bd3e662f71264cc276b911b29826ee566b57
end


function Update()
    
end