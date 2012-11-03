﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Humanizer;

namespace ChameleonForms.FieldGenerator
{
    /// <summary>
    /// The default field HTML generator.
    /// </summary>
    /// <typeparam name="TModel">The type of the view model for the form</typeparam>
    /// <typeparam name="T">The type of the field being generated</typeparam>
    public class DefaultFieldGenerator<TModel, T> : IFieldGenerator
    {
        #region Setup
        private readonly HtmlHelper<TModel> _helper;
        private readonly Expression<Func<TModel, T>> _property;

        /// <summary>
        /// Constructs the field generator.
        /// </summary>
        /// <param name="helper">The HTML helper for the current view</param>
        /// <param name="property">An expression to identify the property to generate the field for</param>
        public DefaultFieldGenerator(HtmlHelper<TModel> helper, Expression<Func<TModel, T>> property)
        {
            _helper = helper;
            _property = property;
            Metadata = ModelMetadata.FromLambdaExpression(_property, _helper.ViewData);
        }
        #endregion

        #region IFieldGenerator Methods

        public ModelMetadata Metadata { get; private set; }

        public IHtmlString GetLabelHtml()
        {
            return _helper.LabelFor(_property);
        }

        public IHtmlString GetValidationHtml()
        {
            return _helper.ValidationMessageFor(_property);
        }

        public IHtmlString GetFieldHtml()
        {
            var typeAttribute = default(string);

            if (Metadata.ModelType.IsEnum)
                return GetEnumHtml(_property.Compile().Invoke((TModel) _helper.ViewData.ModelMetadata.Model));

            if (Metadata.DataTypeName == DataType.Password.ToString())
                return _helper.PasswordFor(_property);

            if (Metadata.DataTypeName == DataType.MultilineText.ToString())
                return _helper.TextAreaFor(_property);

            if (typeof(HttpPostedFileBase).IsAssignableFrom(Metadata.ModelType))
                typeAttribute = "file";

            if (typeAttribute == default(string))
                typeAttribute = "text";

            var attrs = new { type = typeAttribute};
            return _helper.TextBoxFor(_property, attrs);
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Creates the HTML for a drop down list that wraps an enumeration field.
        /// </summary>
        /// <param name="value">The current value of the field</param>
        /// <returns>The HTML for the drop down list</returns>
        public virtual IHtmlString GetEnumHtml(T value)
        {
            var selectList = Enum.GetValues(typeof(T)).OfType<T>().Select(i => new SelectListItem { Text = (i as Enum).Humanize(), Value = i.ToString(), Selected = i.Equals(value)});
            return _helper.DropDownListFor(_property, selectList);
        }
        #endregion
    }
}
