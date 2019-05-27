using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Neurocom.BL.Interfaces;
using Neurocom.DAO.Interfaces;
using Ninject;
using Neurocom.DAO.Repositories;
using Neurocom.BL.Services.ControllerServices;
using Neurocom.BL.NetworkLibrary;

namespace Neurocom.BL.Services.GeneticServices
{
    public class GeneticBPNLayerService : BackPropagationLayerService
    {
        private IGeneticService gaService;
        private IUnitOfWork Database;
        private IAnswerService answer;


        public GeneticBPNLayerService() : base()
        {

        }

        public GeneticBPNLayerService(string xml) : base(xml)
        {

        }

        public override void InitializeService(NetworkInitializer initializer)
        {
            gaService = new GeneticService();
            Database = new EFUnitOfWork();
            answer = new LayerAnswerService(Database);

            base.InitializeService(initializer);

            gaService.InitializeGA(initializer,
                (weights) => {

                    double fitness = 0;

                    bpn.SetWeights(weights);

                    double[][] inputs = answer.GetInputs();
                    double[][] outputs = Normalize.FormAnswersBackPropagationPlast(answer.GetAnswers());

                    for (int i = 0; i < inputs.GetUpperBound(0) + 1; i++)
                    {
                        double[] output;
                        bpn.Run(ref inputs[i], out output);

                        if (outputs[i][0] == 1)
                            fitness += 0 + output[0];

                        if (outputs[i][0] == 0)
                            fitness += 1 - output[0];
                    }

                    return fitness;
                });
        }

        public override void Train(double[][] inputs, double[][] answers)
        {
            gaService.TrainGA();

            double[] weights;
            double fitness;

            gaService.GetBestWeights(out weights, out fitness);

            bpn.SetWeights(weights);
        }

        public void Dispose()
        {
            Database.Dispose();
            answer.Dispose();
        }
    }
}