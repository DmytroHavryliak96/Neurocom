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
            layerSizes = new int[3] { this.parameters, this.hidden, 1 };
            TFuncs = new TransferFunction[3] {TransferFunction.None,
                                                               TransferFunction.Sigmoid,
                                                               TransferFunction.Sigmoid};

            gaService.InitializeGA(initializer, 
                (weights)=> {

                    double fitness = 0;

                    bpn.SetWeights(weights);

                    double[][] inputs = answer.GetInputs();
                    double[][] outputs = Normalize.FormAnswersGeneticBPNKerogen(answer.GetAnswers());

                    for(int i = 0; i < inputs.GetUpperBound(0)+1; i++)
                    {
                        double[] output;
                        bpn.Run(ref inputs[i], out output);

                        if (outputs[i][0] == 0) 
                            fitness += 1.0 - output[0];

                        if (outputs[i][0] == 0.5)
                            fitness += 1.0 - Math.Abs(output[0]-0.5);

                        if (outputs[i][0] == 1)
                            fitness += output[0];
                    }

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
            double[] output = new double[1];
            result = bpn.getClusterGenetic(test, output);
            return result;
        }

        public void Dispose()
        {
            Database.Dispose();
            answer.Dispose();
        }


    }
}