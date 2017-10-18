using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Tks.Model
{
    public sealed partial class GeneralSetting
    {
        System.Collections.Generic.Dictionary<string, object> _customData;

        public GeneralSetting() {
            this._customData = new System.Collections.Generic.Dictionary<string, object>();
        }

        public int Id { get; set; }
        public string Category { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int CreateUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public int LastUpateUserId { get; set; }
        public DateTime LastUpdateDate { get; set; }

        [XmlIgnore()]
        public System.Collections.Generic.Dictionary<string, object> CustomData
        {
            get { return this._customData; }
        }
    }
}
