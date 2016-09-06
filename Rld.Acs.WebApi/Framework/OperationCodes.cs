using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Rld.Acs.WebApi.Framework
{
    public enum OperationCodes
    {
        [Description("Query Department Device")]
        QDEPTDV,
        [Description("Get Department Device By Id")]
        GDEPTDV,
        [Description("Create Department Device")]
        ADEPTDV,
        [Description("Modify Department Device")]
        MDEPTDV,
        [Description("Delete Department Device")]
        DDEPTDV,
        [Description("Query Department")]
        QDEPT,
        [Description("Get Department By Id")]
        GDEPT,
        [Description("Create Department")]
        ADEPT,
        [Description("Modify Department")]
        MDEPT,
        [Description("Delete Department")]
        DDEPT,
        [Description("Query Device Door")]
        QDVDR,

        [Description("Query Device Operation Log")]
        QDVOPLOG,
        [Description("Get Device Operation Log By Id")]
        GDVOPLOG,
        [Description("Create Device Operation Log")]
        ADVOPLOG,
        [Description("Modify Device Operation Log")]
        MDVOPLOG,
        [Description("Delete Device Operation Log")]
        DDVOPLOG,
        [Description("Query Device Role Permission")]
        QDVRLPMS,
        [Description("Get Device Role Permission By Id")]
        GDVRLPMS,
        [Description("Create Device Role Permission")]
        ADVRLPMS,
        [Description("Modify Device Role Permission")]
        MDVRLPMS,
        [Description("Delete Device Role Permission")]
        DDVRLPMS,
        [Description("Query Device Role")]
        QDVRL,
        [Description("Get Device Role By Id")]
        GDVRL,
        [Description("Create Device Role")]
        ADVRL,
        [Description("Modify Device Role")]
        MDVRL,
        [Description("Delete Device Role")]
        DDVRL,
        [Description("Query Device")]
        QDV,
        [Description("Get Device By Id")]
        GDV,
        [Description("Create Device")]
        ADV,
        [Description("Modify Device")]
        MDV,
        [Description("Delete Device")]
        DDV,
        [Description("Query Device Door State")]
        QDVDRS,
        [Description("Get Device Door State By Id")]
        GDVDRS,
        [Description("Create Device Door State")]
        ADVDRS,
        [Description("Modify Device Door State")]
        MDVDRS,
        [Description("Delete Device Door State")]
        DDVDRS,
        [Description("Query Device Traffic Log")]
        QDVTFLOG,
        [Description("Get Device Traffic Log By Id")]
        GDVTFLOG,
        [Description("Create Device Traffic Log")]
        ADVTFLOG,
        [Description("Modify Device Traffic Log")]
        MDVTFLOG,
        [Description("Delete Device Traffic Log")]
        DDVTFLOG,
        [Description("Query Floor Door")]
        QFLDR,
        [Description("Query Floor")]
        QFL,
        [Description("Get Floor By Id")]
        GFL,
        [Description("Create Floor")]
        AFL,
        [Description("Modify Floor")]
        MFL,
        [Description("Delete Floor")]
        DFL,
        [Description("Query User")]
        QUS,
        [Description("Get User By Id")]
        GUS,
        [Description("Create User")]
        AUS,
        [Description("Modify User")]
        MUS,
        [Description("Delete User")]
        DUS,
        [Description("Query User Summary")]
        QUSSMY,
        [Description("Query Time Group")]
        QTMGP,
        [Description("Get Time Group By Id")]
        GTMGP,
        [Description("Create Time Group")]
        ATMGP,
        [Description("Modify Time Group")]
        MTMGP,
        [Description("Delete Time Group")]
        DTMGP,
        [Description("Query Time Segment")]
        QTMSGM,
        [Description("Get Time Segment By Id")]
        GTMSGM,
        [Description("Create Time Segment")]
        ATMSGM,
        [Description("Modify Time Segment")]
        MTMSGM,
        [Description("Delete Time Segment")]
        DTMSGM,
        [Description("Query Time Zone")]
        QTMZN,
        [Description("Get Time Zone By Id")]
        GTMZN,
        [Description("Create Time Zone")]
        ATMZN,
        [Description("Modify Time Zone")]
        MTMZN,
        [Description("Delete Time Zone")]
        DTMZN,
        [Description("Query System Dictionary By Id")]
        QSYSDICT,
        [Description("Get System Dictionary By Id")]
        GSYSDICT,
        [Description("Create System Dictionary")]
        ASYSDICT,
        [Description("Modify System Dictionary")]
        MSYSDICT,
        [Description("Delete System Dictionary")]
        DSYSDICT,
        [Description("Query System Module Element By Id")]
        QSYSELEM,
        [Description("Get System Module Element By Id")]
        GSYSELEM,
        [Description("Create System Module Element")]
        ASYSELEM,
        [Description("Modify System Module Element")]
        MSYSELEM,
        [Description("Delete System Module Element")]
        DSYSELEM,
        [Description("Query System Module By Id")]
        QSYSMDL,
        [Description("Get System Module By Id")]
        GSYSMDL,
        [Description("Create System Module")]
        ASYSMDL,
        [Description("Modify System Module")]
        MSYSMDL,
        [Description("Delete System Module")]
        DSYSMDL,
        [Description("Query System Operation Log")]
        QSYSOPLOG,
        [Description("Get System Operation Log By Id")]
        GSYSOPLOG,
        [Description("Create System Operation Log")]
        ASYSOPLOG,
        [Description("Modify System Operation Log")]
        MSYSOPLOG,
        [Description("Delete System Operation Log")]
        DSYSOPLOG,
        [Description("Query System Operator")]
        QSYSOPT,
        [Description("Get System Operator By Id")]
        GSYSOPT,
        [Description("Create System Operator")]
        ASYSOPT,
        [Description("Modify System Operator")]
        MSYSOPT,
        [Description("Delete System Operator")]
        DSYSOPT,
        [Description("Query System Role Permission")]
        QSYSRLPMS,
        [Description("Get System Role Permission By Id")]
        GSYSRLPMS,
        [Description("Create System Role Permission")]
        ASYSRLPMS,
        [Description("Modify System Role Permission")]
        MSYSRLPMS,
        [Description("Delete System Role Permission")]
        DSYSRLPMS,
        [Description("Query System Role")]
        QSYSRL,
        [Description("Get System Role By Id")]
        GSYSRL,
        [Description("Create System Role")]
        ASYSRL,
        [Description("Modify System Role")]
        MSYSRL,
        [Description("Delete System Role")]
        DSYSRL,
        [Description("Query System Config")]
        QSYSCONF,
        [Description("Get System Config By Id")]
        GSYSCONF,
        [Description("Create System Config")]
        ASYSCONF,
        [Description("Modify System Config")]
        MSYSCONF,
        [Description("Delete System Config")]
        DSYSCONF,
    }
}