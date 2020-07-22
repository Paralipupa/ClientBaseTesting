using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Specialized;

namespace ClientBaseTesting.Model
{

    internal class DictionChanges
    {

        public Dictionary<string, Dictionary<int, Dictionary<int, RegisterChange>>> DictList { get; set; }

        public DictionChanges()
        {
            DictList = new Dictionary<string, Dictionary<int, Dictionary<int, RegisterChange>>>();
        }

    }

    internal class RegisterChange : Table
    {
        private object _sender;
        private PropertyChangedEventArgs _ep;
        NotifyCollectionChangedEventArgs _ec;
        private string _dataname;
        private string _datavalue;
        private string _datavaluetype;

        public RegisterChange() { }
        public RegisterChange(object sender, PropertyChangedEventArgs ep, NotifyCollectionChangedEventArgs ec)
        {
            _sender = sender;
            _ep = ep;
            _ec = ec;
        }

        public object Sender => _sender;
        public NotifyCollectionChangedEventArgs ENotifyCollection => _ec;
        public PropertyChangedEventArgs EProperty => _ep;

        public string DataName
        {
            get { return _dataname; }
            set { _dataname = value; }
        }
        public string DataValue
        {
            get { return _datavalue; }
            set { _datavalue = value; }
        }
        public string DataValueType
        {
            get { return _datavaluetype; }
            set { _datavaluetype = value; }
        }

        public static bool Save(List<RegisterChange> coll)
        {
            bool issave = false;
            DictionChanges regchange = new DictionChanges();

            int count = coll.Count;
            for (int i = 0; i < count; i++)
            {
                RegisterChange o = coll[i];
                Table table = o.Sender as Table;
                if (table != null)
                {
                    PropertyInfo infotablename = table.GetType().GetProperty("TableName");
                    string tablename = (string)infotablename.GetValue(table);

                    if (tablename != null)
                    {
                        PropertyInfo infocode = table.GetType().GetProperty("Code");
                        int code = (int)infocode.GetValue(table);
                        if (o.EProperty != null)
                        {
                            PropertyInfo info = table.GetType().GetProperty(o.EProperty.PropertyName);
                            string res = info.GetValue(table).ToString();
                            PropertyInfo infofields = table.GetType().GetProperty("Fields");
                            Dictionary<string, string> fields = infofields.GetValue(table) as Dictionary<string, string>;
                            o.DataName = fields[o.EProperty.PropertyName];
                            o.DataValue = res;
                            o.DataValueType = info.PropertyType.FullName.ToString();
                            int hash = o.EProperty.PropertyName.GetHashCode();
                            if (regchange.DictList.Count != 0 && (regchange.DictList.ContainsKey(tablename)))
                            {
                                if (regchange.DictList[tablename].ContainsKey(code))
                                {
                                    if (regchange.DictList[tablename][code].ContainsKey(hash))
                                    {
                                        regchange.DictList[tablename][code].Remove(hash);
                                    }
                                    regchange.DictList[tablename][code].Add(hash, o);
                                }
                                else
                                {
                                    regchange.DictList[tablename].Add(code, new Dictionary<int, RegisterChange> { { hash, o } });
                                }
                            }
                            else
                            {
                                regchange.DictList.Add(tablename, new Dictionary<int, Dictionary<int, RegisterChange>> { { code, new Dictionary<int, RegisterChange> { { hash, o } } } });
                            }

                            List<string> ls = new List<string>();

                            foreach (KeyValuePair<string, Dictionary<int, Dictionary<int, RegisterChange>>> keyValue1 in regchange.DictList)
                            {
                                foreach (KeyValuePair<int, Dictionary<int, RegisterChange>> keyValue2 in keyValue1.Value)
                                {
                                    string strhead = $"UPDATE `{keyValue1.Key}` SET ";
                                    string strvalue = "";
                                    foreach (KeyValuePair<int, RegisterChange> keyValue3 in keyValue2.Value)
                                    {
                                        switch (keyValue3.Value.DataValueType.ToString())
                                        {
                                            case "System.Double":
                                            case "System.Int32":
                                                strvalue += $" `{keyValue3.Value.DataName}`={keyValue3.Value.DataValue},";
                                                break;
                                            case "System.String":
                                                strvalue += $" `{keyValue3.Value.DataName}`='{keyValue3.Value.DataValue}',";
                                                break;
                                            case "System.DataTime":
                                                strvalue += $" `{keyValue3.Value.DataName}`={keyValue3.Value.DataValue},";
                                                break;
                                            default:
                                                strvalue += $" `{keyValue3.Value.DataName}`={keyValue3.Value.DataValue},";
                                                break;
                                        }

                                    }

                                    strhead += strvalue.TrimEnd(',') + $" WHERE code={keyValue2.Key}";
                                    ls.Add(strhead);
                                }
                            }

                        }
                        else if (o.ENotifyCollection != null) 
                        {
                            //Удаление записи
                            if (o.ENotifyCollection.Action == NotifyCollectionChangedAction.Remove)
                            {

                            }
                        }
                    }

                }

            }



            issave = true;

            return (issave);
        }
    }
}
