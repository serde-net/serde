﻿//HintName: RgbSerdeTypeInfo.cs
internal static class RgbSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("red", typeof(Rgb).GetField("Red")!),
("blue", typeof(Rgb).GetField("Blue")!)
    });
}