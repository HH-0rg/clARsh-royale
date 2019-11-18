﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CRC
{
    public class Tower : Damageable
    {
        [SerializeField]
        //private Text m_HPText;

        protected virtual void Awake()
        {
            m_CurrentHealth = m_MaxHealth;

            //m_HPText.text = m_CurrentHealth.ToString();
        }

        protected virtual void Start()
        {
            HealthChangeEvent += OnHealthChange;
        }

        protected virtual void OnDestroy()
        {
            //HealthChangeEvent -= OnHealthChange;
            //GameObject.Destroy(this);
        }

        protected override void FireDeathEvent()
        {
            this.gameObject.SetActive(false);
            GameObject.Destroy(this);
        }

        protected virtual void OnHealthChange()
        {
            //m_HPText.text = m_CurrentHealth.ToString();
            Debug.Log("Current health- " + m_CurrentHealth);
        }
    }
}
