﻿using System;
using Feature.FormsExtensions.Fields.Bindings;
using Sitecore.Data.Items;
using Sitecore.ExperienceForms.Mvc.Models.Fields;

namespace Feature.FormsExtensions.Fields.Hidden
{
    [Serializable]
    public class HiddenViewModel : InputViewModel<string>, IBindingSettings
    {
        public string BindingToken { get; set; }
        public bool PrefillBindingValue { get; set; }
        public bool StoreBindingValue { get; set; }

        protected override void InitItemProperties(Item item)
        {
            base.InitItemProperties(item);
            this.InitBindingSettingsProperties(item);
        }

        protected override void UpdateItemFields(Item item)
        {
            base.UpdateItemFields(item);
            this.UpdateBindingSettingsFields(item);
        }
    }
}