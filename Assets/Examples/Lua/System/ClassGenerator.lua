--[[
  创建类方法
  name (string) 类名 
  super (table) 父类
]]
local _allClasses = {}
print("do class generator")
function class(name,super)
  
  if not name or type(name) ~= "string" then
    error("class name must be string")
  end
  
  if _allClasses[name] then
    print(name.."class exist")
    return _allClasses[name]
  end
  
  if not super then
    super = {}
  elseif type(super) ~= "table" then
    error("super must be table or nil")
  end
  
  local myclass = {}
  myclass.super = super
  myclass.className = name
  myclass.__index = myclass
  
  setmetatable(myclass, {__index = function(tab, key)        for k,v in pairs(myclass.super) do            if k == key   then                       return v            end        end    end})
  
  
  function myclass:new(obj)
    print("new class"..self.className)
    obj = obj or {}
    setmetatable(obj,myclass)
    return obj
  end
  
  function myclass:base()
    return myclass.super
  end
  
  
  --[[function myclass:GetClassName()
    return myclass.className
  end]]
  
  _allClasses[name] = myclass
  return myclass
end

--[[
Animal = class("Animal")
Animal.shoutNum = 0

function Animal:Shout()
  self.shoutNum = self.shoutNum + 1
  print(self,"shout"..self.shoutNum)
end

animal1 = Animal:new()
animal1:Shout()

Cat = class("Cat",Animal)

function Cat:Shout()
  self.shoutNum = self.shoutNum + 1
  self:base():Shout()
  print(self,"miao"..self.shoutNum)
end

cat = Cat:new({shoutNum = 1})
cat:Shout()]]

