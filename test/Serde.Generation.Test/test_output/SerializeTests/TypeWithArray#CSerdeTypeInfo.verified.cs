﻿//HintName: CSerdeTypeInfo.cs
internal static class CSerdeTypeInfo
{
    internal static readonly Serde.TypeInfo TypeInfo = Serde.TypeInfo.Create(new (string, System.Reflection.MemberInfo)[] {
("intArr", typeof(C).GetField("IntArr")!)
    });
}