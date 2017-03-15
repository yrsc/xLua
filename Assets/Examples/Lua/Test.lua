require("System/ClassGenerator")

Test = class("Test")

function Test:Awake()
end

function Test:Start() 
end

function Test:Update()
end

test = Test:new()

return test