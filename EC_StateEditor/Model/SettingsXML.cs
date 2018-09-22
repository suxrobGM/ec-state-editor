using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EC_StateEditor.Model
{
    class SettingsXML
    {
        public string FileName { get; set; }

        public string ModPath
        {
            get
            {
                return XDocument.Load(FileName).Element("root").Element("mod_path").Value;
            }
            set
            {
                var xDoc = XDocument.Load(FileName);
                xDoc.Element("root").Element("mod_path").Value = value;
                xDoc.Save(FileName);
            }
        }

        public SettingsXML(string fileName)
        {
            FileName = fileName;

            if (!IsSettingsXmlFile() || !File.Exists(fileName))
                CreateDefaultXml();
        }  
        
        private void CreateDefaultXml()
        {
            XDocument xDoc = new XDocument(
                   new XElement("root",
                       new XComment("Load settings xml"),
                       new XElement("mod_path")
                   )
               );
            xDoc.Save(FileName);
        }

        private bool IsSettingsXmlFile()
        {
            var xDoc = XDocument.Load(FileName);
            
            try
            {
                if (xDoc.Element("root").Element("mod_path") != null)
                    return true;
            }
            catch(Exception)
            {
                return false;
            }
            return false;
        }
    }
}
