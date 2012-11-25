//using System.Xml.Serialization;

//namespace Hue.Contracts
//{
//    /// <remarks />
//    [XmlType(AnonymousType = true, Namespace = "urn:schemas-upnp-org:device-1-0")]
//    [XmlRoot(Namespace = "urn:schemas-upnp-org:device-1-0", IsNullable = false)]
//    public class root
//    {
//        /// <remarks />
//        public rootSpecVersion specVersion { get; set; }

//        /// <remarks />
//        public string URLBase { get; set; }

//        /// <remarks />
//        public rootDevice device { get; set; }
//    }

//    /// <remarks />
//    [XmlType(AnonymousType = true, Namespace = "urn:schemas-upnp-org:device-1-0")]
//    public class rootSpecVersion
//    {
//        /// <remarks />
//        public byte major { get; set; }

//        /// <remarks />
//        public byte minor { get; set; }
//    }

//    /// <remarks />
//    [XmlType(AnonymousType = true, Namespace = "urn:schemas-upnp-org:device-1-0")]
//    public class rootDevice
//    {
//        /// <remarks />
//        public string deviceType { get; set; }

//        /// <remarks />
//        public string friendlyName { get; set; }

//        /// <remarks />
//        public string manufacturer { get; set; }

//        /// <remarks />
//        public string manufacturerURL { get; set; }

//        /// <remarks />
//        public string modelDescription { get; set; }

//        /// <remarks />
//        public string modelName { get; set; }

//        /// <remarks />
//        public ulong modelNumber { get; set; }

//        /// <remarks />
//        public string modelURL { get; set; }

//        /// <remarks />
//        public uint serialNumber { get; set; }

//        /// <remarks />
//        public string UDN { get; set; }

//        /// <remarks />
//        public rootDeviceServiceList serviceList { get; set; }

//        /// <remarks />
//        public string presentationURL { get; set; }

//        /// <remarks />
//        [XmlArrayItem("icon", IsNullable = false)]
//        public rootDeviceIcon[] iconList { get; set; }
//    }

//    /// <remarks />
//    [XmlType(AnonymousType = true, Namespace = "urn:schemas-upnp-org:device-1-0")]
//    public class rootDeviceServiceList
//    {
//        /// <remarks />
//        public rootDeviceServiceListService service { get; set; }
//    }

//    /// <remarks />
//    [XmlType(AnonymousType = true, Namespace = "urn:schemas-upnp-org:device-1-0")]
//    public class rootDeviceServiceListService
//    {
//        /// <remarks />
//        public string serviceType { get; set; }

//        /// <remarks />
//        public string serviceId { get; set; }

//        /// <remarks />
//        public string controlURL { get; set; }

//        /// <remarks />
//        public string eventSubURL { get; set; }

//        /// <remarks />
//        public string SCPDURL { get; set; }
//    }

//    /// <remarks />
//    [XmlType(AnonymousType = true, Namespace = "urn:schemas-upnp-org:device-1-0")]
//    public class rootDeviceIcon
//    {
//        /// <remarks />
//        public string mimetype { get; set; }

//        /// <remarks />
//        public byte height { get; set; }

//        /// <remarks />
//        public byte width { get; set; }

//        /// <remarks />
//        public byte depth { get; set; }

//        /// <remarks />
//        public string url { get; set; }
//    }
//}