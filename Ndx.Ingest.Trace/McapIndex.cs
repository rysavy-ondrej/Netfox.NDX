//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Collections.Generic;

namespace Ndx.Ingest.Trace
{
    /// <summary>
    /// In-memory representation of "index.json" file in an MCAP file.
    /// </summary>
    internal class McapIndex
    {
        /// <summary>
        /// Specifies a single entry in the Mcap file.
        /// </summary>
        public class McapIndexEntry
        {
            /// <summary>
            /// Id of the capture entry.
            /// </summary>
            public Guid Id;
            /// <summary>
            /// Info file that contains summry informaiton about the capture file.
            /// </summary>
            public string InfoFile;
            /// <summary>
            /// Key file name. 
            /// </summary>
            public string KeyFile;
            /// <summary>
            /// Flow record file name.
            /// </summary>
            public string FlowRecordFolder;
            /// <summary>
            /// PacketBlock file name.
            /// </summary>
            public string PacketBlockFolder;

            /// <summary>
            /// Gets a relative path to capture file.
            /// </summary>
            public string CaptureFile;
        }

        /// <summary>
        /// Lists all capture entries indexed by the <see cref="McapDataset"/>.
        /// </summary>
        public Dictionary<Guid,McapIndexEntry> CaptureEntries { get; private set; }

        public McapIndex()
        {
            CaptureEntries = new Dictionary<Guid, McapIndexEntry>();
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

        object _sync = new object();
        public void Add(McapIndexEntry entry)
        {
            lock (_sync)
            {
                CaptureEntries.Add(entry.Id, entry);
            }
        }
    }
}
