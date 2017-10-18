using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace Tks.Model
{
    [Serializable()]
    public class SearchCriteria
    {
        public const string DefaultViewName = "Default View";

        public SearchCriteria()
        {
            this.ViewName = SearchCriteria.DefaultViewName;
        }

        public string ViewName { get; set; }

        public virtual string GetXml()
        {
            try
            {
                return this.GetXml(this.ViewName);
            }
            catch { throw; }
        }

        public virtual string GetXml(string viewName)
        {
            try
            {
                switch (viewName)
                {
                    case SearchCriteria.DefaultViewName:
                        return this.GetDefaultViewXml();
                    default:
                        return string.Empty;
                }
            }
            catch { throw; }
        }

        protected string GetDefaultViewXml()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(this.GetType());
                StringBuilder xmlData = new StringBuilder();

                // To omit xml declaration.
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                // To omit namespace.
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);

                using (XmlWriter writer = XmlWriter.Create(xmlData, settings))
                {
                    serializer.Serialize(writer, this, namespaces);
                    writer.Close();
                }
                return xmlData.ToString();
                //return "N" + xmlData.ToString();
            }
            catch { throw; }
        }
    }
}
