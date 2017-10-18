using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Tks.Model
{
    /// <summary>
    /// Represent a base class for all entities.
    /// </summary>
    [Serializable()]
    public abstract class EntityBase
    {
        #region Class variables

        int _id;
        System.Collections.Generic.Dictionary<string, object> _customData;

        #endregion

        #region Constructor

        public EntityBase()
        {
            this._id = 0;
            this._customData = new System.Collections.Generic.Dictionary<string, object>();
        }

        public EntityBase(int id)
        {
            this._id = id;
            this._customData = new System.Collections.Generic.Dictionary<string, object>();
        }

        #endregion

        #region Public members

        public int Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        [XmlIgnore()]
        public System.Collections.Generic.Dictionary<string, object> CustomData
        {
            get { return this._customData; }
        }

        public virtual string GetXml()
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
            }
            catch { throw; }
        }

        public static string GetXml<T>(List<T> entities)
            where T : EntityBase
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(string.Format("{0}s", typeof(T).Name)));
                StringBuilder xmlData = new StringBuilder();

                // To omit xml declaration.
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                // To omit namespace.
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);

                using (XmlWriter writer = XmlWriter.Create(xmlData, settings))
                {
                    serializer.Serialize(writer, entities, namespaces);
                    writer.Close();
                }

                return xmlData.ToString();
            }
            catch { throw; }
        }

        #endregion
    }
}
