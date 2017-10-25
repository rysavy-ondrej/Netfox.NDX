using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;

namespace Ndx.Decoders
{
    
    public class SSH
    {
        public const string Protocol = "ssh.protocol";
        public const string MessageCode = "ssh.message_code";
        public const string EncryptedPacket = "ssh.encrypted_packet";
        public const string PacketLength = "ssh.packet_length";        
        public const string CompressionAlgorithmsClientToServer = "ssh.compression_algorithms_client_to_server";
        public const string CompressionAlgorithmsServerToClient = "ssh.compression_algorithms_server_to_client";
        public const string EncryptionAlgorithmsClientToServer = "ssh.encryption_algorithms_client_to_server";
        public const string EncryptionAlgorithmsSeeverToClient = "ssh.encryption_algorithms_server_to_client";
        public const string MacAlgorithmsClientToServer = "ssh.mac_algorithms_client_to_server";
        public const string MacAlgorithmsServerToClient = "ssh.mac_algorithms_server_to_client";

        /// <summary>
        /// Specifies which fields to extract from SSH packets to produce <see cref="SshEvent"/> and <see cref="SshConversation"/> objects.
        /// See https://www.wireshark.org/docs/dfref/s/ssh.html for complete specification of SSH fields.
        /// </summary>
        public static string[] Fields = new[] {
            SSH.Protocol,
            SSH.MessageCode,
            SSH.EncryptedPacket,
            SSH.PacketLength,
            TCP.Length,
            SSH.CompressionAlgorithmsClientToServer,
            SSH.CompressionAlgorithmsServerToClient,
            SSH.EncryptionAlgorithmsClientToServer,
            SSH.EncryptionAlgorithmsSeeverToClient,
            SSH.MacAlgorithmsClientToServer,
            SSH.MacAlgorithmsServerToClient            
        };


        public static SshEvent Event(DecodedFrame packet)
        {
            
            return new SshEvent()
            {
                Code = (SshMessageCode)Int32.Parse(packet.GetFieldValue(SSH.MessageCode, "0")),
                //TODO: ???
            };
            
        }
    }
}
