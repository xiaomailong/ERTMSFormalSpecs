using System;

namespace XmlBooster
{
    public enum XmlBTypeKind
    {
        Predefined,
        Element,
        Enum,
        Simple
    }

    public enum XmlBMode
    {
        Single,
        Linked,
        List
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class XmlBSystemAttribute : Attribute
    {
        public XmlBSystemAttribute(string Name, Type acceptor)
        {
            this.Name = Name;
            this.acceptor = acceptor;
        }

        public string Name { get; set; }
        public Type acceptor { get; set; }
        public bool Polymorph { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class XmlBIndexFieldAttribute : Attribute
    {
        public XmlBIndexFieldAttribute(string SystemName, string FieldName)
        {
            this.SystemName = SystemName;
            this.FieldName = FieldName;
        }

        public string SystemName { get; set; }
        public string FieldName { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class XmlBElementAttribute : Attribute
    {
        public XmlBElementAttribute(Type System, string Name)
        {
            this.System = System;
            this.Name = Name;
        }

        public string Name { get; set; }
        public string BaseElement { get; set; }
        public bool Main { get; set; }
        public string ShortDesc { get; set; }
        public Type System { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class XmlBFieldAllowedTypeAttribute : Attribute
    {
        public XmlBFieldAllowedTypeAttribute(Type Type)
        {
            this.Type = Type;
        }

        public Type Type { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class XmlBFieldAttribute : Attribute
    {
        public XmlBFieldAttribute(Type fieldType, string Name, XmlBTypeKind TypeKind)
        {
            this.Name = Name;
            this.TypeKind = TypeKind;
            this.FieldType = fieldType;
        }

        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public XmlBTypeKind TypeKind { get; set; }
        public Type FieldType { get; set; }
        public XmlBMode Mode { get; set; }
        public int ArraySize { get; set; }
        public int MaxLen { get; set; }
        public string Indicator { get; set; }
        public bool Index { get; set; }
        public object EditorStyleKey { get; set; }
    }

    [AttributeUsage(AttributeTargets.Enum)]
    public class XmlBEnumAttribute : Attribute
    {
        public XmlBEnumAttribute(Type System, string Name)
        {
            this.System = System;
            this.Name = Name;
        }

        public Type System { get; set; }
        public string Name { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class XmlBEnumValueAttribute : Attribute
    {
        public XmlBEnumValueAttribute(string Name)
        {
            this.Name = Name;
        }

        public string Name { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class XmlBGroupAttribute : Attribute
    {
        public XmlBGroupAttribute(string Name)
        {
            this.Name = Name;
        }

        public string Name { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class XmlBFieldGroupAttribute : Attribute
    {
        public XmlBFieldGroupAttribute(string Name, string shortDesc)
        {
            this.Name = Name;
            this.ShortDesc = shortDesc;
        }

        public string Name { get; set; }
        public string ShortDesc { get; set; }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class XmlBBuilderAttribute : Attribute
    {
        public XmlBBuilderAttribute(Type Element)
        {
            this.Element = Element;
        }

        public Type Element { get; set; }
    }
}