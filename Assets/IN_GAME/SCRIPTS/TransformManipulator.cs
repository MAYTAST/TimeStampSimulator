using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TransformManipulator : MonoBehaviour
{
    [Header("Prop Info")]
    public Transform m_Transform;

    [Header("Manipulation List")]
    public DataReader m_Reader;

    [Header("TIME VALUES")]
    public float TimeStamp;
    private float simulationTime;

    [Header("UI")]
    [SerializeField] Transform textTransform;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI positionText;
    [SerializeField] TextMeshProUGUI rotationText;

    private int index = 0;
    private ManipulationData currentData;
    [SerializeField] float durationPerModification;//TIME TO CHANGE DATA FROM ONE DUARTION TO OTHER
    private bool simulationStarted;

    void Start()
    {
        durationPerModification = (TimeStamp / (m_Reader.manipulationDataList.Count));
        simulationStarted = false;
        StartCoroutine(ChangePositonAndRotation());
    }
    
    private void OnManipulationComplete()
    {
        index++;
        if (index < m_Reader.manipulationDataList.Count)
        {
            ChangePositonAndRotation();
        }
        else
        {
            simulationStarted = true;
            //displayText(Color.green, "All "+index+" tasks completed !");
        }
    }

    public void Update()
    {
        if (simulationStarted){
            simulationTime += Time.deltaTime;
            OnUpdateCallback();
        }
    }

    public IEnumerator ChangePositonAndRotation()
    {
        simulationStarted = true;

       reiterate: 
            currentData = m_Reader.manipulationDataList[index];//SAVE CURRENT MANIPULATION DATA
            m_Transform.DORotateQuaternion(currentData.targetRotation, durationPerModification).SetEase(Ease.Linear);
            m_Transform.DOMove(currentData.targetPosition, durationPerModification).SetEase(Ease.Linear);
            ++index;
            yield return new WaitForSeconds(durationPerModification);

            if (index < m_Reader.manipulationDataList.Count)
            {
             goto reiterate;
            }

        simulationStarted = false;
        yield return null;
    }

    #region UI

    public void displayText(Color color,string Text)
    {   
       text.color = color;
       text.text = Text;
       textTransform.DOScale(Vector3.one, 0.5f);
    }

    private void OnUpdateCallback()
    {
        positionText.text = "Position :" + m_Transform.localPosition.ToString();
        rotationText.text = "Rotation :" + m_Transform.localEulerAngles.ToString();
        displayText(Color.green, "Task Number : " + (index) + "\n" + simulationTime.ToString("F1"));
    }

    #endregion
}
