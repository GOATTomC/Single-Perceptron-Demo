using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingPoint {

    private const int BIAS = 1;

    private float[] m_Inputs;
    private int m_Label;

    public float[] Inputs
    {
        get { return m_Inputs; }
    }

    public int Label
    {
        get { return m_Label; }
    }

    public GameObject TrainingPointObject;

    public TrainingPoint(Vector2 position, int label)
    {
        m_Inputs = new float[3];
        m_Inputs[0] = position.x;
        m_Inputs[1] = position.y;
        m_Inputs[2] = BIAS;

        m_Label = label;
    }
}
