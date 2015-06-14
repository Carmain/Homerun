using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MobeeApp
{
    public class RecordManager
    {
        private IsolatedStorageSettings storage = IsolatedStorageSettings.ApplicationSettings;
        
        public RecordManager() { }

        public bool isExist(string key)
        {
            object objValue;

            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue<object>(key, out objValue) == false)
                return false;

            return true;
        }

        public bool CreateOrUpdate(string key, string value)
        {
            bool success = true;
            try
            {
               if (isExist(key))
                    storage[key] = value;
                else 
                    storage.Add(key, value);
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine(ex.Message);
                success = false;
            }

            return success;
        }

        public string Read(string key)
        {

            try
            {
                if (isExist(key))
                    return (string)storage[key];
            }
            catch (System.Collections.Generic.KeyNotFoundException ex)
            {
                MessageBox.Show("A problem occurred with your data. Please try later or contact the Mobee team.");
            }
            return null;
        }

        public void Delete(string key)
        {
            storage.Remove(key);
        }
    }
}
