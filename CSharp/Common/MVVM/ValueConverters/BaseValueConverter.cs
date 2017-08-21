﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Common.ValueConverters
{
    /// <summary>
    /// A base value converter that allows direct XAML usage.
    /// </summary>
    /// <typeparam name="T">The type of this value converter.</typeparam>
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
        where T : class, new()
    {
        #region private fields
        /// <summary>
        /// A single static instance of this value converter.
        /// </summary>
        private static T _converter = null;
        #endregion

        #region markup extension methods
        /// <summary>
        /// Provides a static instnace of the value converter.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_converter == null)
                _converter = new T();

            return _converter;
        } 
        #endregion

        #region value converter methods
        /// <summary>
        /// The method to convert one type to another.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// The method to convert a value back to it's source type.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
        #endregion
    }
}
