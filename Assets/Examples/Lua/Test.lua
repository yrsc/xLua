require("System/ClassGenerator")

Test = class("Test")

function Test:Awake()
  print("Awake Test")
  print(self)
end

function Test:Start() 
end

function Test:Update()
end

test = Test:new()

return test