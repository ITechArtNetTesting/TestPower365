using System.ComponentModel;

namespace ProbeTests.Model
{
    public enum ProbeType
    {
        [Description("Mail Migration")]
        Migration = 1,
        [Description("Discovery and Matching")]
        Discovery = 2,
        [Description("Integrations")]
        Integration = 3,
        [Description("ARS A to B")]
        ArsAToB = 4,
        [Description("ARS B to A")]
        ArsBToA = 5,
        [Description("CDS On-Prem to On-Prem")]
        CDSP2P = 6,
        [Description("CDS On-Prem to Cloud")]
        CDSP2C = 7,
        [Description("Discovery Queue")]
        DiscoveryQueue = 8,
        [Description("CDS Cloud to Cloud")]
        CDSC2C = 9,
    }
}
