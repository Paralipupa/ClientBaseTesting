using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBaseTesting.Model
{
    class TypeEdizm
    {
        public TypeEdizm(int code, string name )
        {
            this.Code = code;
            this.Name = name;
        }
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
