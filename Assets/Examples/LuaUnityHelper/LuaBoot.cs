using UnityEngine;
using System.Collections;
using XLua;

namespace xLuaSimpleFramework
{
    public class LuaBoot
    {
        private static LuaEnv _luaEnv = null;
        public static LuaEnv luaEnv
        {
            get
            {
                if (_luaEnv == null)
                {
                    _luaEnv = new LuaEnv();
                    InitLuaEvn();
                }
                return _luaEnv;
            }
        }

        static void InitLuaEvn()
        {
            _luaEnv.AddLoader((ref string filename) => 
            {
				string script = SimpleLoader.LoadLua(filename);
				return System.Text.Encoding.UTF8.GetBytes(script);
            });
        }
    }
}