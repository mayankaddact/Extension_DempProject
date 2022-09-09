using Newtonsoft.Json;
using Sitecore;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Reflection;
using Feature.FormsExtensions.Fields;
using Sitecore.Data.Managers;
using Sitecore.Globalization;

namespace Feature.FormsExtensions.SubmitActions.SendEmail
{
    public class SendEmaildata : SubmitActionBase<FormSendParameter>
    {
        ISubmitActionData _submitActionData;

        public SendEmaildata(ISubmitActionData submitActionData) : base(submitActionData)
        {
            _submitActionData = submitActionData;
        }
        protected override bool TryParse(string value, out FormSendParameter target)
        {
            if (string.IsNullOrEmpty(value))
            {
                target = default(FormSendParameter);
                return false;
            }
            try
            {
                target = JsonConvert.DeserializeObject<FormSendParameter>(value);
            }
            catch (JsonException ex)
            {
                target = default(FormSendParameter);
                return false;
            }
            return target != null;
        }
        protected override bool Execute(FormSendParameter data, FormSubmitContext formSubmitContext)
        {

            bool result = false;
            try
            {
                var emailTemplate = Context.Database.GetItem(new ID(data.messageId), Language.Parse("en"));
                if (emailTemplate == null)
                    return false;

                var emailMessage =
                   new MailMessage
                   {
                       // Subject
                       Subject = emailTemplate.Fields["Subject"].Value,
                       // From
                       From = new MailAddress(!string.IsNullOrEmpty(emailTemplate.Fields["From Address"].Value) ? emailTemplate.Fields["From Address"].Value : "support@globalbajaj.com")
                   };
               
                if(data.type == "fieldValue")
                {
                    foreach (var filed in formSubmitContext.Fields)
                    {
                        if(filed.ItemId == data.fieldEmailAddressId)
                        {
                            string emailto = GetValue(filed);
                            var toAdd = !string.IsNullOrEmpty(emailto) ? emailto : emailTemplate.Fields["Reply To"].Value;
                            emailMessage.To.Add(toAdd);
                        }
                    }
                }
                else
                {
                    var toAdd = !string.IsNullOrEmpty(data.fixedEmailAddress) ? data.fixedEmailAddress : emailTemplate.Fields["Reply To"].Value;
                    emailMessage.To.Add(toAdd);
                }

               

                emailMessage.Body = emailTemplate.Fields["Body"].Value;
                var formfields = new NameValueCollection();
                foreach (var filed in formSubmitContext.Fields)
                {
                    emailMessage.Body = emailMessage.Body.Replace("$" + "form_" + filed.Name + "$", GetValue(filed));
                    formfields.Add(filed.Name, GetValue(filed));
                }
                emailMessage.IsBodyHtml = true;

                MainUtil.SendMail(emailMessage);

                //Log.Info("Email sent successfully - " + toAdd,this);
                return true;
            }
            catch (Exception e)
            {
                Log.Error($"[SendMail Action] Error sending e-mail based on template {data.messageId}", e, this);
                return result;
            }
        }
        protected string GetValue(IViewModel postedField)
        {
            Assert.ArgumentNotNull((object)postedField, "postedField");
            IValueField valueField = postedField as IValueField;
            PropertyInfo property = postedField.GetType().GetProperty("Value");
            object obj;
            if (property == null)
            {
                obj = (object)null;
            }
            else
            {
                IViewModel viewModel = postedField;
                obj = property.GetValue((object)viewModel);
            }

            object postedValue = obj;
            if (postedValue == null)
                return string.Empty;
            string parsedValue = ParseFieldValue(postedValue);

            if (parsedValue == "True" || parsedValue == "False")
                parsedValue = parsedValue.ToLower();
            return parsedValue;
        }

        protected static string ParseFieldValue(object postedValue)
        {
            Assert.ArgumentNotNull(postedValue, "postedValue");
            List<string> list = new List<string>();
            IList secondList = postedValue as IList;
            if (secondList != null)
            {
                foreach (object obj in (IEnumerable)secondList)
                    list.Add(obj.ToString());
            }
            else
                list.Add(postedValue.ToString());
            return string.Join(",", (IEnumerable<string>)list);
        }
    }
}