//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.IO;
using System.Security.Cryptography;

namespace Ndx.Utils
{

    public enum HashAlgorithmName {
        MD5,
        SHA1,
        SHA256,
        SHA384,
        SHA512
    }

    class HashProvider
    {
        private readonly HashAlgorithm m_hashAlgorithm;
        public HashProvider(HashAlgorithm algorithm)
        {
            m_hashAlgorithm = algorithm;
        }

        public string ComputeHash(string path)
        {
            using (FileStream stream = System.IO.File.OpenRead(path))
            {                
                var checksum = m_hashAlgorithm.ComputeHash(stream);
                return BitConverter.ToString(checksum).Replace("-", String.Empty);
            }
        }

        internal static HashProvider Create(HashAlgorithmName hashAlgoName)
        {
            if (hashAlgoName == HashAlgorithmName.MD5)
            {
                return new HashProvider(new MD5Cng());
            }
            if (hashAlgoName == HashAlgorithmName.SHA1)
            {
                return new HashProvider(new SHA1Cng());
            }
            if (hashAlgoName == HashAlgorithmName.SHA256)
            {
                return new HashProvider(new SHA256Cng());
            }
            if (hashAlgoName == HashAlgorithmName.SHA384)
            {
                return new HashProvider(new SHA384Cng());
            }
            if (hashAlgoName == HashAlgorithmName.SHA512)
            {
                return new HashProvider(new SHA512Cng());
            }
            throw new ArgumentException("Supplied hash algorithm is not recognized nor implemented.");
        }
    }
}
