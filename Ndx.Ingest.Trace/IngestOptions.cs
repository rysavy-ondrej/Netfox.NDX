//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace Ndx.Ingest.Trace
{
    public class IngestOptions
    {
        public IngestOptions()
        {            
            CollectorCapacity = -1;
            ExtractorCapacity = -1;
        }
        public int CollectorCapacity { get; set; }
        public int ExtractorCapacity { get; set; }
    }
}