namespace Feature.FormsExtensions.Fields
{
    public class FormSendParameter
    {
        public string messageId { get; set; }
        public string type { get; set; }
        public string fixedEmailAddress { get; set; }
        public string fieldEmailAddressId { get; set; }
        public bool updateCurrentContact { get; set; }
        public string[] fileUploadFieldsToAttach { get; set; }
    }
}