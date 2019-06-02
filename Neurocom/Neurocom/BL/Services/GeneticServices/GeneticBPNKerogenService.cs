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
    public class GeneticBPNKerogenService : BackPropagationKerogenService
    {
        private IGeneticService gaService;
        private IUnitOfWork Database;
        private IAnswerService answer;


        public GeneticBPNKerogenService() : base()
        {
           
        }

        public GeneticBPNKerogenService(string xml) : base(xml)
        {

        }

        public override void CreateNetwork()
        {
            base.CreateNetwork();
            bpn.SetBiasToZero();
        }

        public override void InitializeService(NetworkInitializer initializer)
        {
            gaService = new GeneticService();
            Database = new EFUnitOfWork();
            answer = new KerogenAnswerService(Database);

            BPNInitializer input = (BPNInitializer)initializer;
            this.parameters = input.parameters;
            this.hidden = input.hidden;
            this.learningRate = input.learningRate;
            this.Momentum = input.Momentum;
            this.minError = input.minError;
            layerSizes = new int[3] { this.parameters, this.hidden, 3 };
            TFuncs = new TransferFunction[3] {TransferFunction.None,
                                                               TransferFunction.Sigmoid,
                                                               TransferFunction.Sigmoid};

            double[][] inputs = answer.GetInputs();
            double[][] outputs = answer.GetAnswers();

            gaService.InitializeGA(initializer, 
                (weights)=> {

                    double fitness = 0;

                    bpn.SetWeights(weights);

                    int truePositive = 0;
                    int others = 0;

                    for (int i = 0; i < inputs.GetUpperBound(0)+1; i++)
                    {
                        int cluster;
                        cluster = bpn.getClusterGenetic(inputs[i]);

                        if (outputs[i][0] == cluster)
                            truePositive += 1;
                        else
                            others += 1;
                    }

                    fitness = (truePositive) / (double)(truePositive + others);

                    return fitness;
                } );
        }

        public override void Train(double[][] inputs, double[][] answers)
        {
            gaService.TrainGA();

            double[] weights;
            double fitness;

            gaService.GetBestWeights(out weights, out fitness);

         bpn.SetWeights(weights);
        }

        public override int TestNetwork(double[] test)
        {
            int result;
            result = bpn.getClusterGenetic(test);

            // це для тестування
           /* Database = new EFUnitOfWork();
            answer = new KerogenAnswerService(Database);
            double[][] inputs = answer.GetInputs();
            double[] answers = new double[19];

            for (int i = 0; i < inputs.GetUpperBound(0) + 1; i++)
            {

                answers[i] = bpn.getClusterGenetic(inputs[i]);
            }*/

            return result;
        }

        public void Dispose()
        {
            Database.Dispose();
            answer.Dispose();
        }



    }
}