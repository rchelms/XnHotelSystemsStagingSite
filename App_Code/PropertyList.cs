﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3082
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=2.0.50727.42.
// 
namespace XHS.WBS.PropertyList {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.xnglobalres.com/PropertyList")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.xnglobalres.com/PropertyList", IsNullable=false)]
    public partial class PropertyList {
        
        private WBS_Property[] propertyField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Property")]
        public WBS_Property[] Property {
            get {
                return this.propertyField;
            }
            set {
                this.propertyField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.xnglobalres.com/PropertyList")]
    public partial class WBS_Property {
        
        private WBS_Area[] areasField;
        
        private string hotelCodeField;
        
        private string hotelNameField;
        
        private string brandCodeField;
        
        private string brandNameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Area", IsNullable=false)]
        public WBS_Area[] Areas {
            get {
                return this.areasField;
            }
            set {
                this.areasField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string HotelCode {
            get {
                return this.hotelCodeField;
            }
            set {
                this.hotelCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string HotelName {
            get {
                return this.hotelNameField;
            }
            set {
                this.hotelNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string BrandCode {
            get {
                return this.brandCodeField;
            }
            set {
                this.brandCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string BrandName {
            get {
                return this.brandNameField;
            }
            set {
                this.brandNameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.xnglobalres.com/PropertyList")]
    public partial class WBS_Area {
        
        private string areaIDField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string AreaID {
            get {
                return this.areaIDField;
            }
            set {
                this.areaIDField = value;
            }
        }
    }
}