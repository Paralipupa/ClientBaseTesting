using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBaseTesting.Model
{
    class Discount
    {
        private int _code;
        private string _name = null;

        public int Code => _code;
        public string Name => _name;

        public Discount(int code)
        {
            _code = code;
        }
    }

}
