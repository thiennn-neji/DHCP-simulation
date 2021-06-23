using System.IO;

namespace LibraryDHCP
{
	public enum EDHCPOption
	{
		Pad = 0,                                        // [RFC2132]
		Subnet_Mask = 1,                                // [RFC2132]
		Time_Offset = 2,                                // [RFC2132]
		Router_Option = 3,                              // [RFC2132]
		Time_Server = 4,                                // [RFC2132]
		Name_Server = 5,                                // [RFC2132]
		Domain_Server = 6,                              // [RFC2132]
		Log_Server = 7,                                 // [RFC2132]
		Cookie_Server = 8,                              // [RFC2132]
		LPR_Server = 9,                                 // [RFC2132]
		Impress_Server = 10,                            // [RFC2132]
		RLP_Server = 11,                                // [RFC2132]
		Hostname = 12,                                  // [RFC2132]
		Boot_File_Size = 13,                            // [RFC2132]
		Merit_Dump_File = 14,                           // [RFC2132]
		Domain_Name = 15,                               // [RFC2132]
		Swap_Server = 16,                               // [RFC2132]
		Root_Path = 17,                                 // [RFC2132]
		Extension_File_Path = 18,                       // [RFC2132]

		// IP Layer Parameters per Host	
		IP_Forwarding_Enable = 19,                      // [RFC2132]
		NonLocal_Source_Routing_Enable = 20,            // [RFC2132]
		Policy_Filter = 21,                             // [RFC2132]
		Maximum_Datagram_Reassembly_Size = 22,          // [RFC2132]
		Default_IP_TTL = 23,                            // [RFC2132]
		Path_MTU_Aging_Timeout = 24,                    // [RFC2132]
		Path_MTU_Plateau_Table = 25,                    // [RFC2132]

		// IP Layer Parameters per Interface	
		MTU_Interface = 26,                             // [RFC2132]
		MTU_Subnet = 27,                                // [RFC2132]
		Broadcast_Address = 28,                         // [RFC2132]
		Perform_Mask_Discovery = 29,                    // [RFC2132]
		Mask_Supplier = 30,                             // [RFC2132]
		Perform_Router_Discovery = 31,                  // [RFC2132]
		Router_Solicitation_Address = 32,               // [RFC2132]
		Static_Route = 33,                              // [RFC2132]

		// Link Layer Parameters per Interface	
		Trailers_Encapsulation = 34,                    // [RFC2132]
		ARP_Cache_Timeout = 35,                         // [RFC2132]
		Ethernet_Encapsulation = 36,                    // [RFC2132]

		// TCP Parameters	
		TCP_Default_TTL = 37,                           // [RFC2132]
		TCP_Keepalive_Interval = 38,                    // [RFC2132]
		TCP_Keepalive_Garbage = 39,                     // [RFC2132]

		// Application and Service Parameters	
		Network_Information_Service_Domain = 40,        // [RFC2132]
		Network_Information_Service_Servers = 41,       // [RFC2132]
		Network_Time_Protocol_Servers = 42,             // [RFC2132]
		Vendor_Specific = 43,                           // [RFC2132]
		NETBIOS_Name_Server = 44,                       // [RFC2132]
		NETBIOS_Dist_Server = 45,                       // [RFC2132]
		NETBIOS_Node_Type = 46,                         // [RFC2132]
		NETBIOS_Scope = 47,                             // [RFC2132]
		X_Window_Font = 48,                             // [RFC2132]
		X_Window_Manager = 49,                          // [RFC2132]

		// DHCP Extensions	
		Requested_IP_Address = 50,                      //	[RFC2132]
		IP_Address_Lease_Time = 51,                     //	[RFC2132]
		Overload = 52,                                  //	[RFC2132]
		DHCP_Message_Type = 53,                         //	[RFC2132]
		DHCP_Server_Identifier = 54,                    //	[RFC2132]
		Parameter_Request_List = 55,                    //	[RFC2132]
		DHCP_Message = 56,                              //	[RFC2132]	
		DHCP_Max_Message_Size = 57,                     //	[RFC2132]
		Renewal_Time = 58,                              //	[RFC2132]
		Rebinding_Time = 59,                            //	[RFC2132]
		Vendor_class_Identifier = 60,                   //	[RFC2132]
		Client_Identifier = 61,                         //	[RFC2132]

		NetWare_IP_Domain = 62,                         //	[RFC2242]
		NetWare_IP_Option = 63,                         //	[RFC2242]

		// Application and Service Parameters (cont.)
		Network_Information_Service_Plus_Domain = 64,   // [RFC2132]
		Network_Information_Service_Plus_Servers = 65,  // [RFC2132]

		// DHCP Extensions
		TFTP_server_name = 66,                          // [RFC2132]
		Bootfile_name = 67,                             // [RFC2132]

		// Application and Service Parameters (cont.)
		Mobile_IP_Home_Agent_Addr = 68,                 // [RFC2132]
		SMTP_Server = 69,                               // [RFC2132]
		POP3_Server = 70,                               // [RFC2132]
		NNTP_Server = 71,                               // [RFC2132]
		WWW_Server = 72,                                // [RFC2132]
		Finger_Server = 73,                             // [RFC2132]
		IRC_Server = 74,                                // [RFC2132]
		StreetTalk_Server = 75,                         // [RFC2132]
		STDA_Server = 76,                               // [RFC2132]

		End = 255                                       // [RFC2132]

		/*
			77	User-Class	[RFC3004]
			78	Directory Agent	[RFC2610]
			79	Service Scope	[RFC2610]
			80	Rapid Commit	[RFC4039]
			81	Client FQDN	[RFC4702]
			82	Relay Agent Information	[RFC3046]
			83	iSNS	[RFC4174]
			84	REMOVED/Unassigned	[RFC3679]
			85	NDS Servers	[RFC2241]
			86	NDS Tree Name	[RFC2241]
			87	NDS Context	[RFC2241]
			88	BCMCS Controller Domain Name list	[RFC4280]
			89	BCMCS Controller IPv4 address option	[RFC4280]
			90	Authentication	[RFC3118]
			91	client-last-transaction-time option	[RFC4388]
			92	associated-ip option	[RFC4388]
			93	Client System	[RFC4578]
			94	Client NDI	[RFC4578]
			95	LDAP	[RFC3679]
			96	REMOVED/Unassigned	[RFC3679]
			97	UUID/GUID	[RFC4578]
			98	User-Auth	[RFC2485]
			99	GEOCONF_CIVIC	[RFC4776]
			100	PCode	[RFC4833]
			101	TCode	[RFC4833]
			102-107	REMOVED/Unassigned	[RFC3679]
			108	IPv6-Only Preferred	[RFC8925]
			109	OPTION_DHCP4O6_S46_SADDR	[RFC8539]
			110	REMOVED/Unassigned	[RFC3679]
			111	Unassigned	[RFC3679]
			112	Netinfo Address	[RFC3679]
			113	Netinfo Tag	[RFC3679]
			114	DHCP Captive-Portal	[RFC8910]
			115	REMOVED/Unassigned	[RFC3679]
			116	Auto-Config	[RFC2563]
			117	Name Service Search	[RFC2937]
			118	Subnet Selection Option	[RFC3011]
			119	Domain Search	[RFC3397]
			120	SIP Servers DHCP Option	[RFC3361]
			121	Classless Static Route Option	[RFC3442]
			122	CCC	[RFC3495]
			123	GeoConf Option	[RFC6225]
			124	V-I Vendor Class	[RFC3925]
			125	V-I Vendor-Specific Information	[RFC3925]
			126	Removed/Unassigned	[RFC3679]
			127	Removed/Unassigned	[RFC3679]
			128	PXE - undefined (vendor specific)	[RFC4578]
			128	"Etherboot signature. 6 bytes:
			E4:45:74:68:00:00"	
			128	"DOCSIS ""full security"" server IP
			address"	
			128	"TFTP Server IP address (for IP
			Phone software load)"	
			129	PXE - undefined (vendor specific)	[RFC4578]
			129	"Kernel options. Variable length
			string"	
			129	Call Server IP address	
			130	PXE - undefined (vendor specific)	[RFC4578]
			130	"Ethernet interface. Variable
			length string."	
			130	"Discrimination string (to
			identify vendor)"	
			131	PXE - undefined (vendor specific)	[RFC4578]
			131	Remote statistics server IP address	
			132	PXE - undefined (vendor specific)	[RFC4578]
			132	IEEE 802.1Q VLAN ID	
			133	PXE - undefined (vendor specific)	[RFC4578]
			133	IEEE 802.1D/p Layer 2 Priority	
			134	PXE - undefined (vendor specific)	[RFC4578]
			134	"Diffserv Code Point (DSCP) for
			VoIP signalling and media streams"	
			135	PXE - undefined (vendor specific)	[RFC4578]
			135	"HTTP Proxy for phone-specific
			applications"	
			136	OPTION_PANA_AGENT	[RFC5192]
			137	OPTION_V4_LOST	[RFC5223]
			138	OPTION_CAPWAP_AC_V4	[RFC5417]
			139	OPTION-IPv4_Address-MoS	[RFC5678]
			140	OPTION-IPv4_FQDN-MoS	[RFC5678]
			141	SIP UA Configuration Service Domains	[RFC6011]
			142	OPTION-IPv4_Address-ANDSF	[RFC6153]
			143	OPTION_V4_SZTP_REDIRECT	[RFC8572]
			144	GeoLoc	[RFC6225]
			145	FORCERENEW_NONCE_CAPABLE	[RFC6704]
			146	RDNSS Selection	[RFC6731]
			147	OPTION_V4_DOTS_RI	[RFC8973]
			148	OPTION_V4_DOTS_ADDRESS	[RFC8973]
			149	Unassigned	[RFC3942]
			150	TFTP server address	[RFC5859]
			150	Etherboot	
			150	GRUB configuration path name	
			151	status-code	[RFC6926]
			152	base-time	[RFC6926]
			153	start-time-of-state	[RFC6926]
			154	query-start-time	[RFC6926]
			155	query-end-time	[RFC6926]
			156	dhcp-state	[RFC6926]
			157	data-source	[RFC6926]
			158	OPTION_V4_PCP_SERVER	[RFC7291]
			159	OPTION_V4_PORTPARAMS	[RFC7618]
			160	Unassigned	[RFC7710][RFC8910]
			161	OPTION_MUD_URL_V4	[RFC8520]
			162-174	Unassigned	[RFC3942]
			175	"Etherboot (Tentatively Assigned -
			2005-06-23)"	
			176	"IP Telephone (Tentatively Assigned -
			2005-06-23)"	
			177	"Etherboot (Tentatively Assigned -
			2005-06-23)"	
			177	"PacketCable and CableHome (replaced by
			122)"	
			178-207	Unassigned	[RFC3942]
			208	PXELINUX Magic	[RFC5071][Deprecated]
			209	Configuration File	[RFC5071]
			210	Path Prefix	[RFC5071]
			211	Reboot Time	[RFC5071]
			212	OPTION_6RD	[RFC5969]
			213	OPTION_V4_ACCESS_DOMAIN	[RFC5986]
			214-219	Unassigned	
			220	Subnet Allocation Option	[RFC6656]
			221	Virtual Subnet Selection (VSS) Option	[RFC6607]
			222-223	Unassigned	[RFC3942]
			224-254	Reserved (Private Use)
		*/
	}
	public interface IDHCPOption
	{

		// Dùng để xác định kiểu string, dành cho các DHCPOtion như Message, FileName, ServerName 
		bool NullTerminatedStrings { get; set; }
		EDHCPOption OptionType { get; }

		// Tạo đối tượng DHCPOption từ MemStream
		IDHCPOption FromStream(Stream stream);

		// Ghi đối tượng DHCPOption vào MemStream, chuẩn bị chuyển sang ByteArray
		void ToStream(Stream stream);
	}
}
