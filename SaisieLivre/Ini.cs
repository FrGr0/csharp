using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Ini
{
    /// <summary>
    /// Create a New INI file to store or load data
    /// </summary>
    public class IniFile
    {
        public string path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);
        
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileSectionNames (byte[] lpszReturnBuffer, 
                int nSize, string filePath);

        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <PARAM name="INIPath"></PARAM>
        public IniFile(string INIPath)
        {
            path = INIPath;
        }
        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// Section name
        /// <PARAM name="Key"></PARAM>
        /// Key Name
        /// <PARAM name="Value"></PARAM>
        /// Value Name
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.path);
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// <PARAM name="Key"></PARAM>
        /// <PARAM name="Path"></PARAM>
        /// <returns></returns>
        public string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(5000);
            int i = GetPrivateProfileString(Section, Key, "", temp,
                                            5000, this.path);
            return temp.ToString();

        }


        

        public long IniDeleteSelection(string Selection)
        {
            //Delete a selection form ini
            return WritePrivateProfileString(Selection, null, null, this.path);
        }



        public string[] GetIniSections()
        {

            byte[] buffer = new byte[1024];
            int i = GetPrivateProfileSectionNames(buffer, buffer.Length, this.path);
            string allSections = System.Text.Encoding.Default.GetString(buffer);
            string[] sectionNames = allSections.Split('\0');
            return sectionNames;
        }

    }
}