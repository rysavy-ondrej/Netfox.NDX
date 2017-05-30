//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Collections.Generic;

namespace Ndx.Metacap
{
    /// <summary>
    /// Specifies a single entry in the Mcap file. The entry is created for the capture file.
    /// </summary>
    public class McapIndexEntry
    {
        /// <summary>
        /// Id of the capture entry.
        /// </summary>
        private Guid m_id;
        /// <summary>
        /// Info file that contains summary information about the capture file.
        /// </summary>
        private string m_infoFile;
        /// <summary>
        /// Key file name. 
        /// </summary>
        private string m_keyFile;
        /// <summary>
        /// Flow record folder name.
        /// </summary>
        private string m_flowRecordFolder;
        /// <summary>
        /// PacketBlock folder name.
        /// </summary>
        private string m_packetBlockFolder;

        /// <summary>
        /// Gets a relative path to the capture file.
        /// </summary>
        private string m_captureFile;

        public Guid Id { get => m_id; set => m_id = value; }
        public string InfoFile { get => m_infoFile; set => m_infoFile = value; }
        public string KeyFile { get => m_keyFile; set => m_keyFile = value; }
        public string FlowRecordFolder { get => m_flowRecordFolder; set => m_flowRecordFolder = value; }
        public string PacketBlockFolder { get => m_packetBlockFolder; set => m_packetBlockFolder = value; }
        public string CaptureFile { get => m_captureFile; set => m_captureFile = value; }
    }

    /// <summary>
    /// In-memory representation of "index.json" file in an MCAP file.
    /// </summary>
    internal class McapIndex
    {
        private object m_sync = new object();

        /// <summary>
        /// Lists all capture entries indexed by the <see cref="McapDataset"/>.
        /// </summary>
        private Dictionary<Guid, McapIndexEntry> m_entries;

        /// <summary>
        /// Creates an empty <see cref="McapIndex"/> object.
        /// </summary>
        public McapIndex()
        {
            m_entries = new Dictionary<Guid, McapIndexEntry>();
        }

        /// <summary>
        /// Generates a new unmanage entry. Use <see cref="Add(McapIndexEntry)"/> to 
        /// add this entry in the MCAP index.
        /// </summary>
        /// <returns></returns>
        public McapIndexEntry New()
        {
            var newentry = new McapIndexEntry() { Id = Guid.NewGuid() };
            return newentry;
        }

        /// <summary>
        /// Adds a new entry to the MCAP file index.
        /// </summary>
        /// <param name="entry"></param>
        public void Add(McapIndexEntry entry)
        {
            lock (m_sync)
            {
                m_entries.Add(entry.Id, entry);
            }
        }
    
        /// <summary>
        /// Gets all <see cref="McapIndexEntry"/> items.
        /// </summary>
        public IEnumerable<McapIndexEntry> Entries => m_entries.Values;

        /// <summary>
        /// Gets all keys of stored entries.
        /// </summary>
        public IEnumerable<Guid> Keys => m_entries.Keys;

        /// <summary>
        /// Gets the number of entries.
        /// </summary>
        public int Count => m_entries.Count;


        /// <summary>
        /// Gets the entry for the given id.
        /// </summary>
        /// <param name="index"><see cref="Guid"/> of the entry.</param>
        /// <returns><see cref="McapIndexEntry"/> object for the given id.</returns>
        public McapIndexEntry this[Guid index] => m_entries[index];
    }
}
