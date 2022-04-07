﻿using System.Xml.Serialization;

namespace GazeteKapiMVC5Core.SiteMap
{
    /// <summary>
    /// Yes/No enum
    /// </summary>
    public enum YesNo
    {
                /// <summary>
        /// Default value, won't be serialized.
        /// </summary>
        None,
        
        /// <summary>
        /// Yes
        /// </summary>
        [XmlEnum("yes")]
        Yes,

        /// <summary>
        /// No
        /// </summary>
        [XmlEnum("no")]
        No
    }
}