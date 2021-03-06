﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour {

    private const int MAX_ITTERATIONS = 10;

    private Trainer m_Trainer;

    private XMLWriter m_XMLWriter;
    private Text[] m_WeightTexts;

    private Perceptron m_Perceptron;
    TrainingPoint[] m_TrainingPoints;

    private Vector2 m_leftBottom, m_rightUp;
    private LineRenderer m_LineRenderer;

    private Coroutine m_ItterateRoutine;

    public float F(float x)
    {
        return x;
    }

    private void Awake()
    {
        m_LineRenderer = this.GetComponent<LineRenderer>();
        m_WeightTexts = new Text[] { GameObject.Find("Weight0").GetComponent<Text>(), GameObject.Find("Weight1").GetComponent<Text>(), GameObject.Find("Weight2").GetComponent<Text>()};
    }

    // Use this for initialization
    void Start () {
        m_leftBottom = Camera.main.ViewportToWorldPoint(new Vector3(0, 0));
        m_rightUp = Camera.main.ViewportToWorldPoint(new Vector3(1, 1));
        m_LineRenderer.SetPosition(0, new Vector2(m_leftBottom.x, F(m_leftBottom.x)));
        m_LineRenderer.SetPosition(1, new Vector2(m_rightUp.x, F(m_rightUp.x)));

        m_Perceptron = new Perceptron(3);

        m_XMLWriter = new XMLWriter(m_Perceptron);

        m_Trainer = new Trainer(m_XMLWriter, m_Perceptron, this, m_leftBottom, m_rightUp);

        //for (int point = 0; point < m_TrainingPoints.Length; point++)
        //{
        //    Vector2 position = new Vector2(Random.Range(m_leftBottom.x, m_rightUp.x), Random.Range(m_leftBottom.y, m_rightUp.y));
        //    int label = ComputeLabel(position);

        //    m_TrainingPoints[point] = new TrainingPoint(position, label);
        //    GameObject pointObject = Instantiate<GameObject>(Resources.Load<GameObject>("TrainingPoint"), position, Quaternion.identity);
        //    m_TrainingPoints[point].TrainingPointObject = pointObject;
        //}

        UpdateWeightUI();
	}

    public GameObject Spawn(Vector2 position)
    {
        return Instantiate<GameObject>(Resources.Load<GameObject>("TrainingPoint"), position, Quaternion.identity); 
    }

    public void DestroyGameObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public int ComputeLabel(Vector2 position)
    {
        if (position.y > F(position.x))
            return 1;
        else
            return -1;
    }
	
	// Update is called once per frame
	void Update () {
        HandleInput();
        //SpacePress();
	}

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            TrainPerceptron();
    }

    private void TrainPerceptron()
    {
        if (m_TrainingPoints == null)
            return;

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

            m_Perceptron.Train(m_TrainingPoints[point].Inputs, m_TrainingPoints[point].Label);

        }

        UpdateWeightUI();
    }

    public void SaveButtonClick()
    {
        m_XMLWriter.StartSaving();
    }

    public void StartPerceptronTraining()
    {
        StartFieldClearing();
        m_Trainer.StartLearnProcess();
        StartFieldClearing();
    }

    public void StartPercreptronTrainOnce()
    {
        TrainPerceptron();
    }

    public void StartPerceptron()
    {
        UsePerceptron();
    }

    private void UsePerceptron()
    {
        if (m_TrainingPoints == null)
            return;

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
        }
    }

    public void StartFieldCreation()
    {
        StartFieldClearing();
        CreateField();
    }

    private void CreateField()
    {
        m_TrainingPoints = new TrainingPoint[Random.Range(250, 350)]; //TODO REMOVE MAGIC NUMBERS

        for (int point = 0; point < m_TrainingPoints.Length; point++)
        {
            Vector2 position = new Vector2(Random.Range(m_leftBottom.x, m_rightUp.x), Random.Range(m_leftBottom.y, m_rightUp.y));
            int label = ComputeLabel(position);

            m_TrainingPoints[point] = new TrainingPoint(position, label);
            GameObject pointObject = Spawn(position);
            m_TrainingPoints[point].TrainingPointObject = pointObject;
        }
    }

    public void StartFieldClearing()
    {
        ClearField();
    }

    private void ClearField()
    {
        if (m_TrainingPoints == null)
            return;

        for (int point = 0; point < m_TrainingPoints.Length; point++)
        {
            DestroyGameObject(m_TrainingPoints[point].TrainingPointObject);
        }
    }
    
    public void StartVisualPerceptronSolve()
    {
        if (m_ItterateRoutine != null)
            StopCoroutine(m_ItterateRoutine);

        m_ItterateRoutine = StartCoroutine(SolveFieldItterate());
    }

    private IEnumerator SolveFieldItterate()
    {
        for (int i = 0; i < MAX_ITTERATIONS; i++)
        {
            StartFieldClearing();
            StartFieldCreation();
            StartPerceptron();
            yield return new WaitForSeconds(3f);

        }
    }

    public void UpdateWeightUI()
    {

        for (int weight = 0; weight < m_WeightTexts.Length; weight++)
        {
            m_WeightTexts[weight].text = "Weight " + weight.ToString() + " = " + m_Perceptron.Weights[weight].ToString();
        }
    }


}
