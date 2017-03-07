//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Utils
{
    using NLog;
    public static class PacketDotNetExtensions
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static PacketDotNet.Packet TryParsePacket(PacketDotNet.LinkLayers link, byte[] data)
        {

            try
            {
                return PacketDotNet.Packet.ParsePacket(link, data);
            }
            catch(Exception e)
            {
                // TODO: log exception...
                logger.Error(e, $"Cannot parser data into packet. Reason: {e.Message}.");                
                return null;
            }
        }

        public static bool MoreFragments(this PacketDotNet.IPv4Packet packet)
        {
            var mf = packet.FragmentFlags & 0x2;
            return mf != 0;
        }

        public static bool DontFragment(this PacketDotNet.IPv4Packet packet)
        {
                var mf = packet.FragmentFlags & 0x1;
                return mf != 0;
        }

    }
}
