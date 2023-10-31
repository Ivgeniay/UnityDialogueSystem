using DialogueSystem.Characters;
using UnityEngine;

namespace DialogueSystem.Assets
{
    public class TestActor : IDialogueActor
    {
        public string Name { get; set; }
        private bool isEmptyBag { get; set; }
        [SerializeField] private int age;
        [SerializeField] private float speed;
        [SerializeField] private float distance;
        [SerializeField] private GameObject target;
        [SerializeField] private GameObject gameObject;
        [SerializeField] private GameObject targetGameObject;
        [SerializeField] private decimal gameObject4;
        public float attack;

    }
}
