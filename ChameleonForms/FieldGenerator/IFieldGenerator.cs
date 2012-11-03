﻿using System.Web;
using System.Web.Mvc;

namespace ChameleonForms.FieldGenerator
{
    /// <summary>
    /// Generates the HTML for a single form field.
    /// </summary>
    public interface IFieldGenerator
    {
        /// <summary>
        /// The metadata for the form field.
        /// </summary>
        ModelMetadata Metadata { get; }

        /// <summary>
        /// Creates the HTML for the field control.
        /// </summary>
        /// <returns>The HTML for the field control</returns>
        IHtmlString GetFieldHtml();

        /// <summary>
        /// Creates the HTML for the field label.
        /// </summary>
        /// <returns>The HTML for the field label</returns>
        IHtmlString GetLabelHtml();

        /// <summary>
        /// Creates the HTML for the field's validation messages
        /// </summary>
        /// <returns>The HTML for the field's validation messages</returns>
        IHtmlString GetValidationHtml();
    }
}
