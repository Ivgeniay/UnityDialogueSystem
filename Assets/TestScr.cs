using DialogueSystem.Nodes;
using System;
using UnityEngine;

namespace DialogueSystem
{
    public class TestScr : MonoBehaviour
    {
        [SerializeField] private int a;
        private void Awake()
        {
            Debug.Log("0: " + GetLetterFromNumber(0));
            Debug.Log("-1: " + GetLetterFromNumber(-1));
            Debug.Log("20: " + GetLetterFromNumber(20));
            Debug.Log("25: " + GetLetterFromNumber(25));
            Debug.Log("50: " + GetLetterFromNumber(50));
            Debug.Log("500: " + GetLetterFromNumber(500));
            Debug.Log("501: " + GetLetterFromNumber(501));
            Debug.Log("5000: " + GetLetterFromNumber(5000));
            Debug.Log("50000: " + GetLetterFromNumber(50000));
            Debug.Log("500000: " + GetLetterFromNumber(500000));

        }
        public string GetLetterFromNumber(int number)
        {
            number = Math.Abs(number);

            string result = "";
            do
            {
                result = (char)('A' + (number % 26)) + result;
                number /= 26;
            } while (number-- > 0);

            return result;
        }

    }

}
