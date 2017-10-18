using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Tks.Entities;
using Tks.Services;

namespace Tks.Model
{
    public sealed class Utility
    {
        /// <summary>
        /// Creates an xml element.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XmlElement CreateElement(XmlDocument document, string name, string value)
        {
            try
            {
                // create an element.
                XmlElement element = document.CreateElement(name);
                if (!string.IsNullOrEmpty(value))
                    element.InnerText = value;

                // Return the element.
                return element;
            }
            catch { throw; }
        }

        /// <summary>
        /// Create an xml attribute.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static XmlAttribute CreateAttribute(XmlDocument document, string name, string value)
        {
            try
            {
                XmlAttribute attribute = document.CreateAttribute(name);
                if (!string.IsNullOrEmpty(value))
                    attribute.InnerText = value;

                return attribute;
            }
            catch { throw; }
        }

        public static string ConvertASCII2String(string ASCII)
        {
            try
            {
                string decrypt = "";
                int pos = 0;

                while (pos < ASCII.Length)
                {
                    int Asci = Convert.ToInt32(ASCII.Substring(pos, 4));
                    decrypt += Convert.ToChar(Asci - 1000);
                    pos += 4;
                }
                return decrypt;
            }
            catch { throw; }
        }

        public static string ConvertASCII2Stringchn(string ASCII)
        {
            try
            {
                string decrypt = "";
                int pos = 0;

                while (pos < ASCII.Length)
                {
                    int Asci = Convert.ToInt32(ASCII.Substring(pos, 4));
                    decrypt += Convert.ToChar(Asci - 1000);
                    pos += 4;
                }
                return decrypt;
            }
            catch { throw; }
        }


        public static string GetIpAddress()
        {

            string ipAddress = String.Empty;

            var context = System.Web.HttpContext.Current;            

            if (isMobileBrowser())
            {
                System.Web.HttpBrowserCapabilities bc = context.Request.Browser;
                ipAddress = context.Request.UserHostAddress;
            }
            else
            {

                if (!String.IsNullOrWhiteSpace(context.Request.ServerVariables["REMOTE_ADDR"]))
                {
                    ipAddress = context.Request.ServerVariables["REMOTE_ADDR"];
                }
                else if (context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();

                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        string[] addresses = ipAddress.Split(',');
                        if (addresses.Length != 0)
                        {
                            ipAddress = addresses[0];
                        }
                    }
                }
            }


            //string ipAddress;
            //ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //if (ipAddress == "" || ipAddress == null)
            //   ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            if (ipAddress == "::1")
                ipAddress = "127.0.0.1";


            return ipAddress;
        }


        public static bool isMobileBrowser()
        {
            //GETS THE CURRENT USER CONTEXT
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            //FIRST TRY BUILT IN ASP.NT CHECK
            if (context.Request.Browser.IsMobileDevice)
            {
                return true;
            }
            //THEN TRY CHECKING FOR THE HTTP_X_WAP_PROFILE HEADER
            if (context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null)
            {
                return true;
            }
            //THEN TRY CHECKING THAT HTTP_ACCEPT EXISTS AND CONTAINS WAP
            if (context.Request.ServerVariables["HTTP_ACCEPT"] != null &&
                context.Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap"))
            {
                return true;
            }
            //AND FINALLY CHECK THE HTTP_USER_AGENT 
            //HEADER VARIABLE FOR ANY ONE OF THE FOLLOWING
            if (context.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                //Create a list of all mobile types
                string[] mobiles =
                    new[]
                {
                    "midp", "j2me", "avant", "docomo", 
                    "novarra", "palmos", "palmsource", 
                    "240x320", "opwv", "chtml",
                    "pda", "windows ce", "mmp/", 
                    "blackberry", "mib/", "symbian", 
                    "wireless", "nokia", "hand", "mobi",
                    "phone", "cdm", "up.b", "audio", 
                    "SIE-", "SEC-", "samsung", "HTC", 
                    "mot-", "mitsu", "sagem", "sony"
                    , "alcatel", "lg", "eric", "vx", 
                    "NEC", "philips", "mmm", "xx", 
                    "panasonic", "sharp", "wap", "sch",
                    "rover", "pocket", "benq", "java", 
                    "pt", "pg", "vox", "amoi", 
                    "bird", "compal", "kg", "voda",
                    "sany", "kdd", "dbt", "sendo", 
                    "sgh", "gradi", "jb", "dddi", 
                    "moto", "iphone"
                };

                //Loop through each item in the list created above 
                //and check if the header contains that text
                foreach (string s in mobiles)
                {
                    if (context.Request.ServerVariables["HTTP_USER_AGENT"].
                                                        ToLower().Contains(s.ToLower()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        List<Control> lstControl = new List<Control>();
        public void LoadLabels(List<LblLanguage> lblLanguagelst)
        {            

            if (lblLanguagelst != null)
            {
                List<Label> lblList = getLabels();
                List<Button> btnList = getButtons();
                foreach (LblLanguage lbl in lblLanguagelst)
                {

                    if (lblList != null)
                    {
                        //var frmlbl = lblList.FirstOrDefault(lb => lb.ID == lbl.LabelId);

                        //if (frmlbl != null)
                        //{
                        //    frmlbl.Text = lbl.DisplayText;
                        //}

                        var frmlbls = lblList.Where(lb => lb.ID == lbl.LabelId);
                        if (frmlbls != null)
                        {
                            foreach (var flbl in frmlbls)
                            {
                                flbl.Text = lbl.DisplayText;
                            }
                        }
                       


                    }

                    if (btnList != null)
                    {
                        //var frmbtn = btnList.FirstOrDefault(bt => bt.ID == lbl.LabelId);

                        //if (frmbtn != null)
                        //{
                        //    frmbtn.Text = lbl.DisplayText;
                        //}


                        var frmbtns = btnList.Where(bt => bt.ID == lbl.LabelId);
                        if (frmbtns != null)
                        {
                            foreach (var fbtn in frmbtns)
                            {
                                fbtn.Text = lbl.DisplayText;
                            }
                        }
                    }


                }
            }
            else
            {


            }
        }

        public void LoadGridLabels(List<LblLanguage> lblLanguagelst ,GridView grid)
        {

            if (lblLanguagelst != null)
            {
                List<Label> lblList = getGridLabels(grid);               
                foreach (LblLanguage lbl in lblLanguagelst)
                {

                    if (lblList != null)
                    {
                        //var frmlbl = lblList.FirstOrDefault(lb => lb.ID == lbl.LabelId);

                        //if (frmlbl != null)
                        //{
                        //    frmlbl.Text = lbl.DisplayText;
                        //}

                        var frmlbls = lblList.Where(lb => lb.ID == lbl.LabelId);
                        if (frmlbls != null)
                        {
                            foreach (var flbl in frmlbls)
                            {
                                flbl.Text = lbl.DisplayText;
                            }
                        }
                    }
                }
            }
            else
            {


            }
        }

        private List<Label> getLabels() // Add all Lables to a list
        {
            List<Label> lLabels = new List<Label>();

            Page _objpage = HttpContext.Current.Handler as Page;
            foreach (Control oControl in _objpage.Controls)
            {
                GetAllControlsInWebPage(oControl);
            }

            foreach (Control oControl in lstControl)
            {
                if (oControl.GetType() == typeof(Label))
                {
                    lLabels.Add((Label)oControl);
                }
            }
            return lLabels;
        }

        private List<Button> getButtons() // Add all Lables to a list
        {
            List<Button> lLabels = new List<Button>();

            Page _objpage = HttpContext.Current.Handler as Page;
            foreach (Control oControl in _objpage.Controls)
            {
                GetAllControlsInWebPage(oControl);
            }

            foreach (Control oControl in lstControl)
            {
                if (oControl.GetType() == typeof(Button))
                {
                    lLabels.Add((Button)oControl);
                }
            }
            return lLabels;
        }

        private List<Label> getGridLabels(GridView gv) // Add all Lables to a list
        {
            List<Label> lLabels = new List<Label>();

            foreach (Control oControl in gv.Controls)
            {
                GetAllControlsInWebPage(oControl);
            }

            foreach (Control oControl in lstControl)
            {
                if (oControl.GetType() == typeof(Label))
                {
                    lLabels.Add((Label)oControl);
                }
            }
            return lLabels;
        }

        protected void GetAllControlsInWebPage(Control oControl)
        {
            foreach (Control childControl in oControl.Controls)
            {
                lstControl.Add(childControl); //lstControl - Global variable
                if (childControl.HasControls())
                    GetAllControlsInWebPage(childControl);
            }
        }

    }


}
