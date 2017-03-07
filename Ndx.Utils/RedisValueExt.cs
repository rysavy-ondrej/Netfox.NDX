//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using StackExchange.Redis;
namespace Ndx.Utils
{
    public static class RedisValueExt
    {
        public static RedisValue ToRedisValue(this System.Net.IPAddress address)
        {
            return (RedisValue)address.ToString();
        }

        public static RedisValue ToRedisValue(this System.Net.NetworkInformation.PhysicalAddress address)
        {
            return (RedisValue)address.ToString();
        }
    }
}
