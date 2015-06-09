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

        private bool isExist(string key)
        {
            object objValue;

            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue<object>(key, out objValue) == false)
            {
                return false;
            }

            return true;
        }

        public RecordManager() { }

        public void createOrUpdate(string key, string value)
        {
            try
            {
               if (isExist(key))
                {
                    storage[key] = value;
                    MessageBox.Show("Changed to: " + (string)storage[key]);
                }
                else 
                {
                    storage.Add(key, value);
                    //Debug.WriteLine("Settings stored. " + (string)storage[key]);
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("A problem occurred when saving your data. Please contact the Mobee team.");
                Debug.WriteLine(ex.Message);
            }
        }

        string read(string key)
        {

            try
            {
                MessageBox.Show("Setting retrieved: " + (string)storage[key]);
                if (isExist(key))
                    return (string)storage[key];
            }
            catch (System.Collections.Generic.KeyNotFoundException ex)
            {
                MessageBox.Show("A problem occurred with your data. Please try later or contact the Mobee team.");
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        public void delete(string key)
        {
            storage.Remove(key);
            MessageBox.Show("Data deleted. Click Retrieve to confirm deletion.");
        }
    }
}
