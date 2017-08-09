using System.Collections;
using System.Collections.Generic;
using System;

namespace Model
{
    public enum RefType
    {
        boolType, numberType, stringType
    }

    public static class RefTypeHelper
    {
        public static bool Validate(string text, RefType type)
        {
            bool valid = false;
            switch (type)
            {
                case RefType.boolType:
                    if (text.ToLower().Equals("false") || text.ToLower().Equals("true"))
                        valid = true;
                    break;
                case RefType.numberType:
                    float n;
                    if (float.TryParse(text, out n))
                        valid = true;
                    break;
                case RefType.stringType:
                    valid = true;
                    break;
                default: throw new ArgumentException("Invalid Ref Type");
            }
            return valid;
        }

        public static string Name(RefType type)
        {
            switch (type)
            {
                case RefType.boolType:
                    return "BOOLEAN";
                case RefType.numberType:
                    return "NUMBER";
                case RefType.stringType:
                    return "STRING";
                default: throw new ArgumentException("Invalid Ref Type");
            }
        }
    }
}