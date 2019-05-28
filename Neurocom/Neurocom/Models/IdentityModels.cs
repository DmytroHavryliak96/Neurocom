using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
using System.Web.Mvc;
using Neurocom.DAO.Repositories;
using System.Linq;
using Neurocom.DAO.Repositories;
using Neurocom.DAO.Interfaces;

namespace Neurocom.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
         // add custom profile fields
       
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLogin { get; set; }

        public byte[] ImageData { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Kerogen> Kerogens { get; set; }
        public DbSet<NetworkType> NetworkTypes { get; set; }
        public DbSet<NeuralNetwork> NeuralNetworks { get; set; }
        public DbSet<TaskNetwork> Tasks { get; set; }
        public DbSet<TrainedNetwork> TrainedNetworks { get; set; }
        public DbSet<Layer> Layers { get; set; }
        public DbSet<TestNetwork> TestNetworks { get; set; }
        public DbSet<AvailableNetwork> AvailableNetworks { get; set; }

        public static ApplicationDbContext Create()
        {
            Database.SetInitializer<ApplicationDbContext>(new StoreDbInitializer());
            return new ApplicationDbContext();
        }


    }

    public class StoreDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        private int GetNetworkId (string name, IRepository<NeuralNetwork> dbNetworks)
        {
            return dbNetworks.Find(net => net.Name.Equals(name)).FirstOrDefault().Id;
        }

        private int GetTaskId (string taskName, IRepository<TaskNetwork> dbTasks)
        {
            return dbTasks.Find(task => task.Name.Equals(taskName)).FirstOrDefault().Id;
        }

        protected override void Seed(ApplicationDbContext db)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "admin@gmail.com";
                user.PhoneNumber = "+380695630697";
                user.Address = "Krylos village, Green Street 2";
                user.RegistrationDate = DateTime.Now;
                user.LastLogin = DateTime.Now;  

                string userPWD = "Admin123@com";

                var checkUser = UserManager.Create(user, userPWD);

                if (checkUser.Succeeded)
                {
                    var result = UserManager.AddToRole(user.Id, "Admin");
                }

                TaskNetworkRepository dbTasks = new TaskNetworkRepository(db);
                TaskNetwork taskKerogen = new TaskNetwork { Name = "Kerogen", Description = "парам", TableName = "Kerogens" };
                TaskNetwork taskLayer = new TaskNetwork { Name = "Layer", Description = "парам", TableName = "Layers" };
                dbTasks.Create(taskKerogen);

                db.SaveChanges();

                dbTasks.Create(taskLayer);

                db.SaveChanges();

                const int kerogenAmount = 19;
                const int layerAmount = 15;
                const int typesAmount = 2;
                const int networkAmount = 3;

                KerogenRepository dbKerogens = new KerogenRepository(db);
                Kerogen[] kerogens = new Kerogen[kerogenAmount] {
                    new Kerogen { Carbon = 0.765, Hydrogen = 0.1, Oxygen = 0.103, Nitrogen = 0.6, Sulfur = 0.026, Type = 1},
                    new Kerogen { Carbon = 0.759, Hydrogen = 0.091, Oxygen = 0.084, Nitrogen = 0.039, Sulfur = 0.026, Type = 1},
                    new Kerogen { Carbon = 0.809, Hydrogen = 0.086, Oxygen = 0.044, Nitrogen = 0.038, Sulfur = 0.023, Type = 1},
                    new Kerogen { Carbon = 0.726, Hydrogen = 0.079, Oxygen = 0.124, Nitrogen = 0.021, Sulfur = 0.049, Type = 2},
                    new Kerogen { Carbon = 0.854, Hydrogen = 0.079, Oxygen = 0.05, Nitrogen = 0.023, Sulfur = 0.002, Type = 2},
                    new Kerogen { Carbon = 0.806, Hydrogen = 0.059, Oxygen = 0.064, Nitrogen = 0.034, Sulfur = 0.038, Type = 2},
                    new Kerogen { Carbon = 0.727, Hydrogen = 0.06, Oxygen = 0.19, Nitrogen = 0.023, Sulfur = 0.0, Type = 3},
                    new Kerogen { Carbon = 0.833, Hydrogen = 0.046, Oxygen = 0.095, Nitrogen = 0.021, Sulfur = 0.005, Type = 3},
                    new Kerogen { Carbon = 0.916, Hydrogen = 0.032, Oxygen = 0.029, Nitrogen = 0.02, Sulfur = 0.003, Type = 3},
                    new Kerogen { Carbon = 0.827, Hydrogen = 0.041, Oxygen = 0.083, Nitrogen = 0.017, Sulfur = 0.032, Type = 1},
                    new Kerogen { Carbon = 0.854, Hydrogen = 0.035, Oxygen = 0.056, Nitrogen = 0.021, Sulfur = 0.033, Type = 2},
                    new Kerogen { Carbon = 0.686, Hydrogen = 0.051, Oxygen = 0.212, Nitrogen = 0.026, Sulfur = 0.025, Type = 3},
                    new Kerogen { Carbon = 0.775, Hydrogen = 0.108, Oxygen = 0.093, Nitrogen = 0.004, Sulfur = 0.02, Type = 1},
                    new Kerogen { Carbon = 0.735, Hydrogen = 0.083, Oxygen = 0.18, Nitrogen = 0.026, Sulfur = 0.018, Type = 2},
                    new Kerogen { Carbon = 0.883, Hydrogen = 0.05, Oxygen = 0.039, Nitrogen = 0.02, Sulfur = 0.008, Type = 3},
                    new Kerogen { Carbon = 0.759, Hydrogen = 0.094, Oxygen = 0.088, Nitrogen = 0.021, Sulfur = 0.038, Type = 1},
                    new Kerogen { Carbon = 0.693, Hydrogen = 0.083, Oxygen = 0.18, Nitrogen = 0.026, Sulfur = 0.018, Type = 2},
                    new Kerogen { Carbon = 0.913, Hydrogen = 0.032, Oxygen = 0.032, Nitrogen = 0.018, Sulfur = 0.005, Type = 3},
                    new Kerogen { Carbon = 0.822, Hydrogen = 0.099, Oxygen = 0.013, Nitrogen = 0.013, Sulfur = 0.025, Type = 1}
                };

                for (int i = 0; i < kerogenAmount; i++)
                {
                    dbKerogens.Create(kerogens[i]);
                    db.SaveChanges();
                }

                LayerRepository dbLayers = new LayerRepository(db);
                Layer[] layers = new Layer[layerAmount]
                {
                    new Layer {Porosity = 0.189, Clayness = 0.1, Carbonate = 0.86, Amplitude = 0.22, Type = 1},
                    new Layer {Porosity = 0.141, Clayness = 0.078, Carbonate = 0.123, Amplitude = 0.12, Type = 1},
                    new Layer {Porosity = 0.15, Clayness = 0.095, Carbonate = 0.128, Amplitude = 0.08, Type = 1},
                    new Layer {Porosity = 0.126, Clayness = 0.401, Carbonate = 0.085, Amplitude = 0.04, Type = 2},
                    new Layer {Porosity = 0.109, Clayness = 0.156, Carbonate = 0.179, Amplitude = 0.08, Type = 2},
                    new Layer {Porosity = 0.095, Clayness = 0.278, Carbonate = 0.124, Amplitude = 0.05, Type = 2},
                    new Layer {Porosity = 0.156, Clayness = 0.124, Carbonate = 0.09, Amplitude = 0.17, Type = 1},
                    new Layer {Porosity = 0.178, Clayness = 0.167, Carbonate = 0.075, Amplitude = 0.05, Type = 1},
                    new Layer {Porosity = 0.107, Clayness = 0.222, Carbonate = 0.119, Amplitude = 0.14, Type = 1},
                    new Layer {Porosity = 0.115, Clayness = 0.174, Carbonate = 0.182, Amplitude = 0.07, Type = 1},
                    new Layer {Porosity = 0.126, Clayness = 0.151, Carbonate = 0.144, Amplitude = 0.1, Type = 1},
                    new Layer {Porosity = 0.088, Clayness = 0.189, Carbonate = 0.25, Amplitude = 0.03, Type = 2},
                    new Layer {Porosity = 0.12, Clayness = 0.335, Carbonate = 0.086, Amplitude = 0.03, Type = 2},
                    new Layer {Porosity = 0.09, Clayness = 0.147, Carbonate = 0.197, Amplitude = 0.07, Type = 2},
                    new Layer {Porosity = 0.085, Clayness = 0.15, Carbonate = 0.224, Amplitude = 0.04, Type = 2}
                };

                for (int i = 0; i < layerAmount; i++)
                {
                    dbLayers.Create(layers[i]);
                    db.SaveChanges();
                }

                NetworkTypeRepository dbTypes = new NetworkTypeRepository(db);
                NetworkType[] types = new NetworkType[typesAmount]
                {
                    new NetworkType {Name = "Supervised leraning", Description = "парам"},
                    new NetworkType {Name = "Unsupervised leraning", Description = "парам"}
                };

                for (int i = 0; i < typesAmount; i++)
                {
                    dbTypes.Create(types[i]);
                    db.SaveChanges();
                }

                NeuralNetworkRepository dbNetworks = new NeuralNetworkRepository(db);
                NeuralNetwork[] networks = new NeuralNetwork[networkAmount]
                {
                    new NeuralNetwork {Name = "BPN", Description = "param", NetworkTypeId = 1 },
                    new NeuralNetwork {Name = "LVQ", Description = "param", NetworkTypeId = 2 },
                    new NeuralNetwork {Name = "GeneticBPN", Description = "param", NetworkTypeId = 1 }
                };

                for (int i = 0; i < networkAmount; i++)
                {
                    dbNetworks.Create(networks[i]);
                    db.SaveChanges();
                }

                AvailableNetworksRepository dbANet = new AvailableNetworksRepository(db);
                AvailableNetwork[] aNetworks = new AvailableNetwork[]
                {
                    new AvailableNetwork {NeuralNetworkId = GetNetworkId("BPN", dbNetworks), TaskId =  GetTaskId("Kerogen", dbTasks)},
                    new AvailableNetwork {NeuralNetworkId = GetNetworkId("BPN", dbNetworks), TaskId =  GetTaskId("Layer", dbTasks)},
                    new AvailableNetwork {NeuralNetworkId = GetNetworkId("LVQ", dbNetworks), TaskId =  GetTaskId("Kerogen", dbTasks)},
                    new AvailableNetwork {NeuralNetworkId = GetNetworkId("LVQ", dbNetworks), TaskId =  GetTaskId("Layer", dbTasks)},
                    new AvailableNetwork {NeuralNetworkId = GetNetworkId("GeneticBPN", dbNetworks), TaskId =  GetTaskId("Kerogen", dbTasks)},
                    new AvailableNetwork {NeuralNetworkId = GetNetworkId("GeneticBPN", dbNetworks), TaskId =  GetTaskId("Layer", dbTasks)}
                };

                for (int i = 0; i < aNetworks.Length; i++)
                {
                    dbANet.Create(aNetworks[i]);
                    db.SaveChanges();
                }

            }
            
            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }

        
            db.SaveChanges();
        }
    }
}