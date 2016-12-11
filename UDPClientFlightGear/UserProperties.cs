using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPClientFlightGear
{
    class UserProperties
    {
        private Dictionary<String, String> list;
        private String fileName;

        public UserProperties(String file)
        {
            reload(file);
        }

        public void reload()
        {
            reload(this.fileName);
        }

        public void reload(String fileName)
        {
            this.fileName = fileName;
            list = new Dictionary<String, String>();
            if (System.IO.File.Exists(fileName))            
                loadFromFile(fileName);
            else
                System.IO.File.Create(fileName);
        }

        private void loadFromFile(String file)
        {
            foreach (String line in System.IO.File.ReadAllLines(file))
            {
                if ((!String.IsNullOrEmpty(line)) &&
                    (!line.StartsWith(";")) &&
                    (!line.StartsWith("#")) &&
                    (!line.StartsWith("'")) &&
                    (line.Contains('=')))
                {
                    int index = line.IndexOf('=');
                    String key = line.Substring(0, index).Trim();
                    String value = line.Substring(index + 1).Trim();

                    if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                        (value.StartsWith("'") && value.EndsWith("'")))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }

                    try
                    {
                        //ignore duplicates
                        list.Add(key, value);
                    }
                    catch { }
                }
            }
        }

        public void Save()
        {
            Save(this.fileName);
        }

        public void Save(String fileName)
        {
            this.fileName = fileName;
            if (!System.IO.File.Exists(fileName))
                System.IO.File.Create(fileName);

            System.IO.StreamWriter file = new System.IO.StreamWriter(fileName);

            foreach (String prop in list.Keys.ToArray())
                if (!String.IsNullOrWhiteSpace(list[prop]))
                    file.WriteLine(prop + "=" + list[prop]);

            file.Close();
        }

        public String get(String field, String defValue)
        {
            return (get(field) == null)? (defValue) : (get(field));
        }

        public String get(String field)
        {
            return (list.ContainsKey(field)) ? (list[field]) : (null);
        }

        public float getFloat(String field)
        {
            return float.Parse(get(field));
        }
        
        public int getInt(String field, int defValue)
        {
            return (getInt(field) == null) ? (defValue) : getInt(field);
        }
        public int getInt(String field)
        {
            return Int32.Parse(get(field));
        }
        public void set(String field, Object value)
        {
            if (!list.ContainsKey(field))
                list.Add(field, value.ToString());
            else
                list[field] = value.ToString();
        }
    }
}
