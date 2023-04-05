
using System.Threading.Tasks;
using Xunit;
using static Serde.Test.GeneratorTestUtils;

namespace Serde.Test
{
    public class MemberFormatTests
    {
        [Fact]
        public Task Default()
        {
            var src = @"
using Serde;

[GenerateSerialize]
partial struct S
{
    public int One { get; set; }
    public int TwoWord { get; set; }
}";
            return VerifyGeneratedCode(src, "S.ISerialize", @"
#nullable enable
using Serde;

partial struct S : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType(""S"", 2);
        type.SerializeField(""one"", new Int32Wrap(this.One));
        type.SerializeField(""twoWord"", new Int32Wrap(this.TwoWord));
        type.End();
    }
}");
        }

        [Fact]
        public Task CamelCase()
        {
            var src = @"
using Serde;

[GenerateSerialize]
[SerdeTypeOptions(MemberFormat = MemberFormat.CamelCase)]
partial struct S
{
    public int One { get; set; }
    public int TwoWord { get; set; }
}";
            return VerifyGeneratedCode(src, "S.ISerialize", @"
#nullable enable
using Serde;

partial struct S : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType(""S"", 2);
        type.SerializeField(""one"", new Int32Wrap(this.One));
        type.SerializeField(""twoWord"", new Int32Wrap(this.TwoWord));
        type.End();
    }
}");
        }

        [Fact]
        public Task EnumValues()
        {
            var src = """
using Serde;
[GenerateSerialize, GenerateDeserialize]
partial struct S
{
    public ColorEnum E;
}
[GenerateSerialize, GenerateDeserialize]
[SerdeTypeOptions(MemberFormat = MemberFormat.None)]
partial struct S2
{
    public ColorEnum E;
}
enum ColorEnum
{
    Red,
    Green,
    Blue
}
""";
            return VerifyGeneratedCode(src, new[] {
                ("S.IDeserialize", """

#nullable enable
using Serde;

partial struct S : Serde.IDeserialize<S>
{
    static S Serde.IDeserialize<S>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "E"
        };
        return deserializer.DeserializeType<S, SerdeVisitor>("S", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<S>
    {
        public string ExpectedTypeName => "S";

        S Serde.IDeserializeVisitor<S>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<ColorEnum> e = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case "e":
                        e = d.GetNextValue<ColorEnum, ColorEnumWrap>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new S()
            {
                E = e.GetValueOrThrow("E"),
            };
            return newType;
        }
    }
}
"""),
                ("Serde.ColorEnumWrap", """

namespace Serde
{
    internal readonly partial record struct ColorEnumWrap(ColorEnum Value);
}
"""),
                ("Serde.ColorEnumWrap.ISerialize", """

#nullable enable
using Serde;

namespace Serde
{
    partial record struct ColorEnumWrap : Serde.ISerialize
    {
        void Serde.ISerialize.Serialize(ISerializer serializer)
        {
            var name = Value switch
            {
                ColorEnum.Red => "red",
                ColorEnum.Green => "green",
                ColorEnum.Blue => "blue",
                _ => null
            };
            serializer.SerializeEnumValue("ColorEnum", name, new Int32Wrap((int)Value));
        }
    }
}
"""),
                ("S.ISerialize", """

#nullable enable
using Serde;

partial struct S : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("S", 1);
        type.SerializeField("e", new ColorEnumWrap(this.E));
        type.End();
    }
}
"""),
                ("Serde.ColorEnumWrap.IDeserialize", """

#nullable enable
using Serde;

namespace Serde
{
    partial record struct ColorEnumWrap : Serde.IDeserialize<ColorEnum>
    {
        static ColorEnum Serde.IDeserialize<ColorEnum>.Deserialize<D>(ref D deserializer)
        {
            var visitor = new SerdeVisitor();
            return deserializer.DeserializeString<ColorEnum, SerdeVisitor>(visitor);
        }

        private sealed class SerdeVisitor : Serde.IDeserializeVisitor<ColorEnum>
        {
            public string ExpectedTypeName => "ColorEnum";

            ColorEnum Serde.IDeserializeVisitor<ColorEnum>.VisitString(string s) => s switch
            {
                "red" => ColorEnum.Red,
                "green" => ColorEnum.Green,
                "blue" => ColorEnum.Blue,
                _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + s)};
            ColorEnum Serde.IDeserializeVisitor<ColorEnum>.VisitUtf8Span(System.ReadOnlySpan<byte> s) => s switch
            {
                _ when System.MemoryExtensions.SequenceEqual(s, "red"u8) => ColorEnum.Red,
                _ when System.MemoryExtensions.SequenceEqual(s, "green"u8) => ColorEnum.Green,
                _ when System.MemoryExtensions.SequenceEqual(s, "blue"u8) => ColorEnum.Blue,
                _ => throw new InvalidDeserializeValueException("Unexpected enum field name: " + System.Text.Encoding.UTF8.GetString(s))};
        }
    }
}
"""),
                ("S2.ISerialize", """

#nullable enable
using Serde;

partial struct S2 : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("S2", 1);
        type.SerializeField("E", new ColorEnumWrap(this.E));
        type.End();
    }
}
"""),
                ("S2.IDeserialize", """

#nullable enable
using Serde;

partial struct S2 : Serde.IDeserialize<S2>
{
    static S2 Serde.IDeserialize<S2>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "E"
        };
        return deserializer.DeserializeType<S2, SerdeVisitor>("S2", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<S2>
    {
        public string ExpectedTypeName => "S2";

        S2 Serde.IDeserializeVisitor<S2>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<ColorEnum> e = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case "E":
                        e = d.GetNextValue<ColorEnum, ColorEnumWrap>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new S2()
            {
                E = e.GetValueOrThrow("E"),
            };
            return newType;
        }
    }
}
"""),
            });
        }

        [Fact]
        public Task KebabCase()
        {
            var src = @"
using Serde;

[GenerateSerde]
[SerdeTypeOptions(MemberFormat = MemberFormat.KebabCase)]
partial struct S
{
    public int One { get; set; }
    public int TwoWord { get; set; }
}";
            return VerifyGeneratedCode(src, new[] {
                ("S.ISerialize", """

#nullable enable
using Serde;

partial struct S : Serde.ISerialize
{
    void Serde.ISerialize.Serialize(ISerializer serializer)
    {
        var type = serializer.SerializeType("S", 2);
        type.SerializeField("one", new Int32Wrap(this.One));
        type.SerializeField("two-word", new Int32Wrap(this.TwoWord));
        type.End();
    }
}
"""),
                ("S.IDeserialize", """

#nullable enable
using Serde;

partial struct S : Serde.IDeserialize<S>
{
    static S Serde.IDeserialize<S>.Deserialize<D>(ref D deserializer)
    {
        var visitor = new SerdeVisitor();
        var fieldNames = new[]
        {
            "One",
            "TwoWord"
        };
        return deserializer.DeserializeType<S, SerdeVisitor>("S", fieldNames, visitor);
    }

    private sealed class SerdeVisitor : Serde.IDeserializeVisitor<S>
    {
        public string ExpectedTypeName => "S";

        S Serde.IDeserializeVisitor<S>.VisitDictionary<D>(ref D d)
        {
            Serde.Option<int> one = default;
            Serde.Option<int> twoword = default;
            while (d.TryGetNextKey<string, StringWrap>(out string? key))
            {
                switch (key)
                {
                    case "one":
                        one = d.GetNextValue<int, Int32Wrap>();
                        break;
                    case "two-word":
                        twoword = d.GetNextValue<int, Int32Wrap>();
                        break;
                    default:
                        break;
                }
            }

            var newType = new S()
            {
                One = one.GetValueOrThrow("One"),
                TwoWord = twoword.GetValueOrThrow("TwoWord"),
            };
            return newType;
        }
    }
}
""")
            });
        }
   }
}