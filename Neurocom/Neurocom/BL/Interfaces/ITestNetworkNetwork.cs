﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neurocom.BL.Interfaces
{
    public interface ITestNetwork
    {
        int TestNetworkFromDataBase(int trainedNetworkId, double[] testVector);

        void Dispose();
    }
}
