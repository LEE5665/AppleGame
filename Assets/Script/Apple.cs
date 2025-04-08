using TMPro;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public int number;
    public GameObject numberText;

    public void SetNumber(int number) {
        numberText.GetComponent<TextMeshPro>().text = $"{number}";
        this.number = number;
    }
}