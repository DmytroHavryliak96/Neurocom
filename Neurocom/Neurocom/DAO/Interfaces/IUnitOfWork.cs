using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neurocom.Models;

namespace Neurocom.DAO.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<ApplicationUser> Users { get; }
        IRepository<Kerogen> Kerogens { get;}
        IRepository<Layer> Layers { get; }
        IRepository<NetworkType> Types { get; }
        IRepository<NeuralNetwork> NeuralNetworks { get; }
        IRepository<TaskNetwork> TaskNetworks { get; }
        IRepository<TestNetwork> TestNetworks { get; }
        IRepository<TrainedNetwork> TrainedNetworks { get;  }
        IRepository<AvailableNetwork> AvailableNetworks { get;  }
        void Save();
    }
}
