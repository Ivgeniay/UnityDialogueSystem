using UnityEngine;

namespace DialogueSystem.Database.Error
{
    internal class DialogueSystemErrorData
    {
        internal Color Color { get; set; }
        internal DialogueSystemErrorData()
        {
            GenerateRandomColor();
        }

        private void GenerateRandomColor()
        {
            Color = new Color32(
                (byte)Random.Range(65, 256),
                (byte)Random.Range(50, 176),
                (byte)Random.Range(50, 176),
                255
            );
        }
    }
}
