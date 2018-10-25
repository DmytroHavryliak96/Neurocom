using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurocom.DAO.Interfaces;

namespace Neurocom.BL.Interfaces
{
    public interface IAnswerService
    {
        double[][] GetInputs();
        
    }
}
