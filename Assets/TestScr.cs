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
