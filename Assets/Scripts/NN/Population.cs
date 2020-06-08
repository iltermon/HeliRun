using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
	public class Population
	{
		System.Random randomize;

		NeuralNetwork[] individuals;

		float[] distanceFitness;

		public int populationSize;

		int mutationRate;

		public int generationNumber;

		public int currIndiv;

		public Population(int populationSize, int[] topology, float learningRate, int mutationRate)
		{
			this.populationSize = populationSize;
			this.mutationRate = mutationRate;
			this.generationNumber = 1;
			this.currIndiv = -1;

			randomize = new System.Random();

			distanceFitness = new float[populationSize];

			individuals = new NeuralNetwork[populationSize];
			for(int i = 0; i < individuals.Length; i++)
				individuals[i] = new NeuralNetwork(topology[0], topology[1], topology[2], learningRate);
		}

		public NeuralNetwork Next()
		{
			currIndiv++;
			if(currIndiv == populationSize)
				NewGeneration();
			
			return individuals[currIndiv];
		}

		public void SetFitnessOfCurrIndividual(float dist)
		{
			this.distanceFitness[currIndiv] = dist;
		}

		private void NewGeneration()
		{
			NeuralNetwork first, second;
			FindFirstAndSecond(out first, out second);

			NeuralNetwork child = Crossover(first, second);

			for(int i = 0; i < populationSize; i++)
			{
				individuals[i] = Mutation(child);
			}
				
			distanceFitness = new float[populationSize];

			generationNumber++;
			currIndiv = 0;
		}

		private void FindFirstAndSecond(out NeuralNetwork first, out NeuralNetwork second)
		{
			int firstIndex = 0;
			int secondIndex = 1;

			if(distanceFitness[firstIndex] < distanceFitness[secondIndex])
			{
				int temp = firstIndex;
				firstIndex = secondIndex;
				secondIndex = temp;
			}

			for(int i = 2; i < distanceFitness.Length; i++)
			{
				if(distanceFitness[i] > distanceFitness[secondIndex])
				{
					secondIndex = i;
					if(distanceFitness[secondIndex] > distanceFitness[firstIndex])
					{
						int temp = firstIndex;
						firstIndex = secondIndex;
						secondIndex = temp;
					}
				}
			}

			first = individuals[firstIndex];
			second = individuals[secondIndex];
		}

		private NeuralNetwork Mutation(NeuralNetwork child)
		{
			// Get copy of child
			NeuralNetwork mutated = new NeuralNetwork(child);

			for(int j = 0; j < mutationRate; j++)
			{
				int rows = mutated.weights[0].matrix.GetLength(0);
				int cols = mutated.weights[0].matrix.GetLength(1);

				int mutatedRow = randomize.Next(rows);
				int mutatedCol = randomize.Next(cols);

				mutated.weights[0].matrix[mutatedRow, mutatedCol] = (float)(randomize.NextDouble() * 2.0 - 1.0);

				rows = mutated.weights[1].matrix.GetLength(0);
				cols = mutated.weights[1].matrix.GetLength(1);

				mutatedRow = randomize.Next(rows);
				mutatedCol = randomize.Next(cols);

				mutated.weights[1].matrix[mutatedRow, mutatedCol] = (float)(randomize.NextDouble() * 2.0 - 1.0);
			}

			return mutated;
		}

		private NeuralNetwork Crossover(NeuralNetwork nn1, NeuralNetwork nn2)
		{
			NeuralNetwork child = new NeuralNetwork(nn1.inputN, nn1.hiddenN, nn1.outputN, nn1.learningRate);

            int crossOver = 2;

            if (crossOver == 2)
            {
                Crossover2(nn1.weights[0], nn2.weights[0], child.weights[0]);
                Crossover2(nn1.weights[1], nn2.weights[1], child.weights[1]);
                Crossover2(nn1.biases[0], nn2.biases[0], child.biases[0]);
                Crossover2(nn1.biases[1], nn2.biases[1], child.biases[1]);
            }
            else
            {
                Crossover3(nn1.weights[0], nn2.weights[0], child.weights[0]);
                Crossover3(nn1.weights[1], nn2.weights[1], child.weights[1]);
                Crossover3(nn1.biases[0], nn2.biases[0], child.biases[0]);
                Crossover3(nn1.biases[1], nn2.biases[1], child.biases[1]);
            }
			return child;
		}
        //we are crossing over the rows like "take rows from matrix1 till
        //middle and then take rest of the matrix2 (from middle)
		private void Crossover2(Matrix m1, Matrix m2, Matrix child)
		{
			int rows = m1.matrix.GetLength(0);
			int cols = m2.matrix.GetLength(1);
			int middle = rows / 2;

			for(int i = 0; i < rows; i++)
			{
				for(int j = 0; j < cols; j++)
					if(i < middle)
						child.matrix[i, j] = m1.matrix[i, j];
					else
						child.matrix[i, j] = m2.matrix[i, j];
			}
		}
        //we are crossing over every matrix in two matrises
        ///m1
        /// 1 2 3 
        /// 5 6 7
        /// 
        /// m2
        /// 4 8 9
        /// 0 3 2
        /// 
        /// 4 2 9
        /// 0 6 2


    
        private void Crossover3(Matrix m1, Matrix m2, Matrix child)
        {
            int rows = m1.matrix.GetLength(0);
            int cols = m2.matrix.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    if (i % 2  == 0 )
                        child.matrix[i, j] = m1.matrix[i, j];
                    else
                        child.matrix[i, j] = m2.matrix[i, j];
            }
        }

    }
}

