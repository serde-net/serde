﻿
#nullable enable
using Serde;

namespace Serde.Test
{
    partial class SampleTest
    {
        partial record Address : Serde.ISerialize
        {
            void Serde.ISerialize.Serialize(ISerializer serializer)
            {
                var type = serializer.SerializeType("Address", 5);
                type.SerializeField("Name", new StringWrap(this.Name), new System.Attribute[] { new System.Xml.Serialization.XmlAttributeAttribute() { }, new Serde.SerdeMemberOptions() { ProvideAttributes = true } });
                type.SerializeField("Line1", new StringWrap(this.Line1));
                type.SerializeFieldIfNotNull("City", new NullableRefWrap.SerializeImpl<string, StringWrap>(this.City), this.City);
                type.SerializeField("State", new StringWrap(this.State));
                type.SerializeField("Zip", new StringWrap(this.Zip));
                type.End();
            }
        }
    }
}