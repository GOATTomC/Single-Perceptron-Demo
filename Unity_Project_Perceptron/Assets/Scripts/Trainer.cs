using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer {

    private const int MAX_FLAWLESS_ITTERATIONS = 20;
    private int m_FlawlessItterations = 0;

    private XMLWriter m_XMLWriter;
    private Perceptron m_Perceptron;
    private MainManager m_MainManager;
    private Vector2 m_BottomLeft, m_UpperRight;

    TrainingPoint[] m_TrainingPoints;

    public Trainer(XMLWriter xmlWriter, Perceptron perceptron, MainManager mainManager, Vector2 bottomLeft, Vector2 upperRight)
    {
        m_XMLWriter = xmlWriter;
        m_Perceptron = perceptron;
        m_MainManager = mainManager;
        m_BottomLeft = bottomLeft;
        m_UpperRight = upperRight;


    }

    public void StartLearnProcess()
    {
        Itterate();
    }

    private void Itterate()
    {
        
        while(m_FlawlessItterations < MAX_FLAWLESS_ITTERATIONS)
        {
            CreateTrainField();
            UsePerceptron();
            if (m_FlawlessItterations < MAX_FLAWLESS_ITTERATIONS)
                RemoveTrainingField();
            m_MainManager.UpdateWeightUI();
        }

        m_XMLWriter.StartSaving();
    }

    private void CreateTrainField()
    {
        m_TrainingPoints = new TrainingPoint[Random.Range(250, 350)]; //TODO REMOVE MAGIC NUMBERS

        for (int point = 0; point < m_TrainingPoints.Length; point++)
        {
            Vector2 position = new Vector2(Random.Range(m_BottomLeft.x, m_UpperRight.x), Random.Range(m_BottomLeft.y, m_UpperRight.y));
            int label = m_MainManager.ComputeLabel(position);

            m_TrainingPoints[point] = new TrainingPoint(position, label);
            GameObject pointObject = m_MainManager.Spawn(position);
            m_TrainingPoints[point].TrainingPointObject = pointObject;
        }

    }

    private void UsePerceptron()
    {
        int errors = 0;

        for (int point = 0; point < m_TrainingPoints.Length; point++)
        {
            int guess = m_Perceptron.FeedForward(m_TrainingPoints[point].Inputs);

            if (guess > 0)
            {
                m_TrainingPoints[point].TrainingPointObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                m_TrainingPoints[point].TrainingPointObject.GetComponent<SpriteRenderer>().color = Color.cyan;
            }

            if (guess == m_TrainingPoints[point].Label)
                continue;

            errors++;

            m_Perceptron.Train(m_TrainingPoints[point].Inputs, m_TrainingPoints[point].Label);

        }

        if (errors == 0)
            m_FlawlessItterations++;
        else
            m_FlawlessItterations = 0;
    }
    
    private void RemoveTrainingField()
    {
        for (int point = 0; point < m_TrainingPoints.Length; point++)
        {
            m_MainManager.DestroyGameObject(m_TrainingPoints[point].TrainingPointObject);
        }
    }
}
