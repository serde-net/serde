﻿//HintName: CSerdeTypeInfo.cs
internal static class CSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create<C>(nameof(C), new (string, System.Reflection.MemberInfo)[] {
        ("nestedArr", typeof(C).GetField("NestedArr")!)
    });
}