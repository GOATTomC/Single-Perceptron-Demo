using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perceptron {

    private float[] m_Weights;
    private float m_LearningRate = 0.01f;

    public float[] Weights
    {
        get { return m_Weights; }
    }

    public void SetWeight(int index, float value)
    {
        if (index >= m_Weights.Length)
            return;

        m_Weights[index] = value;
    }

    public Perceptron(int amountOfWeights)
    {
        m_Weights = new float[amountOfWeights];
        for (int weight = 0; weight < m_Weights.Length; weight++)
        {
            m_Weights[weight] = Random.Range(-1, 1);
        }
    }

    public int FeedForward(float[] inputs)
    {
        return Predict(CalculateNetInput(inputs));
    }

    private float CalculateNetInput(float[] inputs)
    {
        float sum = 0;

        for (int i = 0; i < m_Weights.Length; i++)
        {
            sum += inputs[i] * m_Weights[i];
        }

        return sum;
    }

    private int Predict(float netInput)
    {
        if (netInput > 0)
            return 1;
        else
            return -1;
    }

    public void Train(float[] inputs, float desired)
    {
        int guess = GetGuess(inputs);
        float error = ComputeError(desired, guess);

        if (error == 0)
            return;

        for (int weight = 0; weight < m_Weights.Length; weight++)
        {
            m_Weights[weight] += m_LearningRate * error * inputs[weight];
        }

    }

    private int GetGuess(float[] inputs)
    {
        return FeedForward(inputs);
    }

    private float ComputeError(float desired, float guess)
    {
        return desired - guess;
    }
}
