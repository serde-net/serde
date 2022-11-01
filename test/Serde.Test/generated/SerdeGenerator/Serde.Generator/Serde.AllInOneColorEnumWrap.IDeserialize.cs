﻿
#nullable enable
using Serde;

namespace Serde
{
    partial record struct AllInOneColorEnumWrap : Serde.IDeserialize<Serde.Test.AllInOne.ColorEnum>
    {
        static Serde.Test.AllInOne.ColorEnum Serde.IDeserialize<Serde.Test.AllInOne.ColorEnum>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            return deserializer.DeserializeString<Serde.Test.AllInOne.ColorEnum, SerdeVisitor>(visitor);
        }

        private struct SerdeVisitor : Serde.IDeserializeVisitor<Serde.Test.AllInOne.ColorEnum>
        {
            public string ExpectedTypeName => "Serde.Test.AllInOne.ColorEnum";
            Serde.Test.AllInOne.ColorEnum Serde.IDeserializeVisitor<Serde.Test.AllInOne.ColorEnum>.VisitString(string s)
            {
                Serde.Test.AllInOne.ColorEnum enumValue;
                switch (s)
                {
                    case "red":
                        enumValue = Serde.Test.AllInOne.ColorEnum.Red;
                        break;
                    case "blue":
                        enumValue = Serde.Test.AllInOne.ColorEnum.Blue;
                        break;
                    case "green":
                        enumValue = Serde.Test.AllInOne.ColorEnum.Green;
                        break;
                    default:
                        throw new InvalidDeserializeValueException("Unexpected enum field name: " + s);
                }

                return enumValue;
            }
        }
    }
}