﻿namespaceSerde.Test{partialrecordAllInOne{internal static class ColorEnumSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create<ColorEnum>(nameof(ColorEnum), new (string, System.Reflection.MemberInfo)[] {
        ("red", typeof(ColorEnum).GetField("Red")!),
("blue", typeof(ColorEnum).GetField("Blue")!),
("green", typeof(ColorEnum).GetField("Green")!)
    });
}}}