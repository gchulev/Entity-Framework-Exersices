﻿using System.Xml.Serialization;

namespace Footballers.Data.Models.Enums
{
    [XmlType(IncludeInSchema = true)]
    public enum PositionType
    {
        [XmlEnum("0")]
        Goalkeeper,

        [XmlEnum("1")]
        Defender,

        [XmlEnum("2")]
        Midfielder,

        [XmlEnum("3")]
        Forward
    }
}
