//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Utils
{
    public class IPAddressComparer : IComparer<IPAddress>
    {
        public int Compare(IPAddress x, IPAddress y)
        {
            int returnVal = 0;
            if (x.AddressFamily == y.AddressFamily)
            {
                byte[] b1 = x.GetAddressBytes();
                byte[] b2 = y.GetAddressBytes();

                for (int i = 0; i < b1.Length; i++)
                {
                    if (b1[i] < b2[i])
                    {
                        returnVal = -1;
                        break;
                    }
                    else if (b1[i] > b2[i])
                    {
                        returnVal = 1;
                        break;
                    }
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(x), "Cannot compare two addresses because they are in different Address Family.");
            }

            return returnVal;
        }

    }
}
