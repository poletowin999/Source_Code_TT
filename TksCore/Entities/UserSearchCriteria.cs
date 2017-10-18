using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Tks.Model;


namespace Tks.Entities
{
    [XmlRoot("SearchCriteria")]
    public class UserSearchCriteria
        : SearchCriteria
    {
        public UserSearchCriteria()
            : base()
        {
            this.Status = "ALL";
        }

        public string Name { get; set; }

        public string RoleName { get; set; }

        public string EMailId { get; set; }

        public string Status { get; set; }

        public int? UserId { get; set; }

        public string CityName { get; set; }

        public override string GetXml(string viewName)
        {
            try
            {
                switch (viewName)
                {
                    case SearchCriteria.DefaultViewName:
                        return this.GetDefaultViewXml();
                    case "HieraricalSearch":
                        return this.CustomSearchCriteria();
                    case "SearchManager":
                        return this.CustomSearchCriteria();
                    case "SearchManagerforhierarchy":
                        return this.CustomSearchCriteria();
                    case "ActivityResetUserSearch":
                        return this.ActivityResetUserSearchCriteria();
                    case "SearchUsersforuserSetting":
                        return this.CustomSearchCriteria();
                    default:
                        return string.Empty;
                }
            }
            catch { throw; }
        }
        private string CustomSearchCriteria()
        {
            StringBuilder xml = new StringBuilder("<SearchCriteria>");
            xml.Append(string.Format("<Name>{0}</Name><RoleName>{1}</RoleName>", this.Name, this.RoleName));
            xml.Append(string.Format("<EmailId>{0}</EmailId><CityName>{1}</CityName><Status>{2}</Status>",this.EMailId,this.CityName,this.Status));
            xml.Append(string.Format("<LoginUserId>{0}</LoginUserId>",this.UserId));
            xml.Append("</SearchCriteria>");
            return xml.ToString();

            
        }
        private string ActivityResetUserSearchCriteria()
        {
            StringBuilder xml = new StringBuilder("<SearchCriteria>");
            xml.Append(string.Format("<Name>{0}</Name>", this.Name));
            xml.Append("</SearchCriteria>");
            return xml.ToString();
        }

    }
}
