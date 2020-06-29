using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp1.Control
{
    class Cost
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public double Summa { get; set; }
        public byte Count { get; set; }
        public byte Order { get; set; }
        public TypeCost Type { get; set;}

        public Cost(int _code) 
        {
            Code = _code;
        }

        public Cost(int _code, double _summa, TypeCost _typecost)
        {
            Code = _code;
            Summa = _summa;
            Type = _typecost;
        }

        public Cost(int _code, double _summa, TypeCost _typecost = null, byte _count=1, byte _order=0 )
        {
            Code = _code;
            Summa = _summa;
            Count = _count;
            Order = _order;
            Type = _typecost;
        }
    }
}
