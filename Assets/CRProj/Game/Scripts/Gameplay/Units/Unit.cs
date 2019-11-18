using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Networking;

namespace CRC
{
    [CreateAssetMenu(fileName = "New Unit Definition")]
    public class UnitDefinition : ScriptableObject
    {

        [SerializeField]
        private string m_UnitName;
        public string Name { get { return m_UnitName; } }

        [SerializeField]
        private float m_MovementSpeed;
        public float MovementSpeed { get { return m_MovementSpeed; } }

        [SerializeField]
        private float m_Damage;
        public float Damage { get { return m_Damage; } }

        [SerializeField]
        private float m_DamageInterval;
        public float DamageInterval { get { return m_DamageInterval; } }
    }

    public class Unit : Damageable
    {
        public float stopRange;
        
        public enum UnitState
        {
            Idle = 0,
            Moving,
            Attacking
        }

        [SerializeField]
        private UnitDefinition m_Definition;
        public UnitDefinition Definition { get { return m_Definition; } }

        [SerializeField]
        private Renderer m_Renderer;

        //[SerializeField]
        //private NavMeshAgent m_Agent;

        [Header("Healthbar")]

        [SerializeField]
        private GameObject m_HealthPanelPrefab;

        [SerializeField]
        private float m_HPBarYOffPx = 50.0f;

        private Image m_HPBarForeground;

        private bool m_Enabled;

        private UnitState m_State;

        public float attackRate;
        private float tempTime;

        public float damage;

        public int elixirCost = -3;

        private Damageable nearest = null;
        private int nearestI;

        public bool isRanged;
        public GameObject projectile;
        public float projectileSpeed;

        void Awake()
        {
            m_State = UnitState.Idle;
        }

        public void RangedAttack()
        {
            GameObject proj = Instantiate(projectile, this.transform.position + this.transform.forward * 0.1f, Quaternion.identity);
            proj.GetComponent<ProjectileController>().Fire(nearest, projectileSpeed, damage);
            NetworkServer.Spawn(proj.gameObject);
        }
        public void MeleeAttack()
        {
            nearest.Hurt(damage);
        }

        public void Initialize(KingTower owner)
        {
            m_Owner = owner;

            //m_Renderer.material.color = m_Owner.Definition.Color;
            //m_Renderer.material.color = Color.blue;

            m_State = UnitState.Idle;
            m_CurrentHealth = m_MaxHealth;

            /*GameObject healthPanel = Instantiate
            (
                m_HealthPanelPrefab,
                Camera.main.WorldToScreenPoint(this.transform.position) + Vector3.up * m_HPBarYOffPx,
                Quaternion.identity
                //GameObject.Find("HUD").transform
            );

            healthPanel.GetComponent<UnitHealthPanel>().Initialize(this, m_HPBarYOffPx);
            
            m_HPBarForeground = healthPanel.transform.GetChild(1).GetComponent<Image>();
            //m_HPBarForeground.color = m_Owner.Definition.Color;
            //m_HPBarForeground.color = Color.blue;
            
            HealthChangeEvent += OnHealthChange;
            */
            m_Enabled = true;

            //GetComponent<Rigidbody>().velocity = new Vector3(0, 1, 0);
        }

        void Update()
        {
            if (!m_Enabled)
                return;

            if(m_State == UnitState.Attacking)
            {
                tempTime += Time.deltaTime;
                if (tempTime > attackRate)
                {
                    tempTime = 0;
                    if (isRanged)
                    {
                        RangedAttack();
                    }

                    else
                    {
                        MeleeAttack();
                    }
                }
                if (nearest == null || nearest.Equals(null))
                {
                    m_State = UnitState.Moving;
                    Damageable[] damageables = FindObjectsOfType<Damageable>();
                }
            }

            StartCoroutine(Walk());
        }

        void OnDestroy()
        {
            HealthChangeEvent -= OnHealthChange;
        }

        private void OnHealthChange()
        {
            m_HPBarForeground.fillAmount = m_CurrentHealth / m_MaxHealth;
        }

        private IEnumerator Walk()
        {
            yield return new WaitForSeconds(0.5f);


            float range = Mathf.Infinity;

            Damageable[] damageables = FindObjectsOfType<Damageable>();
            for (int i = 0; i < damageables.Length; i++)
            {
                Damageable d = damageables[i];

                if (d.Owner != m_Owner)
                {
                    float dist = Vector3.Distance(this.transform.position, d.transform.position);

                    if (dist < range)
                    {
                        nearest = d;
                        range = dist;
                        nearestI = i;
                    }
                }
            }

            

            if (nearest != null)
            {
                if (range > stopRange)
                {
                    //m_Agent.SetDestination(nearest.transform.position);
                    this.GetComponent<NavMeshAgent>().SetDestination(nearest.transform.position);
                    this.GetComponent<NavMeshAgent>().isStopped = false;
                    Debug.Log(nearest.transform.position);
                    m_State = UnitState.Moving;
                }
                else
                {
                    this.GetComponent<NavMeshAgent>().isStopped = true;
                    m_State = UnitState.Attacking;
                }
            }
        }
    }
}
