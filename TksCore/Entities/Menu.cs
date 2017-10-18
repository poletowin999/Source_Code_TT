using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Tks.Model;


namespace Tks.Entities
{

    public class Menu
       : EntityBase
    {

        public Menu()
            : base()
        {

        }

        public Menu(int id)
            : base(id)
        {
        }

        #region Public Members

        public string Text { get; set; }

        public string Name { get; set; }

        public int Type { get; set; }

        public bool IsPublic { get; set; }

        public int ParentId { get; set; }

        public string ModuleName { get; set; }

        public string FormName { get; set; }

        public bool IsValid { get; set; }

        public int Priority { get; set; }

        public string ImageName { get; set; }

        public string Tooltip { get; set; }

        public string NavigateUrl { get; set; }

        public int CreateUser { get; set; }

        public DateTime? CreateDate { get; set; }

        public int LastUpdateUser { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        #endregion

    }

}
