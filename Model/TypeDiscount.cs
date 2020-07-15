using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBaseTesting.Model
{
    class TypeDiscount
    {
        private int _code;
        private readonly string _name=null;

        public int Code => _code;
        public string Name => _name;

        public TypeDiscount(int code) 
        {
            _code = code;
        }
    }
}
