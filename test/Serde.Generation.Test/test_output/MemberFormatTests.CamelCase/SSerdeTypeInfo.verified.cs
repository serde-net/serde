﻿//HintName: SSerdeTypeInfo.cs
internal static class SSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("one", typeof(S).GetProperty("One")!),
("twoWord", typeof(S).GetProperty("TwoWord")!)
    });
}