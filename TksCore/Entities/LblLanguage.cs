using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;

namespace Tks.Entities
{
    public class LblLanguage : EntityBase
    {
        #region Public Members
        public LblLanguage()
            : base()
        {
        }

        public LblLanguage(int id)
            : base(id)
        {

        }
        public string LabelId { get; set; }
        public string DisplayText { get; set; }
        public string SupportingText1 { get; set; }
        

        #endregion
    }
}
