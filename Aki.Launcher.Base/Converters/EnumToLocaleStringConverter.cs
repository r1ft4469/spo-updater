/* EnumToLocaleStringConverter.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * waffle.lord
 */


using Aki.Launcher.Helpers;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Aki.Launcher.Converters
{
    public class EnumToLocaleStringConverter : EnumConverter
    {
        public EnumToLocaleStringConverter(Type type)
            : base(type)
        {
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                if (value != null)
                {
                    //this adds an underscore before capitalized letters, except if it is the first letter in the string. Then it is lower cased.
                    //The result should be the name of the localization providers property you want to use.
                    //Example: MinimizeAction  ->  minimize_action
                    string localePropertyName = Regex.Replace(value.ToString(), "(?<!^)[A-Z]", "_$0").ToLower();

                    return LocalizationProvider.Instance.GetType().GetProperty(localePropertyName).GetValue(LocalizationProvider.Instance, null) ?? value;
                }

                return string.Empty;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
