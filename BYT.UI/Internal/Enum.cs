using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BYT.UI.Internal
{
        public static class EnumExtensions
        {
            public static string GetDescription(this Enum element)
            {
                Type type = element.GetType();
                MemberInfo[] memberInfo = type.GetMember(element.ToString());

                if (memberInfo != null && memberInfo.Length > 0)
                {
                    object[] attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (attributes != null && attributes.Length > 0)
                    {
                        return ((DescriptionAttribute)attributes[0]).Description;
                    }
                }

                return element.ToString();
            }
        }
      
    }

