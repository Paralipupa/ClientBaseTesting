using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBaseTesting.Model
{
    class TypeCost
    {
        public TypeCost(int code)
        {
            Code = code;
        }

        public TypeCost(int code, string name)
        {
            Code = code;
            Name = name;
        }
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
