using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockerManager
{
    public static class CustomMapper
    {
        public static List<SensorModel> ToSensorModel(this string messsages)
        {
            var rs = new List<SensorModel>();
            var arrMessages = messsages.Split(",");
            
            foreach (var messsage in arrMessages)
            {
                var model = new SensorModel();
                if (!string.IsNullOrEmpty(messsage))
                {
                    var keyvalues = messsage.Trim('{', '}').Split(";");
                    var list = new List<SensorModel>();
                    foreach (var keyvalue in keyvalues)
                    {
                        var key = keyvalue.Split(':')[0];
                        var value = keyvalue.Split(':')[1];
                        if(key=="Name")
                        {
                            model.Name = value;
                        }
                        if(key =="Value")
                        {
                            model.Value = value;
                        }
                        if (key == "Port")
                        {
                            model.Port = value;
                        }
                        if (key == "Type")
                        {
                            model.Type = value;
                        }
                        if (key == "Unit")
                        {
                            model.Unit = value;
                        }
                    }
                    rs.Add(model);
                }
            }
            return rs;
        }
    }
}
public class SensorModel
{
    public string Name { get; set; }
    public string Value { get; set; }
    public string Type { get; set; }
    public string Port { get; set; }
    public string Unit { get; set; }
}
