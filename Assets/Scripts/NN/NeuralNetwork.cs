using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetwork
{
        public class NeuralNetwork
        {
            public int inputN, hiddenN, outputN;
            public float learningRate;
            public Matrix input;
            public Matrix output;
            public Matrix target;
            public Matrix hidden;
            public Matrix[] weights = new Matrix[2];
        public Matrix[] biases = new Matrix[2];
            public NeuralNetwork(int inputN, int hiddenN, int outputN, float learningRate)
            {
                this.inputN = inputN;
                this.hiddenN = hiddenN;
                this.outputN = outputN;
                this.learningRate = learningRate;

                this.input = new Matrix(inputN, 1);
                this.hidden = new Matrix(hiddenN, 1);
                this.output = new Matrix(outputN, 1);

                this.weights[0] = new Matrix(hiddenN, inputN); 
                this.weights[1] = new Matrix(outputN, hiddenN); 
                this.biases[0] = new Matrix(hiddenN, 1);
                this.biases[1] = new Matrix(outputN, 1);

                this.weights[0].Randomize();
                this.weights[1].Randomize();
                this.biases[0].Randomize();
                this.biases[1].Randomize();
            }
            public NeuralNetwork(NeuralNetwork nn)
            {
                this.inputN = nn.inputN;
                this.hiddenN = nn.hiddenN;
                this.outputN = nn.outputN;
                this.learningRate = nn.learningRate;

                this.input  = nn.input; 
                this.hidden = nn.hidden;
                this.output = nn.output;
                this.weights[0] = new Matrix(nn.weights[0]);
                this.weights[1] = new Matrix(nn.weights[1]);
                this.biases[0] = new Matrix(nn.biases[0]);
                this.biases[1] = new Matrix(nn.biases[1]);
            }
            public static void Sigmoid(Matrix temp)
            {
                for (int i = 0; i < temp.row; i++)
                {
                    for (int j = 0; j < temp.column; j++)               
                        temp.matrix[i, j] = 1f / (1f + (float)Math.Exp(-temp.matrix[i, j]));
                }
            }
            public static void Tanh(Matrix temp)
            {
                for (int i = 0; i < temp.row; i++)
                {
                    for (int j = 0; j < temp.column; j++)               
                        temp.matrix[i, j] = 2f / (1f + (float)Math.Exp(-2f*temp.matrix[i, j]))-1;
                }
            }
            public static float error(Matrix output, Matrix target)
            {
                float err = 0.0f;
                Matrix temp = new Matrix(output.row, output.column);
                for (int i = 0; i < temp.row; i++)
                {
                    for (int j = 0; j < temp.column; j++)
                        temp.matrix[i, j] = 0.5f * (float)(Math.Pow((double)(target.matrix[i, j] - output.matrix[i, j]), 2.0));
                }
                for (int i = 0; i < temp.row; i++)
                {
                    err += temp.matrix[i, 0];
                }
                return err;
            }

            public static Matrix dError(Matrix output, Matrix target)
            {
                Matrix temp = Matrix.subtract(output, target);
                return temp;
            }
            public static Matrix dActivation(Matrix output)
            {
                Matrix temp = new Matrix(output.row, output.column);
                for (int i = 0; i < temp.row; i++)
                {
                    for (int j = 0; j < temp.column; j++)
                        temp.matrix[i, j] = output.matrix[i, j] * (1f - output.matrix[i, j]);
                }
                return temp;
            }
            public void FeedForward()
            {
                hidden = Matrix.mult(weights[0], input);//h
                hidden = Matrix.add(hidden, biases[0]);//neth
                Tanh(hidden);//outh
                output = Matrix.mult(weights[1], hidden);//o
                output = Matrix.add(output, biases[1]);//neto
                Tanh(output);//outo  
            }
            public void backProp()
            {
                //output layer
                Matrix out_d_error_L2 = dError(output, target);
                Matrix net_d_out_L2 = dActivation(output);
                Matrix net_d_error_L2 = Matrix.eWMult(out_d_error_L2, net_d_out_L2);
                Matrix w_d_error_L2 = Matrix.mult(net_d_error_L2, Matrix.transpose(hidden));
                Matrix out_d_error_L1 = Matrix.mult(Matrix.transpose(weights[1]), net_d_error_L2);
                weights[1] = Matrix.subtract(weights[1], Matrix.scalarMult(w_d_error_L2, learningRate));

                //hidden layer
                Matrix net_d_out_L1 = dActivation(hidden);
                Matrix net_d_error_L1 = Matrix.eWMult(out_d_error_L1, net_d_out_L1);
                Matrix w_d_error_L1 = Matrix.mult(net_d_error_L1, Matrix.transpose(input));
                weights[0] = Matrix.subtract(weights[0], Matrix.scalarMult(w_d_error_L1, learningRate));
            }
            /**
             * @param args the command line arguments
             */
        }
    }
