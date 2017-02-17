function Awake()
    print("Lua Awake")
end

local leftBtn
local rightBtn
local midBtn
function OnBtnClick(btn)	
	return function()
		--print(btn.."left btn "..leftBtn)
		if(btn == leftBtn) then print("left btn be clicked!") end
	end
end


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
end


function Update()
    
end