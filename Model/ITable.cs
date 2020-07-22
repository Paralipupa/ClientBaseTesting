using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBaseTesting.Model
{
    interface ITable
    {
        string TableName { get; set; }
        int Code { get; set; }
        string Name { get; set; }
        Dictionary<string, string> Fields {get;set;}
    }
}
