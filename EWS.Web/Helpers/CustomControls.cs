using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EWS.Web.Helpers
{
    public static class CustomControls
    {
        const string FORM_CONTROL = "form-control";
        const string TEXT_BOX = "text-box";
        const string SINGLE_LINE = "single-line";
        const string CONTROL_LABEL = "col-form-label";
        const string COL_MD_2 = "col-md-2";
        const string COL_MD_10 = "col-md-10";
        const string TEXT_DANGER = "text-danger";
        const string FORM_GROUP = "form-group ";

        //TAG'S
        const string DIV = "div";

        //attributes
        const string FILE = "file";
        const string TYPE = "type";
        public const string FILE_SUFIX = "FileUploader";

        public static List<string> externalForm = new List<string>();
        public static int externalFormCount = 0;
        public static MvcHtmlString _EWSDropdown(this HtmlHelper helper, string name, string displayName, bool required, string disabled, string master, string url = "", string key = "", string value = "", string parameter = "", Guid? modelValue = null, string style = "", bool addNew = false, string cssClass = "", bool secinizEkle = true)
        {
            string textBox = "";
            string req = "";
            if (required)
                req = "@required='required'";
            else
                req = "";

            if (displayName != "")
                textBox += string.Format("<label id='lbl{0}' class='{2}'>{1}</label>", name, displayName, CONTROL_LABEL);
            //textBox += string.Format("<select id='{0}' class='{1}' name='{0}' {2} {3}/>", name, FORM_CONTROL, req, disabled);

            string[] str = url.Split('/');
            url = ResolveServerUrl(VirtualPathUtility.ToAbsolute("~/" + str[0] + "/" + str[1]), false);

            WebClient client = new WebClient();
            dynamic dynJson = "";
            //var content = client.DownloadString(url);

            //if (modelValue != null)
            //    url = url + "?" + key + "=" + modelValue.ToString();

            var returnValues = client.DownloadData(url);

            if (master == "" || modelValue != null)
                dynJson = JsonConvert.DeserializeObject<object>(Encoding.UTF8.GetString(returnValues));


            //textBox += "<option value=''>Lütfen Seçiniz</option>";
            //foreach (var item in dynJson)
            //{
            //    textBox += "<option value='" + item[key] + "'>" + item[value] + "</option>";
            //}

            List<SelectListItem> list = new List<SelectListItem>();

            if (secinizEkle)
            {
                SelectListItem selectItem = new SelectListItem();
                selectItem.Value = "";
                selectItem.Text = "Lütfen Seçiniz";

                list.Add(selectItem);
            }
            string MasterID = "";
            foreach (var item in dynJson)
            {
                SelectListItem selItem = new SelectListItem();
                selItem.Value = item[key];
                selItem.Text = item[value];
                if (modelValue != null && modelValue == Guid.Parse(item[key].ToString()))
                    selItem.Selected = true;
                MasterID = Convert.ToString(item[master]);
                list.Add(selItem);
            }

            //textBox += string.Format("</select>");


            string textBox2 = "";
            if (master != "")
            {
                string[] url2 = null;
                string newURL = "";

                url2 = url.Split('?');
                newURL = url2[0];


                textBox2 += "<script>$('#" + master + "').change(function(){";
                textBox2 += "$('#" + name + "').children().remove();";
                textBox2 += "$('#" + name + "')";
                textBox2 += ".append($('<option>',  {value: '' }) ";
                textBox2 += ".text('Lütfen Seçiniz'));";
                textBox2 += "$.ajax({";
                textBox2 += "          type: 'GET',";
                textBox2 += "          url: '" + newURL + "',";
                textBox2 += "          data: { " + parameter + " : $('#" + master + "').val() },";
                textBox2 += "          cache: false,";
                textBox2 += "          contenttype: 'application/json; charset=utf-8',";
                textBox2 += "          success: function myfunction(text,data) {";
                textBox2 += "            for (i = 0; i < text.length ; i++)";
                textBox2 += "                {";
                textBox2 += "                        $('#" + name + "')";
                textBox2 += "                            .append($('<option>', { value: text[i]['" + key + "'] }) ";
                textBox2 += "		                    .text(text[i]['" + value + "']));";
                textBox2 += "                }";
                textBox2 += "          },";
                textBox2 += "      async: false";
                textBox2 += "  });";
                textBox2 += "});";

                textBox2 += "</script>";
            }

            if (cssClass == "")
                cssClass = "form-control";

            MvcHtmlString returnString = System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, list, new { @class = cssClass, req, @style = style });

            if (addNew)
            {
                textBox += "<div class='row'>";
                textBox = textBox + "<div class='col-sm-11'>" + returnString.ToString() + "</div><div class='col-sm-1' style='padding:0px;padding-top:12px;'>" +
                    "<a href='javascript:void(0)' onclick='Toggle()'><i class='fa fa-plus'></i></a></div></div><div id='dropCollapse" + name + "' class='collapse'><div class='row'><div class='col-sm-11'><input type='text' class='form-control'></input></div><div class='col-sm-1' style='padding:0px;'><a href='javascript:void(0)'><i class='fa fa-check'></i></a></div></div></div>" + textBox2;

                textBox += "<script> function Toggle() {  $('#dropCollapse" + name + "').slideToggle(); } </script>";
            }
            else
                textBox = textBox + returnString.ToString() + textBox2;


            textBox += "<script>$('#" + name + "').val('" + modelValue + "');</script>";

            return new MvcHtmlString(textBox);

            //return new MvcHtmlString(textBox);
        }
        public static MvcHtmlString _EWSDropdownWithMultiple(this HtmlHelper helper, string name, string displayName, bool required, string disabled, string master, string url = "", string key = "", string value = "", string parameter = "", Guid? modelValue = null, string style = "", bool addNew = false, string cssClass = "", string multiple = "")
        {
            string textBox = "";
            string req = "";
            if (required)
                req = "@required='required'";
            else
                req = "";

            if (displayName != "")
                textBox += string.Format("<label id='lbl{0}' class='{2}'>{1}</label>", name, displayName, CONTROL_LABEL);
            //textBox += string.Format("<select id='{0}' class='{1}' name='{0}' {2} {3}/>", name, FORM_CONTROL, req, disabled);

            string[] str = url.Split('/');
            url = ResolveServerUrl(VirtualPathUtility.ToAbsolute("~/" + str[0] + "/" + str[1]), false);

            WebClient client = new WebClient();
            dynamic dynJson = "";
            //var content = client.DownloadString(url);

            //if (modelValue != null)
            //    url = url + "?" + key + "=" + modelValue.ToString();

            var returnValues = client.DownloadData(url);

            if (master == "" || modelValue != null)
                dynJson = JsonConvert.DeserializeObject<object>(Encoding.UTF8.GetString(returnValues));


            //textBox += "<option value=''>Lütfen Seçiniz</option>";
            //foreach (var item in dynJson)
            //{
            //    textBox += "<option value='" + item[key] + "'>" + item[value] + "</option>";
            //}

            List<SelectListItem> list = new List<SelectListItem>();

            if (multiple == "")
            {
                SelectListItem selectItem = new SelectListItem();
                selectItem.Value = "";
                selectItem.Text = "Lütfen Seçiniz";

                list.Add(selectItem);
            }
            string MasterID = "";
            foreach (var item in dynJson)
            {
                SelectListItem selItem = new SelectListItem();
                selItem.Value = item[key];
                selItem.Text = item[value];
                if (modelValue != null && modelValue == Guid.Parse(item[key].ToString()))
                    selItem.Selected = true;
                MasterID = Convert.ToString(item[master]);
                list.Add(selItem);
            }

            //textBox += string.Format("</select>");


            string textBox2 = "";
            if (master != "")
            {
                string[] url2 = null;
                string newURL = "";

                url2 = url.Split('?');
                newURL = url2[0];


                textBox2 += "<script>$('#" + master + "').change(function(){";
                textBox2 += "$('#" + name + "').children().remove();";
                textBox2 += "$('#" + name + "')";
                textBox2 += ".append($('<option>',  {value: '' }) ";
                textBox2 += ".text('Lütfen Seçiniz'));";
                textBox2 += "$.ajax({";
                textBox2 += "          type: 'GET',";
                textBox2 += "          url: '" + newURL + "',";
                textBox2 += "          data: { " + parameter + " : $('#" + master + "').val() },";
                textBox2 += "          cache: false,";
                textBox2 += "          contenttype: 'application/json; charset=utf-8',";
                textBox2 += "          success: function myfunction(text,data) {";
                textBox2 += "            for (i = 0; i < text.length ; i++)";
                textBox2 += "                {";
                textBox2 += "                        $('#" + name + "')";
                textBox2 += "                            .append($('<option>', { value: text[i]['" + key + "'] }) ";
                textBox2 += "		                    .text(text[i]['" + value + "']));";
                textBox2 += "                }";
                textBox2 += "          },";
                textBox2 += "      async: false";
                textBox2 += "  });";
                textBox2 += "});";

                textBox2 += "</script>";
            }

            if (cssClass == "")
                cssClass = "form-control";

            MvcHtmlString returnString = System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, list, new { @class = cssClass, req, @style = style, @multiple = multiple });

            if (addNew)
            {
                textBox += "<div class='row'>";
                textBox = textBox + "<div class='col-sm-11'>" + returnString.ToString() + "</div><div class='col-sm-1' style='padding:0px;padding-top:12px;'>" +
                    "<a href='javascript:void(0)' onclick='Toggle()'><i class='fa fa-plus'></i></a></div></div><div id='dropCollapse" + name + "' class='collapse'><div class='row'><div class='col-sm-11'><input type='text' class='form-control'></input></div><div class='col-sm-1' style='padding:0px;'><a href='javascript:void(0)'><i class='fa fa-check'></i></a></div></div></div>" + textBox2;

                textBox += "<script> function Toggle() {  $('#dropCollapse" + name + "').slideToggle(); } </script>";
            }
            else
                textBox = textBox + returnString.ToString() + textBox2;


            textBox += "<script>$('#" + name + "').val('" + modelValue + "');</script>";

            return new MvcHtmlString(textBox);

            //return new MvcHtmlString(textBox);
        }
        public static MvcHtmlString _EWSTextBox(this HtmlHelper helper, string name, string displayName, bool required, string disabled, string datatype = "", string placeholder = "", int maxlenght = 0, string style = "", string value = "")
        {
            string textBox = "";
            string req = "";
            if (required)
                req = "required";

            if (datatype == "")
                datatype = "text";

            string maxLng = "";

            if (maxlenght != 0)
                maxLng = "maxlenght='" + maxlenght.ToString() + "'";

            if (value != "")
                value = "value='" + value + "'";

            if (placeholder != "")
                placeholder = "placeholder='" + placeholder + "'";

            textBox += string.Format("<label id='lbl{0}' class='{2}'>{1}</label>", name, displayName, CONTROL_LABEL);
            textBox += string.Format("<input type='{5}' name='{0}' id='{0}' class='{1}' {2} {0} {3} {4} {6} style='{7}' {8}/>", name, FORM_CONTROL, req, disabled, placeholder, datatype, maxLng, style, value);

            return new MvcHtmlString(textBox);
        }
        public static MvcHtmlString _EWSHidden(this HtmlHelper helper, string name, string value)
        {
            string textBox = "";
            textBox += string.Format("<input type='hidden' name='{0}' id='{0}' value='{1}'/>", name, value);

            return new MvcHtmlString(textBox);
        }

        public static MvcHtmlString _EWSTextArea(this HtmlHelper helper, string name, string displayName, bool required, string disabled, string datatype = "", string value = "", string placeholder = "")
        {
            string textBox = "";
            string req = "";
            if (required)
                req = "required";

            if (datatype == "")
                datatype = "text";

            if (value != "")
                value = "value='" + value + "'";

            textBox += string.Format("<label id='lbl{0}' class='{2}'>{1}</label>", name, displayName, CONTROL_LABEL);
            textBox += string.Format("<textarea name='{0}' id='{0}' class='{1}' {2} {0} {3} placeholder='{4}' {5}/></textarea>", name, FORM_CONTROL, req, disabled, placeholder, value);

            return new MvcHtmlString(textBox);
        }

        public static MvcHtmlString _EWSDateTimePicker(this HtmlHelper helper, string name, string displayName, bool required, string disabled, string datatype = "", DateTime? value = null, string style = "")
        {
            string textBox = "";
            string req = "";
            if (required)
                req = "required";

            string val = "";
            if (value.HasValue)
            {
                DateTime newVal = (DateTime)value;
                val = "value='" + newVal.ToString("yyyy-MM-dd") + "'";
            }
            textBox += string.Format("<label id='lbl{0}' class='{2}'>{1}</label>", name, displayName, CONTROL_LABEL);
            textBox += string.Format("<input type='date' name='{0}' id='{0}' class='{1}' {2} {3} {4} style='{5}'>", name, FORM_CONTROL, req, disabled, val, style);

            return new MvcHtmlString(textBox);
        }

        public static MvcHtmlString _EWSEnumToList<T>(this HtmlHelper helper, T entity, string name, string displayName, bool required, string disabled, string datatype = "", string value = "") where T : class
        {
            //todo: sisel : Bu kısmı yazamadım. Şimdilik Dropdown kullanılacak enum değerler için.
            //List<T> values = Helper.EnumToList<T>();

            string textBox = "";

            textBox += string.Format("<label id='lbl{0}' class='{2}'>{1}</label>", name, displayName, CONTROL_LABEL);
            textBox += string.Format("<input type='date' name='{0}' id='{0}' class='{1}' {2} {3}>", name, FORM_CONTROL, required, disabled);

            return new MvcHtmlString(textBox);
        }

        public static MvcHtmlString _EWSCheckBox(this HtmlHelper helper, string name, string displayName, bool required, string disabled, int lenght = 12, string datatype = "", string value = "", string classname = "")
        {
            string textBox;

            if (classname == "blue-style")
            {
                textBox = string.Format("<label id='lbl{0}'>", name);
                textBox += displayName + "&nbsp;&nbsp;&nbsp;&nbsp;";

                MvcHtmlString returnString = System.Web.Mvc.Html.InputExtensions.CheckBox(helper, name, new { @class = classname });
                //textBox += string.Format("<input type='checkbox'  name='{0}' id='{0}' class='{1}' {2} {3} value='{4}' />", name, classname, required, disabled, value);
                textBox += returnString.ToString() + "</label>";
            }
            else if (classname == "line-style")
            {
                //textBox = string.Format("<input type='checkbox'  name='{0}' id='{0}' class='{1}' {2} {3} value='{4}'/>", name, classname, required, disabled, value);
                textBox = string.Format("<label id='lbl{0}'>{1}", name, displayName);
                MvcHtmlString returnString = System.Web.Mvc.Html.InputExtensions.CheckBox(helper, name, new { @class = classname });
                textBox = returnString.ToString() + textBox;
                textBox += "</label>";
            }
            else
            {
                textBox = string.Format("<label id='lbl{0}'>{1}&nbsp;&nbsp;&nbsp;&nbsp;", name, displayName);
                MvcHtmlString returnString = System.Web.Mvc.Html.InputExtensions.CheckBox(helper, name);
                textBox = textBox + returnString.ToString();
                textBox += "</label>";
            }

            return new MvcHtmlString(textBox);
        }
        public static MvcHtmlString _EWSTitle(this HtmlHelper helper, string targetname, string displayName, int lenght = 12, string badge = "")
        {
            string textBox;
            //textBox = "<div class='well'>";
            textBox = " <div class='pageparttitle' data-toggle='collapse' data-target='#" + targetname + "'>";

            if (badge != "")
                textBox += string.Format("<h4><span class='badge badge-pill badge-primary'>{1}</span> {0}</h4>", displayName, badge);
            else
                textBox += string.Format("<h4>{0}</h4>", displayName);

            textBox += "</div>";

            return new MvcHtmlString(textBox);
        }
        public static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            if (serverUrl.IndexOf("://") > -1)
                return serverUrl;

            string newUrl = serverUrl;
            Uri originalUri = System.Web.HttpContext.Current.Request.Url;
            newUrl = (forceHttps ? "https" : originalUri.Scheme) +
                "://" + originalUri.Authority + newUrl;
            return newUrl;
        }
        public static string colSpan(int lenght)
        {
            //if (lenght == 12)
            //    lenght = 10;

            return string.Format("col-lg-{0} col-md-{0} col-sm-12 col-xs-12", lenght);
        }

    }
}