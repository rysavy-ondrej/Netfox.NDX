﻿//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//

namespace Ndx.Metacap
{
    /// <summary>
    /// Represents a category of application protocols. This list is due to Cisco NBAR.
    /// </summary>
    public enum ProtocolCategory : short
    {
        None = 0,
        Browsing = 1,
        Bussiness = 2,
        Email = 3,
        FileSharing = 4,
        Gaming,
        Industrial,
        InstantMessaging,
        InternetPrivacy,
        Tunneling,
        LocationBasedService,
        NetAdmin,
        Newsgroup,
        SocialNetworking,
        Streaming,
        Trojan,
        VoiceAndVideo,
        Other,
        Obsolete
    }

    /// <summary>
    /// Defines all known protocols. This list is due to Cisco NBAR.
    /// </summary>
    /// <remarks>
    /// This enum is based on the list specified at:
    /// http://www.cisco.com/c/en/us/products/collateral/ios-nx-os-software/network-based-application-recognition-nbar/product_bulletin_c25-627831.html
    /// </remarks>
    public enum ApplicationProtocol : int
    {
        /// <summary>
        /// Used when applciation protocol is not specified.
        /// </summary>
        NULL = 0,
        /// <summary>
        /// Used when application protocol was not recognized.
        /// </summary>
        UNKNOWN = 1,

        BROWSING = ProtocolCategory.Browsing << 16,
        FLASHMYSPACE,
        FLASHVIDEO,
        FLASHYAHOO,
        GOPHER,
        GSSHTTP,
        HTTP,
        HTTPALT,
        QQACCOUNTS,
        SHOCKWAVE,
        SPDY,
        WAPPUSH,
        WAPPUSHHTTP,
        WAPPUSHHTTPS,
        WAPPUSHSECURE,
        WAPVCAL,
        WAPVCALS,
        WAPVCARD,
        WAPVCARDS,
        WAPWSP,
        WAPWSPS,
        WAPWSPWTP,
        WAPWSPWTPS,
        YAHOOACCOUNTS,

        BUSSINESS = ProtocolCategory.Bussiness << 16,
        ACTIVESYNC,
        ADOBECONNECT,
        AFPOVERTCP,
        BACNET,
        BANYANRPC,
        BANYANVIP,
        BB,
        CAILIC,
        CHECKPOINTCPMI,
        CISCONAC,
        CITRIX,
        CLEARCASE,
        COAUTHOR,
        CORBAIIOP,
        CPQWBEM,
        CUSTIX,
        CVSPSERVER,
        CVSUP,
        CYBERCASH,
        DANTZ,
        DBASE,
        DCP,
        DDMDFM,
        DDMRDB,
        DDMSSL,
        DEOS,
        DISCLOSE,
        DISTCC,
        DNP,
        DNSIX,
        ENTRUSTSPS,
        ERPC,
        FIX,
        FLEXLM,
        GDS_DB,
        GKRELLM,
        GROOVE,
        GSSXLICEN,
        HEMS,
        HL7,
        HTTPRPCEPMAP,
        IBMDB2,
        IIOP,
        INGRESNET,
        IPCSERVER,
        IPP,
        ISOILL,
        LANSERVER,
        LEGENT1,
        LEGENT2,
        MATIPTYPEA,
        MATIPTYPEB,
        MFCOBOL,
        MITDOV,
        MORTGAGEWARE,
        MSFTGC,
        MSFTGCSSL,
        MSDYNAMICSCRMONLINE,
        MSOLAP,
        MSSQLM,
        MUMPS,
        NCP,
        NDMP,
        NET8CMAN,
        OBJCALL,
        OPALISRDV,
        ORACLEBI,
        ORACLENAMES,
        ORACLENET8CMAN,
        ORACLEEBSUITEUNSECURED,
        ORASRV,
        ORBIXCONFIG,
        ORBIXLOCATOR,
        ORBIXLOCSSL,
        PERFORCE,
        PHONEBOOK,
        POSTGRESQL,
        PRINTSRV,
        PSRSERVER,
        RADMINPORT,
        RDA,
        RDBDBSDISP,
        RESCAP,
        RISCM,
        SALESFORCE,
        SAP,
        SEMANTIX,
        SMARTPACKETS,
        SQLEXEC,
        SQLNET,
        SQLSERV,
        SQLSERVER,
        SST,
        STREETTALK,
        STX,
        SURMEAS,
        SWIFTRVF,
        SYNERGY,
        TDREPLICA,
        TDSERVICE,
        TLISRV,
        TSERVER,
        VATP,
        VEMMI,
        VPP,
        VPPSQUA,
        WEBEXAPPSHARING,
        WINDOWSAZURE,
        XACTBACKUP,
        XDTP,
        XNSAUTH,
        XNSCH,
        Z39_50,

        EMAIL = ProtocolCategory.Email << 16,
        BMPP,
        ESROEMSDP,
        EUDORASET,
        EXCHANGE,
        GBRIDGE,
        GMAIL,
        GROUPWISE,
        HOTMAIL,
        IMAP,
        IMSP,
        LOTUSNOTES,
        MAILQ,
        MAPI,
        MPP,
        MSEXCHROUTING,
        MSP,
        NIMAIL,
        NOTES,
        ODMR,
        OUTLOOKWEBSERVICE,
        POP2,
        POP3,
        QMQP,
        QMTP,
        QOTD,
        REMAILCK,
        SECUREIMAP,
        SECUREPOP3,
        SMTP,
        SUBMISSION,
        ULISTPROC,
        XNSCOURIER,
        XNSMAIL,
        YAHOOMAIL,


        FILESHARING = ProtocolCategory.FileSharing << 16,
        AIRPLAY,
        AMINET,
        BAIDUMOVIE,
        BFTP,
        BINARYOVERHTTP,
        BITTORRENT,
        CIFS,
        DIRECTCONNECT,
        DROPBOX,
        EDONKEY,
        EDONKEYSTATIC,
        ENCRYPTEDBITTORRENT,
        ENCRYPTEDEMULE,
        FASTTRACK,
        FASTTRACKSTATIC,
        FATSERV,
        FILETOPIA,
        FTP,
        FTPAGENT,
        FTPDATA,
        FTPSDATA,
        GNUTELLA,
        GOBOOGY,
        GOOGLEDOCS,
        GRIDFTP,
        GSIFTP,
        GTALKFT,
        GURUGURU,
        ITUNES,
        KAZAA2,
        KONSPIRE2B,
        MANOLITO,
        MFTP,
        MICROSOFTDS,
        MYJABBERFT,
        NAPSTER,
        NETAPPSNAPMIRROR,
        NETWAREIP,
        NETWORKINGGNUTELLA,
        NIFTP,
        NOVASTORBAKCUP,
        PANDO,
        PFTP,
        POCO,
        QFT,
        RCP,
        REMOTEFS,
        RUSHD,
        SAFT,
        SECUREFTP,
        SFTP,
        SIFTUFT,
        SONGSARI,
        SORIBADA,
        SOULSEEK,
        SUBNTBCST_TFTP,
        SVN,
        TFTP,
        TOMATOPANG,
        UUCPPATH,
        VMPWSCS,
        WASTE,
        WEBTHUNDER,
        WINMX,
        WINNY,
        XUNLEI,
        ZANNET,

        GAMING = ProtocolCategory.Gaming << 16,
        BLIZWOW,
        BNET,
        DIRECTPLAY,
        DIRECTPLAY8,
        DOOM,
        GAMESPY,
        KALI,
        MAPLESTORY,
        PARSECGAME,
        XFIRE,

        INDUSTRIAL = ProtocolCategory.Industrial << 16,

        ACRNEMA, ASAAPPLPROTO,
        CABPROTOCOL,
        DATEXASN,
        DICOM,
        EMBLNDT,
        IEEEMMS,
        IEEEMMSSSL,
        ISCSI,
        MONDEX,

        INSTANTMESSAGING = ProtocolCategory.InstantMessaging << 16,
        AOLMESSENGER,
        AOLPROTOCOL,
        CONFERENCE,
        CUSEEME,
        FRING,
        GTALK,
        GTALKCHAT,
        ICQ,
        IRC,
        IRCSERV,
        MSNMESSENGER,
        MSNP,
        NETWALL,
        NTALK,
        SECUREIRC,
        TALK,
        XMPPCLIENT,
        YAHOOMESSENGER,

        INTERNETPRIVACY = ProtocolCategory.InternetPrivacy << 16,
        CRYPTOADMIN,
        ENTRUSTASH,
        ENTRUSTKMSH,
        GTPUSER,
        IPSEC,
        ISAKMP,
        KEYSERVER,
        OPENVPN,
        PPTP,
        SDNSKMP,
        SECUREHTTP,
        SOCKS,
        SSL,
        TINC,
        TOR,
        TUNNELHTTP,
        WEBSENSE,

        TUNNELING = ProtocolCategory.Tunneling << 16,
        THREEPC,
        AN,
        ARGUS,
        ARIS,
        AX25,
        BBNRCCMON,
        BNA,
        BRSATMON,
        CBT,
        CFTP,
        CHAOS,
        COMPAQPEER,
        CPHB,
        CPNX,
        CRTP,
        DCNMEAS,
        DDP,
        DDX,
        DGP,
        EMCON,
        ENCAP,
        ETHERIP,
        FIRE,
        GGP,
        GMTP,
        HMP,
        HOPOPT,
        IATP,
        IDPR,
        IDPRCMTP,
        IDRP,
        IFMP,
        IGRP,
        IL,
        INLSP,
        IPCOMP,
        IPCV,
        IPINIP,
        IPIP,
        IPPC,
        IPV6FRAG,
        IPV6ICMP,
        IPV6INIP,
        IPV6NONXT,
        IPV6OPTS,
        IPV6ROUTE,
        IPXINIP,
        IRTP,
        ISIS,
        ISOTP4,
        LARP,
        LEAF1,
        LEAF2,
        MERITINP,
        MFENSP,
        MICP,
        MOBILE,
        MTP,
        MUX,
        NARP,
        NETBLT,
        NSFNETIGP,
        NVPII,
        PGM,
        PIM,
        PNNI,
        PRM,
        PTP,
        PUP,
        PVP,
        QNX,
        RDP,
        RVD,
        SATEXPAK,
        SATMON,
        SCCSP,
        SCHEDULETRANSFER,
        SCPS,
        SDRP,
        SECUREVMTP,
        SKIP,
        SM,
        SMP,
        SNP,
        SPRITERPC,
        SRP,
        ST,
        SUNND,
        SWIPE,
        TCF,
        TEXAR,
        TLSP,
        TPPLUSPLUS,
        TRUNK1,
        TRUNK2,
        TTP,
        UTI,
        VISA,
        VMTP,
        VRRP,
        WBEXPAK,
        WBMON,
        WSN,
        XNET,
        XNSIDP,
        XTP,

        LOCATIONBASEDSERVICE = ProtocolCategory.LocationBasedService << 16,
        GOOGLEEARTH,
        NETADMIN = ProtocolCategory.NetAdmin << 16,
        A914CG,
        A9PFS,
        ACAP,
        AGENTX,
        ALPES,
        AODV,
        APERTUSLDP,
        APPLEREMOTEDESKTOP,
        ARUBAPAPI,
        ASIPREGISTRY,
        ASIPWEBADMIN,
        AT3,
        AT5,
        AT7,
        AT8,
        ATECHO,
        ATNBP,
        ATRTMP,
        ATZIS,
        AUDITD,
        AURP,
        AUTH,
        AYIYAIPV6TUNNELED,
        BGMP,
        BGP,
        CDC,
        CHARGEN,
        CHSHELL,
        CISCOFNA,
        CISCOSYS,
        CISCOTDP,
        CISCOTNA,
        CLOANTONET1,
        CMIPAGENT,
        CMIPMAN,
        CODAAUTH2,
        COMPRESSNET,
        CRS,
        CSNETNS,
        CTF,
        CVC_HOSTD,
        CYCLESERV,
        CYCLESERV2,
        DAMEWAREMRC,
        DAYTIME,
        DECAUTH,
        DECVMSSYSMGT,
        DHCP,
        DHCPFAILOVER,
        DHCPFAILOVER2,
        DHCPV6CLIENT,
        DHCPV6SERVER,
        DHT,
        DISCARD,
        DNS,
        DTK,
        DWR,
        ECHO,
        EGP,
        EIGRP,
        ELCSD,
        ENTRUSTAAAS,
        ENTRUSTAAMS,
        EXEC,
        FCP,
        FINGER,
        FLNSPX,
        GACP,
        GDOMAP,
        GOLOGIN,
        GOTOMYPC,
        GRE,
        HACLUSTER,
        HASSLE,
        HDAP,
        HELLOPORT,
        HMMPIND,
        HMMPOP,
        HOSTNAME,
        HPALARMMGR,
        HPCOLLECTOR,
        HPMANAGEDNODE,
        HTTPMGMT,
        ICMP,
        ISATAPIPV6TUNNELED,
        ISOIP,
        ISOTP0,
        KERBEROS,
        KLOGIN,
        KPASSWD,
        KSHELL,
        L2TP,
        LDAP,
        LDP,
        LOCKD,
        LOGIN,
        LOGMEIN,
        MACSRVRADMIN,
        MASQDIALER,
        MICROMUSELM,
        MIKOGO,
        MOBILEIPAGENT,
        MONITOR,
        MRM,
        MSDP,
        MSWBT,
        MSWINDNS,
        MYLEXMAPD,
        MYSQL,
        NAME,
        NAMP,
        NAS,
        NDSAUTH,
        NESTPROTOCOL,
        NETASSISTANT,
        NETBIOS,
        NETRJS1,
        NETRJS2,
        NETRJS3,
        NETRJS4,
        NETVIEWDM1,
        NETVIEWDM2,
        NETVIEWDM3,
        NETVMGTRACEROUTE,
        NEWRWHO,
        NEXTSTEP,
        NFS,
        NICNAME,
        NLOGIN,
        NMAP,
        NOVADIGM,
        NPMPGUI,
        NPMPLOCAL,
        NPMPTRAP,
        NQS,
        NSRMP,
        NSSROUTING,
        NTP,
        NXEDIT,
        OLSR,
        OPALISROBOT,
        OPENVMSSYSIPC,
        OSPF,
        OSUNMS,
        PASSGO,
        PASSGOTIVOLI,
        PASSWORDCHG,
        PCANYWHERE,
        PHOTURIS,
        PIMRPDISC,
        PING,
        PKIX3CARA,
        PKIXTIMESTAMP,
        PROSPERO,
        PTPEVENT,
        PURENOISE,
        PWDGEN,
        QBIKGDP,
        QUOTAD,
        RADIUS,
        RCMD,
        RIP,
        RIPNG,
        RJE,
        RLP,
        RLZDBASE,
        RMONITOR,
        RMT,
        RPC2PORTMAP,
        RRH,
        RRP,
        RSHSPX,
        RSVP,
        RSVP_TUNNEL,
        RSYNC,
        RTELNET,
        SCODTMGR,
        SCOHELP,
        SCOINETMGR,
        SCOSYSMGR,
        SCOWEBSRVRMG3,
        SCOWEBSRVRMGR,
        SECURELDAP,
        SECURETELNET,
        SERVERIPX,
        SFLOW,
        SGMP,
        SGMPTRAPS,
        SHELL,
        SHOWMYPC,
        SHRINKWRAP,
        SIXTOFOURIPV6TUNNELED,
        SMSP,
        SNARE,
        SNMP,
        SNTPHEARTBEAT,
        SOFTROSMESSENGERFT,
        SQLSRV,
        SRC,
        SRMP,
        SSH,
        SSHELL,
        STATSRV,
        STUNNAT,
        STUNS,
        SUNDR,
        SUNRPC,
        SUPDUP,
        SVRLOC,
        SYNOPTICSTRAP,
        SYNOTICSBROKER,
        SYNOTICSRELAY,
        SYSLOG,
        SYSTAT,
        TACACS,
        TELL,
        TELNET,
        TEREDOIPV6TUNNELED,
        TIMBUKTU,
        TIME,
        TIMED,
        ULP,
        URM,
        UTIME,
        UUCP,
        UUCPRLOGIN,
        UUIDGEN,
        VNC,
        VNCHTTP,
        WHOAMI,
        WHOISPLUSPLUS,
        WINDOWSUPDATE,
        XDMCP,
        XNSTIME,
        XWINDOWS,

        NEWSGROUP = ProtocolCategory.Newsgroup << 16,
        NETNEWS,
        NNSP,
        NNTP,
        SECURENNTP,

        SOCIALNETWORKING = ProtocolCategory.SocialNetworking << 16,
        BLOGGER,
        FACEBOOK,
        GOOGLEPLUS,
        LINKEDIN,
        PICASA,
        TWITTER,

        STREAMING = ProtocolCategory.Streaming << 16,
        HULU,

        TROJAN = ProtocolCategory.Trojan << 16,
        HAP,
        MECOMM,
        TRINOO,

        VOICEANDVIDEO = ProtocolCategory.VoiceAndVideo << 16,
        APPLEQTC,
        APPLEQTCSRVR,
        AUDIOOVERHTTP,
        BABELGUM,
        CISCOIPCAMERA,
        CISCOPHONE,
        COOLTALK,
        DIRECTVCATLG,
        DIRECTVSOFT,
        DIRECTVTICK,
        DIRECTVWEB,
        FACETIME,
        FRINGVIDEO,
        FRINGVOIP,
        GTALKVIDEO,
        GTALKVOIP,
        H323,
        IASD,
        IAX,
        KURO,
        LIVEMEETING,
        LIVESTATION,
        MEGAVIDEO,
        MGCP,
        MSLYNC,
        MSLYNCMEDIA,
        NETFLIX,
        NETSHOW,
        NMSP,
        PANDORA,
        PHILIPSVC,
        PPSTREAM,
        PPTV,
        QQLIVE,
        RDT,
        RHAPSODY,
        RTCP,
        RTMP,
        RTMPE,
        RTP,
        RTSP,
        RTSPS,
        SGCP,
        SHOUTCAST,
        SILC,
        SIP,
        SIPTLS,
        SKINNY,
        SKYPE,
        SLING,
        SOPCAST,
        SS7NS,
        STREAMWORK,
        TEAMSPEAK,
        TELEPRESENCECONTROL,
        TELEPRESENCEMEDIA,
        TPIP,
        VDOLIVE,
        VENTRILO,
        VIBER,
        VIDEOOVERHTTP,
        WEBEXMEDIA,
        WEBEXMEETING,
        XUNLEIKANKAN,
        YAHOOVOIPMESSENGER,
        YAHOOVOIPOVERSIP,
        YOUTUBE,
        ZATTOO,

        OTHER = ProtocolCategory.Other << 16,
        APPLEJUICE,
        ANYHOSTINTERNAL,
        AOLMESSENGERFT,
        AOLMESSENGERAUDIO,
        AOLMESSENGERVIDEO,
        AVOCENT,
        BITTORRENTNETWORKING,
        CITRIXSTATIC,
        ESIGNAL,
        GHOSTSURF,
        GOOGLEACCOUNTS,
        GOOGLESERVICES,
        HAMACHI,
        HEROIXLONGITUDE,
        ICQFILETRANSFER,
        KERBEROSADM,
        MOBILITYSRV,
        MSLIVEACCOUNTS,
        MSNMESSENGERFT,
        MSNMESSENGERVIDEO,
        MSOFFICE365,
        MSUPDATE,
        NCUBELM,
        NETOPREMOTECONTROL,
        OSCARFILETRANSFER,
        P10,
        PCOIP,
        PERSONALLINK,
        REALMEDIA,
        RSVPENCAP1,
        RSVPENCAP2,
        STEAM,
        SYBASE,
        TEAMSOUND,
        TEAMVIEWEROK,
        TESLASYSMSG,
        TRADESTATION,
        VIRTUALPLACES,
        VMWAREVIEW,
        VMWAREVMOTION,
        WARROCK,
        WCCP,
        THREECOMAMP3,
        ACAS,
        ACCESSBUILDER,
        ACCESSNETWORK,
        ACP,
        ACTIVEDIRECTORY,
        ANET,
        ANSANOTIFY,
        ANSATRADER,
        APPLIX,
        ARCISDMS,
        ARIEL1,
        ARIEL2,
        ARIEL3,
        ARNS,
        ASA,
        ASSERVERMAP,
        AUDIT,
        AURORACMGR,
        AVIAN,
        BDP,
        BGSNSI,
        BHEVENT,
        BHFHS,
        BHMDS,
        BLIDM,
        BORLANDDSJ,
        CABLEPORTAX,
        CADLOCK,
        CAPWAPCONTROL,
        CAPWAPDATA,
        CDDBPALT,
        CFDPTKT,
        CIMPLEX,
        COLLABORATOR,
        COMMERCE,
        COMSCM,
        CON,
        CONNENDP,
        CONTENTSERVER,
        CORERJD,
        COURIER,
        COVIA,
        CREATIVEPARTNR,
        CREATIVESERVER,
        CRUDP,
        CSISGWP,
        DASP,
        DATASURFSRV,
        DATASURFSRVSEC,
        DCCP,
        DCTP,
        DEC_DLM,
        DECAP,
        DECBSRV,
        DECLADEBUG,
        DEIICDA,
        DESKNETS,
        DEVICE,
        DIGITALVRC,
        DMP,
        DN6NLMAUD,
        DNACML,
        DPSI,
        DSFGW,
        DSP,
        DSP3270,
        DSR,
        DTAGSTESB,
        EMFISCNTL,
        EMFISDATA,
        ENTOMB,
        EPMAP,
        ESCPIP,
        ESROGEN,
        FC,
        FONTSERVICE,
        FUJITSUDEV,
        GENIE,
        GENRADMUX,
        GGFNCP,
        GINAD,
        GOTODEVICE,
        GRAPHICS,
        HCPWISMAR,
        HIP,
        HITACHISPC,
        HPPDLDATASTR,
        HYBRIDPOP,
        HYPERWAVEISP,
        IAFDBASE,
        IAFSERVER,
        IBMAPP,
        IBMDIRECTOR,
        IBPROTOCOL,
        ICLCNET_SVINFO,
        ICLCNETLOCATE,
        IDFP,
        INBUSINESS,
        INTECOURIER,
        INTEGRASME,
        INTRINSA,
        IPCD,
        IPDD,
        IPLT,
        IPMESSENGER,
        IS99C,
        IS99S,
        ISCSITARGET,
        ISIGL,
        ISOTSAP,
        ISOTSAPC2,
        ITMMCELLS,
        JARGON,
        KBLOCK,
        KIS,
        KRYPTOLAN,
        LAMAINT,
        LJKLOGIN,
        LOCUSCON,
        LOCUSMAP,
        LOGLOGIC,
        LWAPP,
        MAGENTALOGIC,
        MAILBOXLM,
        MAITRD,
        MANET,
        MAXDB,
        MCAFEEUPDATE,
        MCIDAS,
        MCNSSEC,
        MDCPORTMAPPER,
        MEREGISTER,
        META5,
        METAGRAM,
        METER,
        MICOMPFS,
        MITMLDEV,
        MOBILIPMN,
        MPLSINIP,
        MPM,
        MPMFLAGS,
        MPMSND,
        MSGAUTH,
        MSGICP,
        MSIIS,
        MSNETLOGON,
        MSOCSFILETRANSFER,
        MSROME,
        MSRPC,
        MSSHUTTLE,
        MSSMS,
        MSSTREAMING,
        MULTILINGHTTP,
        MULTIPLEX,
        NCED,
        NCLD,
        NETBIOSNS,
        NETGW,
        NETRCS,
        NETSCDEV,
        NETSCPROD,
        NPP,
        NS,
        NSIIOPS,
        NSWFE,
        OBEX,
        OCBINDER,
        OCS_AMU,
        OCS_CMU,
        OCSERVER,
        OHIMSRV,
        OMGINITIALREFS,
        OMHS,
        OMSERV,
        ONMUX,
        OPENPORT,
        OPSMGR,
        ORACLESQLNET,
        PIP,
        PIPE,
        PIRP,
        POVRAY,
        PRINTER,
        PROFILE,
        PTCNAMESERVICE,
        PTPGENERAL,
        PUMP,
        QRH,
        RAP,
        REALMRUSD,
        REMOTEKIS,
        REPCMD,
        REPSCMD,
        RIS,
        RMC,
        RMIACTIVATION,
        RMIREGISTRY,
        RSVD,
        RSVPE2EIGNORE,
        RTIP,
        RTMPT,
        RXE,
        SANITY,
        SCCSECURITY,
        SCOI2ODIALOG,
        SCTP,
        SCXPROXY,
        SECONDLIFE,
        SEND,
        SET,
        SFSCONFIG,
        SFSSMPNET,
        SHAREPOINT,
        SIAM,
        SITARADIR,
        SITARAMGMT,
        SITARASERVER,
        SKRONK,
        SMARTSDP,
        SMPNAMERES,
        SMSD,
        SMUX,
        SNAGAS,
        SNET,
        SNPP,
        SOFTPC,
        SONAR,
        SPMP,
        SPS,
        SRSSEND,
        SSCOPMCE,
        STMF,
        SUBMIT,
        SURF,
        TACNEWS,
        TAPEWARE,
        TCPOVERDNS,
        TEEDTAP,
        TEMPO,
        TENFOLD,
        TICF1,
        TICF2,
        TNETOS,
        TNSCML,
        TNTLFD1,
        UAAC,
        UARPS,
        UDPLITE,
        UIS,
        ULPNET,
        UNIFY,
        UTMPCD,
        UTMPSD,
        VACDSMAPP,
        VACDSMSWS,
        VID,
        VIDEOTEX,
        VMNET,
        VMWAREFDM,
        VNAS,
        VPPSVIA,
        VSINET,
        VSLMP,
        WEBSTER,
        WORLDFUSION,
        WPGS,
        XBONECTL,
        XFER,
        XVTTP,
        XYPLEXMUX,
        ZSERV,
        APCPOWERCHUTE,
        CALLOFDUTY,
        DCLINK,
        FILEMAKERANOUNCEMENT,
        WLCCP,

        OBSOLETE = ProtocolCategory.Obsolete << 16,
        THREECOMTSMUX,
        AED512,
        DIXIE,
        DLS,
        DLSMON,
        HYPERG,
        INFOSEEK,
        KNETCMP,
        PCMAILSRV,
        POWERBURST,
        SERVSTAT,
        SMAKYNET,
        SUMITTG
    }
}