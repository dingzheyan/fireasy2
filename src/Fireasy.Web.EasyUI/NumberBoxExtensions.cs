﻿// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Fireasy.Web.Mvc;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Fireasy.Web.EasyUI
{
    public static class NumberBoxExtensions
    {
        /// <summary>
        /// 为 <see cref="HtmlHelper"/> 对象扩展 numberbox 元素。
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="exp">属性名或使用 txt 作为前缀的 ID 名称。</param>
        /// <param name="settings">参数选项。</param>
        /// <returns></returns>
        public static ExtendHtmlString NumberBox(this HtmlHelper htmlHelper, string exp, NumberBoxSettings settings = null)
        {
            settings = settings ?? new NumberBoxSettings();

            var builder = new EasyUITagBuilder("input", "easyui-numberbox", settings);
            builder.MergeAttribute("name", exp);
            builder.MergeAttribute("data-options", SettingsSerializer.Serialize(settings));
            builder.AddCssClass("form-input");
            builder.GenerateId("txt" + exp);
            return new ExtendHtmlString(builder);
        }

        /// <summary>
        /// 为 <see cref="HtmlHelper"/> 对象扩展 numberbox 元素。
        /// </summary>
        /// <typeparam name="TModel">数据模型类型。</typeparam>
        /// <typeparam name="TProperty">绑定的属性的类型。</typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression">指定绑定属性的表达式。</param>
        /// <param name="settings">参数选项。</param>
        /// <returns></returns>
        public static ExtendHtmlString NumberBox<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, NumberBoxSettings settings = null)
        {
            settings = settings ?? new NumberBoxSettings();

            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var propertyName = metadata.PropertyName;
            settings.Bind(typeof(TModel), propertyName);

            var builder = new EasyUITagBuilder("input", "easyui-numberbox", settings);
            builder.MergeAttribute("name", propertyName);
            builder.MergeAttribute("data-options", SettingsSerializer.Serialize(settings));
            builder.AddCssClass("form-input");
            builder.GenerateId("txt" + propertyName);
            return new ExtendHtmlString(builder);
        }
    }
}
