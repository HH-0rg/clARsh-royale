﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CRC
{
    public delegate void DeathEventHandler();
    public delegate void HealthChangeEventHandler();
    public interface IDamageable
    {
        event DeathEventHandler DeathEvent;
        event HealthChangeEventHandler HealthChangeEvent;

        KingTower Owner { get; }

        float MaxHealth { get; }
        float CurrentHealth { get; }

        void Hurt(float amount);
        void Heal(float amount);
    }

    public abstract class Damageable : MonoBehaviour, IDamageable
    {
        public event DeathEventHandler DeathEvent;
        public event HealthChangeEventHandler HealthChangeEvent;

        [SerializeField]
        protected KingTower m_Owner;
        public KingTower Owner { get { return m_Owner; } }

        [SerializeField]
        protected float m_MaxHealth;
        public float MaxHealth { get { return m_MaxHealth; } }

        protected float m_CurrentHealth;
        public float CurrentHealth { get { return m_CurrentHealth; } }

        public virtual void Hurt(float amount)
        {
            m_CurrentHealth -= amount;

            if (m_CurrentHealth <= .0f)
            {
                m_CurrentHealth = .0f;
                FireDeathEvent();
            }

            FireHealthChangeEvent();
        }

        public virtual void Heal(float amount)
        {
            m_CurrentHealth += amount;

            if (m_CurrentHealth > m_MaxHealth)
                m_CurrentHealth = m_MaxHealth;

            FireHealthChangeEvent();
        }

        protected virtual void FireDeathEvent()
        {
        }

        protected void FireHealthChangeEvent()
        {
            if (HealthChangeEvent != null)
                HealthChangeEvent();
        }
    }
}
