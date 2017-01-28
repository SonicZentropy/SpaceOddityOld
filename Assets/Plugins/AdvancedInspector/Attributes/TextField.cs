using System;

namespace AdvancedInspector
{
    /// <summary>
    /// This allows control over how a string field is displayed. 
    /// Only useful on string field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class TextFieldAttribute : Attribute, IListAttribute
    {
        private const string TITLE = "Select Path...";
        private const string PATH = "";
        private const string EXTENSION = "";

        private string title = "";

        /// <summary>
        /// Title of the modal dialog
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string path = "C:\\";

        /// <summary>
        /// Default file/folder path
        /// </summary>
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        private string extension = "";

        /// <summary>
        /// Force the file dialog to show only specific file type.
        /// </summary>
        public string Extension
        {
            get { return extension; }
            set { extension = value; }
        }

        private TextFieldType type;

        /// <summary>
        /// What type of control is this string.
        /// </summary>
        public TextFieldType Type
        {
            get { return type; }
            set { type = value; }
        }

        public TextFieldAttribute(TextFieldType type)
            : this(type, PATH, TITLE, EXTENSION) { }

        public TextFieldAttribute(TextFieldType type, string path)
            : this(type, path, TITLE, EXTENSION) { }

        public TextFieldAttribute(TextFieldType type, string path, string title)
            : this(type, path, title, EXTENSION) { }

        public TextFieldAttribute(TextFieldType type, string path, string title, string extension)
        {
            this.type = type;
            this.title = title;
            this.path = path;
            this.extension = extension;
        }
    }
}