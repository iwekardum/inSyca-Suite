namespace insyca.bt.gl.esb.schemas.entities.business.logistics {
    using Microsoft.XLANGs.BaseTypes;
    
    
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.BizTalk.Schema.Compiler", "3.0.1.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [SchemaType(SchemaTypeEnum.Document)]
    [Schema(@"http://insyca.bt.gl.esb.schemas.entities.business.logistics.handling_10",@"handling")]
    [System.SerializableAttribute()]
    [SchemaRoots(new string[] {@"handling"})]
    public sealed class handling_10 : Microsoft.XLANGs.BaseTypes.SchemaBase {
        
        [System.NonSerializedAttribute()]
        private static object _rawSchema;
        
        [System.NonSerializedAttribute()]
        private const string _strSchema = @"<?xml version=""1.0"" encoding=""utf-16""?>
<xs:schema xmlns=""http://insyca.bt.gl.esb.schemas.entities.business.logistics.handling_10"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" targetNamespace=""http://insyca.bt.gl.esb.schemas.entities.business.logistics.handling_10"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""handling"" nillable=""true"">
    <xs:annotation>
      <xs:appinfo>
        <b:recordInfo notes=""Umschlag von Gütern, Be- und Entladen, Verladen"" xmlns:b=""http://schemas.microsoft.com/BizTalk/2003"" />
      </xs:appinfo>
    </xs:annotation>
    <xs:complexType>
      <xs:sequence minOccurs=""0"" maxOccurs=""1"">
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""type"" nillable=""true"" type=""xs:string"">
          <xs:annotation>
            <xs:appinfo>
              <b:fieldInfo notes=""Typ"" />
            </xs:appinfo>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""priority"" nillable=""true"" type=""xs:integer"">
          <xs:annotation>
            <xs:appinfo>
              <b:fieldInfo notes=""priorität"" />
            </xs:appinfo>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""gate"" nillable=""true"" type=""xs:string"">
          <xs:annotation>
            <xs:appinfo>
              <b:fieldInfo notes=""Inhaltsbezeichnung"" />
            </xs:appinfo>
          </xs:annotation>
        </xs:element>
        <xs:element minOccurs=""0"" maxOccurs=""1"" name=""loading_sequence"" nillable=""true"" type=""xs:integer"" />
        <xs:element name=""blocked_flag"" type=""xs:boolean"">
          <xs:annotation>
            <xs:appinfo>
              <b:fieldInfo notes=""Kennzeichen gesperrt"" />
            </xs:appinfo>
          </xs:annotation>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
        
        public handling_10() {
        }
        
        public override string XmlContent {
            get {
                return _strSchema;
            }
        }
        
        public override string[] RootNodes {
            get {
                string[] _RootElements = new string [1];
                _RootElements[0] = "handling";
                return _RootElements;
            }
        }
        
        protected override object RawSchema {
            get {
                return _rawSchema;
            }
            set {
                _rawSchema = value;
            }
        }
    }
}
