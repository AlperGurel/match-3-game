using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Match3
{
    public class Main : MonoBehaviour
    {
        public static Main Instance { get; private set; }

        #region COMPONENTS

        [Header("Root")] [SerializeField] private Transform match;
        [SerializeField] private Transform meta;

        [Header("Meta")] [SerializeField] private TextMeshProUGUI playButtonText;
        
        [Header("Board Components")] 
        [SerializeField]
        private Transform boardTransform;
        [SerializeField] private Transform boardBackground;
        [SerializeField] private GameObject generatorMaskPrefab;


        [Header("Match UI Components")] 
        [SerializeField]
        private GameObject objectivePrefab;

        [SerializeField] private Transform firstRowTransform;
        [SerializeField] private Transform secondRowTransform;
        [SerializeField] private Transform popupCanvas;
        [SerializeField] private Transform popupBackground;
        #endregion

        #region PROPERTIES

        public Transform BoardTransform => boardTransform;
        public Transform BoardBackground => boardBackground;

        public GameObject ObjectivePrefab => objectivePrefab;
        public Transform FirstRowTransform => firstRowTransform;
        public Transform SecondRowTransform => secondRowTransform;
        public Transform PopupCanvas => popupCanvas;
        public Transform PopupBackground => popupBackground;
        public Transform Match => match;
        public Transform Meta => meta;
        public GameObject GeneratorMaskPrefab => generatorMaskPrefab;
        public TextMeshProUGUI PlayButtonText => playButtonText;
        #endregion

        private void Awake()
        {
            Instance = this;
            LoadGame();
            
        }

        void Start()
        {
            
        }

        private void LoadGame()
        {
            ItemFactoryManager.CreateInstance();
            ItemFactoryManager.InitializeInstance();
            
            Player.CreateInstance();
            Player.InitializeInstance();

            MatchManager.CreateInstance();
            MatchManager.InitializeInstance();

            PopupManager.CreateInstance();
            PopupManager.InitializeInstance();
            
            
        }

        public void LoadMeta()
        {
            meta.gameObject.SetActive(true);
            match.gameObject.SetActive(false);
            MatchManager.Instance.UnloadMatch();
        }

        public void LoadMatch()
        {
            meta.gameObject.SetActive(false);
            match.gameObject.SetActive(true);
            MatchManager.Instance.InitializeBoard();
        }
    }
}
