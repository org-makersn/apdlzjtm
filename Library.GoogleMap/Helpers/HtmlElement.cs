﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Library.GoogleMap
{
    public class HtmlElement : IHtmlNode
    {
        private readonly TagBuilder tagBuilder;
        private Action<TextWriter> TemplateCallback
        {
            get;
            set;
        }
        public TagRenderMode RenderMode
        {
            get;
            private set;
        }
        public string TagName
        {
            get
            {
                return this.tagBuilder.TagName;
            }
        }
        public IList<IHtmlNode> Children
        {
            get;
            private set;
        }
        public string InnerHtml
        {
            get
            {
                if (this.Children.Any<IHtmlNode>())
                {
                    StringBuilder innerHtml = new StringBuilder();
                    this.Children.Each(delegate(IHtmlNode child)
                    {
                        innerHtml.Append(child.ToString());
                    });
                    return innerHtml.ToString();
                }
                return this.tagBuilder.InnerHtml;
            }
        }
        public HtmlElement(string tagName)
            : this(tagName, TagRenderMode.Normal)
        {
        }
        public HtmlElement(string tagName, TagRenderMode renderMode)
        {
            this.tagBuilder = new TagBuilder(tagName);
            this.Children = new List<IHtmlNode>();
            this.RenderMode = renderMode;
        }
        public IDictionary<string, string> Attributes()
        {
            return this.tagBuilder.Attributes;
        }
        public string Attribute(string key)
        {
            return this.Attributes()[key];
        }
        public override string ToString()
        {
            string result;
            using (StringWriter output = new StringWriter(CultureInfo.CurrentCulture))
            {
                this.WriteTo(output);
                result = output.GetStringBuilder().ToString();
            }
            return result;
        }
        public IHtmlNode Attributes(object attributes)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            if (attributes != null)
            {
                foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(attributes))
                {
                    dictionary.Add(property.Name.Replace("_", "-"), property.GetValue(attributes));
                }
            }

            this.Attributes<string, object>(dictionary);
            return this;
        }
        public IHtmlNode AddClass(params string[] classes)
        {
            for (int i = 0; i < classes.Length; i++)
            {
                string @class = classes[i];
                string value;
                if (this.Attributes().TryGetValue("class", out value))
                {
                    this.Attributes()["class"] = value + " " + @class;
                }
                else
                {
                    this.Attributes()["class"] = @class;
                }
            }
            return this;
        }
        public IHtmlNode Css(string key, string value)
        {
            string style;
            if (this.Attributes().TryGetValue("style", out style))
            {
                if (!style.Contains("display:none"))
                {
                    this.Attributes()["style"] = string.Concat(new string[]
					{
						style,
						";",
						key,
						":",
						value
					});
                }
            }
            else
            {
                this.Attributes()["style"] = key + ":" + value;
            }
            return this;
        }
        public IHtmlNode ToggleCss(string key, string value, bool condition)
        {
            if (condition)
            {
                this.Css(key, value);
            }
            return this;
        }
        public IHtmlNode PrependClass(string[] classes)
        {
            foreach (string @class in classes.Reverse<string>())
            {
                this.tagBuilder.AddCssClass(@class);
            }
            return this;
        }
        public IHtmlNode ToggleClass(string @class, bool condition)
        {
            if (condition)
            {
                this.AddClass(new string[]
				{
					@class
				});
            }
            return this;
        }
        public IHtmlNode Attributes<TKey, TValue>(IDictionary<TKey, TValue> values)
        {
            return this.Attributes<TKey, TValue>(values, true);
        }
        public IHtmlNode Attributes<TKey, TValue>(IDictionary<TKey, TValue> values, bool replaceExisting)
        {
            this.tagBuilder.MergeAttributes<TKey, TValue>(values, replaceExisting);
            return this;
        }
        public IHtmlNode AppendTo(IHtmlNode parent)
        {
            parent.Children.Add(this);
            return this;
        }
        public IHtmlNode Attribute(string key, string value)
        {
            return this.Attribute(key, value, true);
        }
        public IHtmlNode Attribute(string key, string value, bool replaceExisting)
        {
            this.tagBuilder.MergeAttribute(key, value, replaceExisting);
            return this;
        }
        public IHtmlNode Html(string value)
        {
            this.Children.Clear();
            this.Children.Add(new LiteralNode(value));
            return this;
        }
        public IHtmlNode Template(Action<TextWriter> value)
        {
            this.TemplateCallback = value;
            return this;
        }
        public Action<TextWriter> Template()
        {
            return this.TemplateCallback;
        }
        public IHtmlNode ToggleAttribute(string key, string value, bool condition)
        {
            if (condition)
            {
                this.Attribute(key, value);
            }
            return this;
        }
        public IHtmlNode Text(string value)
        {
            this.tagBuilder.SetInnerText(value);
            this.Children.Clear();
            return this;
        }
        public void WriteTo(TextWriter output)
        {
            if (this.RenderMode != TagRenderMode.SelfClosing)
            {
                output.Write(this.tagBuilder.ToString(TagRenderMode.StartTag));
                if (this.TemplateCallback != null)
                {
                    this.TemplateCallback(output);
                }
                else
                {
                    if (this.Children.Any<IHtmlNode>())
                    {
                        this.Children.Each(delegate(IHtmlNode child)
                        {
                            child.WriteTo(output);
                        });
                    }
                    else
                    {
                        output.Write(this.tagBuilder.InnerHtml);
                    }
                }
                output.Write(this.tagBuilder.ToString(TagRenderMode.EndTag));
                return;
            }
            output.Write(this.tagBuilder.ToString(TagRenderMode.SelfClosing));
        }
    }
}
