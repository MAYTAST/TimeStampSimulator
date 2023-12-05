using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class ManipulationData
{
    public Vector3 targetPosition;
    public Vector3 targetRotation;
    public float timeStamp;
}

public class TransformManipulator : MonoBehaviour
{
    [Header("Prop Info")]
    public Transform m_Transform;

    [Header("Manipulation List")]
    public List<ManipulationData> manipulationList;

    [Header("UI")]
    public Transform textTransform;
    public TextMeshProUGUI text;
    public TextMeshProUGUI positionText;
    public TextMeshProUGUI rotationText;

    private int index = 0;
    void Start()
    {
        ChangePositonAndRotation();
    }

    private void OnManipulationComplete()
    {
        index++;
        if (index < manipulationList.Count)
        {
            ChangePositonAndRotation();
        }
        else
        {
           // displayText(Color.green, "All tasks completed !");
        }
    }

    public void ChangePositonAndRotation()
    {
        ManipulationData currentData = manipulationList[index];//SAVE CURRENT MANIPULATION DATA
        float duration = GetDuration();
        DOTween.Sequence()
            .Append(m_Transform.DOMove(currentData.targetPosition, duration).SetEase(Ease.Linear))
            .Join(m_Transform.DORotate(currentData.targetRotation, duration).SetEase(Ease.Linear))
            .OnComplete(() =>
            {
                OnManipulationComplete();
            })
          .OnUpdate(OnUpdateCallback);
    }

    public float GetDuration()
    {
        if (index==0)
        {
            return manipulationList[index].timeStamp;
        }
        else
        {
            return manipulationList[index].timeStamp- manipulationList[index-1].timeStamp;
        }

    }

    public void displayText(Color color,string Text)
    {   
       text.color = color;
       text.text = Text;
       textTransform.DOScale(Vector3.one, 0.5f);
    }

    private void OnUpdateCallback()
    {
        positionText.text ="Position :" + m_Transform.localPosition.ToString();
        rotationText.text="Rotation :" + m_Transform.localEulerAngles.ToString();
        displayText(Color.green,"Task Number : " +(index+1)+ "\n" + Time.time.ToString("F1"));
    }
}
