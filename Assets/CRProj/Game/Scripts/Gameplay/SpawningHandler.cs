using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

namespace CRC
{
    public class SpawningHandler : Singleton<SpawningHandler>
    {
        [SerializeField]
        private KingTower m_KingTowerOne, m_KingTowerTwo;
        public KingTower KingTowerOne { get { return m_KingTowerOne; } }
        public KingTower KingTowerTwo { get { return m_KingTowerTwo; } }

        [SerializeField]
        private Unit m_UnitPrefab;

        [SerializeField]
        private LayerMask m_SpawnableLayer;

        [SerializeField]
        private Transform m_Container;

        public Camera m_firstPersonCamera;

        public ScoreboardController scoreboard;

        public TextMeshProUGUI tmp;

        private int elixir = 10;

        private float tempTime;

        private float elixirGenTime = 1;

        public void lol_Update()
        {
            //RaycastHit hitInfo;
            // SPAWN SAHI AREA MEIN HI KARNA WARNA MAROGE
            //bool hitSuccess = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, m_SpawnableLayer);
            
            Ray raycast = m_firstPersonCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hitInfo;
            if (Physics.Raycast(raycast, out hitInfo))
            {
                KingTower KingTower;
                float dist1 = Vector3.Distance(hitInfo.transform.position, KingTowerOne.gameObject.transform.position);
                float dist2 = Vector3.Distance(hitInfo.transform.position, KingTowerTwo.gameObject.transform.position);
                if (dist1 > dist2)
                    KingTower = KingTowerTwo;
                else
                    KingTower = KingTowerTwo;

                m_UnitPrefab.transform.localScale = new Vector3(0.1f,0.1f,0.1f) / 2;
                Unit unit = Instantiate
                (
                    m_UnitPrefab.gameObject,
                    hitInfo.point + Vector3.up * m_UnitPrefab.transform.lossyScale.y,
                    Quaternion.identity
                )
                .GetComponent<Unit>();

                

                if (unit == null)
                    return;

                UpdateElixir(-unit.elixirCost);
                unit.transform.SetParent(m_Container, true);
                unit.Initialize(KingTower);
                //NetworkServer.Spawn(unit.gameObject);
            }
        }

        public bool UpdateElixir(int deltaElixir)
        {
            if (elixir + deltaElixir > 10 || elixir + deltaElixir < 0)
            {
                return false;
            }
            elixir += deltaElixir;
            tmp.SetText(elixir.ToString());
            
            return true;
        }

        void Update()
        {
            if(elixir<10)
            {
                tempTime += Time.deltaTime;
                if(tempTime >= elixirGenTime)
                {
                    UpdateElixir(1);
                    tempTime -= elixirGenTime;
                }
            }
        }
        private void Start()
        {
            UpdateElixir(-5);
        }
    }
}
