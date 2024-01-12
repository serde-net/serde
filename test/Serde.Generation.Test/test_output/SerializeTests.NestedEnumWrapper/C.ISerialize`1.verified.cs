﻿//HintName: C.ISerialize`1.cs

#nullable enable
using System;
using Serde;

partial class C : Serde.ISerialize<C>
{
    void ISerialize<C>.Serialize(C value, ISerializer serializer)
    {
        var type = serializer.SerializeType("C", 1);
        type.SerializeFieldIfNotNull<Rgb?, Serde.NullableWrap.SerializeImpl<Rgb, global::RgbWrap>>("colorOpt", value.ColorOpt);
        type.End();
    }
}