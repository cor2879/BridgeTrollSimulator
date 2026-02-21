using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Audio;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.StatusEffects;
using OldSchoolGames.BridgeTrollSimulator.Scripts.InputHandling;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;
using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public abstract class EntityController 
        : MonoBehaviour, IEventSource, IEncounterable
    {
        protected Animator animator;
        protected Rigidbody2D rb;
        protected IInputSource inputSource;
        protected EntityCombatUI entityCombatUI;

        #region Character Stats

        [Header("Character Stats")]
        [SerializeField]
        protected string entityName;
        [SerializeField]
        protected bool isPlayerControlled;
        [SerializeField]
        protected int gold;
        [SerializeField]
        protected int maxHealth;
        [SerializeField]
        protected int maxStamina = 10;
        [SerializeField]
        protected int attackCost = 3;
        [SerializeField]
        protected int defendRestore = 2;
        [SerializeField, ReadOnly]
        protected int currentStamina;
        [SerializeField, ReadOnly]
        protected int currentHealth;
        [SerializeField]
        protected int dexterity = 10;
        [SerializeField, ReadOnly]
        protected int initiativeRoll;
        [SerializeField]
        protected int attack = 3;
        [SerializeField]
        protected int defense = 1;
        [SerializeField, ReadOnly]
        protected int defenseModifier;
        [SerializeField, ReadOnly]
        private List<StatusEffect> activeEffects = new();
        [SerializeField, ReadOnly]
        protected bool isDefending;
        [SerializeField]
        protected float defendMultiplier = 0.5f;
        [SerializeField]
        protected float critChance = 0.1f;
        [SerializeField]
        protected float critMultiplier = 2f;
        [SerializeField]
        protected float temporaryCritBonus;
        [SerializeField]
        protected CombatFaction faction;
        [SerializeField]
        protected Ability[] abilities;

        #endregion

        #region Sound Effects

        [SerializeField]
        protected AudioClip hurtSfx;
        [SerializeField]
        protected AudioClip deathSfx;

        #endregion

        [SerializeField, ReadOnly]
        protected EntityType entityType;

        [SerializeField, ReadOnly]
        protected Canvas combatUIRoot;

        [SerializeField, ReadOnly]
        protected Guid instanceId;

        [SerializeField]
        protected ControlMode controlMode = ControlMode.FreeRoam;

        [SerializeField]
        protected string sourceName;

        [Header("Movement")]
        [SerializeField] 
        private float moveSpeed = 1.0f;

        [SerializeField]
        protected float animatorSpeed = 0.0f;

        private Vector2 movementInput;
        private bool facingRight = true;

        #region Properties

        public abstract ControlMode DefaultControlMode { get; }
        public GameObject GameObject => this.gameObject;
        public ControlMode CurrentControlMode => controlMode;
        public IInputSource InputSource => inputSource;
        public EntityCombatUI CombatUI => entityCombatUI;
        public bool IsPlayerControlled => isPlayerControlled;

        #region Stats

        public string Name { get => entityName; set => this.entityName = value;}
        public int Attack
        {
            get
            {
                var value = attack;

                foreach (var effect in activeEffects)
                {
                    value = effect.ModifyAttack(value);
                }

                return value;
            }
        }
        public int BaseDefense => defense;
        public int Defense
        {
            get
            {
                var value = BaseDefense + defenseModifier;

                foreach (var effect in activeEffects)
                {
                    value = effect.ModifyDefense(value);
                }

                return value;
            }
        }
        public int Gold => gold;
        public int MaxHealth => maxHealth;
        public int Dexterity => dexterity;
        public int InitiativeRoll => initiativeRoll;
        public float CritChance => critChance;
        public float CritMultiplier => critMultiplier;
        public int CurrentHealth 
        { 
            get => currentHealth; 
            protected set
            {
                this.currentHealth = value; 
                entityCombatUI.Refresh();
            }
        }
        public int MaxStamina => maxStamina;
        public int AttackCost => attackCost;
        public int CurrentStamina
        {
            get => currentStamina;
            protected set
            {
                this.currentStamina = value;
                entityCombatUI.Refresh();
            }
        }
        public CombatFaction Faction => faction;
        public Ability[] Abilities => abilities;
        public List<StatusEffect> ActiveEffects => activeEffects;

        #endregion

        public GameSystemType SystemType => GameSystemType.Entity;

        public string SourceName => sourceName;

        #endregion

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            combatUIRoot = GetComponentInChildren<Canvas>();
            instanceId = Guid.NewGuid();
            currentHealth = maxHealth;
            entityCombatUI = GetComponent<EntityCombatUI>();
            entityCombatUI.SetActive(false);
        }

        protected virtual void Update()
        {
            this.ProcessInput();
            this.UpdateAnimator();
        }

        protected virtual void FixedUpdate()
        {
            this.ApplyMovement();
        }

        #region Input

        protected bool HasInput => inputSource is not null;

        protected virtual void ProcessInput()
        {
            if (!HasInput)
            {
                return;
            }

            if (controlMode == ControlMode.Disabled ||
                controlMode == ControlMode.CutScene ||
                controlMode == ControlMode.Dead ||
                controlMode == ControlMode.Npc ||
                controlMode == ControlMode.Encounter)
            {
                return;
            }

            var horizontalAxis = inputSource.GetHorizontal();

            animatorSpeed = horizontalAxis != 0.0f ? 0.5f : 0.0f;

            this.movementInput = new Vector2(
                this.inputSource.GetHorizontal(),
                this.inputSource.GetVertical()
            );

            HandleFacing();
        }

        protected void HandleFacing()
        {
            if (this.movementInput.x > 0 && !this.facingRight)
            {
                this.Flip();
            }

            if (this.movementInput.x < 0 && this.facingRight)
            {
                this.Flip();
            }
        }

        public void SetInputSource(IInputSource source)
        {
            inputSource = source;
        }

        public void ClearInputSource()
        {
            inputSource = null;
        }

        public void SetControlMode(ControlMode mode)
        {
            if (CurrentControlMode == mode)
            {
                return;
            }

            controlMode = mode;

            OnControlModeChanged(mode);
        }

        public void ResetControlMode()
        {
            SetControlMode(DefaultControlMode);
        }

        protected virtual void OnControlModeChanged(ControlMode newMode)
        {
            if (combatUIRoot == null)
            {
                return;
            }

            entityCombatUI.SetActive(newMode == ControlMode.Combat);
        }

        #endregion

        #region Combat

        public void EnterCombat()
        {
            SetControlMode(ControlMode.Combat);
            CurrentStamina = MaxStamina;
            RollInitiative();
        }

        #endregion

        #region Movement

        protected virtual void ApplyMovement()
        {
            if (CurrentControlMode == ControlMode.Encounter)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            if (this.CurrentControlMode != ControlMode.FreeRoam)
            {
                rb.linearVelocity = new Vector2(
                    0f, rb.linearVelocity.y);
                return;
            }

            rb.linearVelocity = new Vector2(
                movementInput.x * moveSpeed * transform.localScale.x, 
                GetComponent<Rigidbody2D>().linearVelocity.y);
        }

        protected void Flip()
        {
            facingRight = !facingRight;

            animator.SetFloat(
                Constants.AnimatorParams.xDirection,
                !facingRight ? -1f : 1f);
        }

        #endregion

        #region Animation

        protected virtual void UpdateAnimator()
        {
            animator.SetFloat(
                Constants.AnimatorParams.Speed, 
                Mathf.Abs(animatorSpeed));
            animator.SetFloat(
                Constants.AnimatorParams.xDirection, 
                facingRight ? 1f : -1f);
        }

        protected void TriggerAction(string triggerName)
        {
            animator.SetTrigger(triggerName);
        }

        #endregion

        #region Collision Detection

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            var otherEntity = other.GetComponent<EntityController>();
            if (otherEntity == null)
            {
                return;
            }

            var thisEntity = GetComponent<EntityController>();
            if (thisEntity == null)
            {
                return;
            }

            EntityController player = null;
            EntityController npc = null;

            if (thisEntity.CompareTag("Troll"))
            {
                player = thisEntity;
            }
            else if (otherEntity.CompareTag("Troll"))
            {
                player = otherEntity;
            }

            if (thisEntity.CompareTag("NPC"))
            {
                npc = thisEntity;
            }
            else if (otherEntity.CompareTag("NPC"))
            {
                npc = otherEntity;
            }

            if (player == null || npc == null)
            {
                return;
            }

            GameEventBus.Publish(
                new EntityEncounterEvent(player, npc, Time.frameCount));
        }

        public virtual void HandleEncounter(IEncounterable other)
        {
            SetControlMode(ControlMode.Encounter);
        }

        #endregion
    
        #region Interaction

        public virtual bool CanAttack()
        {
            return true;
        }

        public virtual bool CanDefend()
        {
            return true;
        }

        public virtual bool CanExecute(Ability ability)
        {
            return CurrentStamina >= ability.StaminaCost;
        }

        public virtual int DeductGold(int amount)
        {
            var goldReturned = amount;

            if (this.gold < amount)
            {
                goldReturned = this.gold;
                this.gold = 0;
            }
            else
            {
                this.gold -= amount;
            }
            
            return goldReturned;
        }

        public virtual void AddGold(int amount)
        {
            this.gold += amount;
        }
        
        public virtual void TakeDamage(int amount)
        {
            this.CurrentHealth -= amount;
            GameEventBus.Publish(
                new DamageTakenEvent(this, amount, Time.frameCount));

            AudioSystem.Instance.PlaySFX(hurtSfx);
        }

        public virtual void SpendStamina(int amount)
        {
            if (this.CurrentStamina < amount)
            {
                this.CurrentStamina = 0;
            }
            else
            {
                this.CurrentStamina -= amount;
            }
            
            GameEventBus.Publish(
                new StaminaDamageTakenEvent(this, amount, Time.frameCount));
        }

        public virtual void RestoreStamina(int amount)
        {
            CurrentStamina = Math.Min(maxStamina, CurrentStamina + amount);
        }

        public void Defend()
        {
            if (isDefending)
            {
                return;
            }

            isDefending = true;

            var bonus = Mathf.RoundToInt(BaseDefense * defendMultiplier);
            defenseModifier += bonus;

            temporaryCritBonus = 0.15f;
        }

        public void ClearTurnFlags()
        {
            if (isDefending)
            {
                var bonus = Mathf.RoundToInt(BaseDefense * defendMultiplier);
                defenseModifier -= bonus;
                isDefending = false;
            }

            temporaryCritBonus = 0.0f;
        }

        public float GetTemporaryCritBonus()
        {
            return temporaryCritBonus;
        }

        public void ApplyDefenseDebuff(int amount, int duration)
        {
            ActiveEffects.Add(new ArmorBreakEffect(amount, duration));
        }

        public virtual Ability ChooseCombatAbility()
        {
            var staminaRatio = (float)CurrentStamina / MaxStamina;
            var defendProbability = 1f - staminaRatio;

            foreach (var ability in Abilities)
            {
                if (!ability.IsOffensive)
                {
                    if (UnityEngine.Random.value < defendProbability)
                    {
                        return ability;
                    }
                }
            }

            var offensiveAbilities = Abilities.Where(a => a.IsOffensive).ToList();

            if (offensiveAbilities.Count > 0)
                return offensiveAbilities[UnityEngine.Random.Range(0, offensiveAbilities.Count)];

            return Abilities[0];
        }

        public virtual int GetInitiativeModifier()
        {
            return Dexterity / 2;
        }

        public void RollInitiative()
        {
            initiativeRoll = GetInitiativeModifier() + UnityEngine.Random.Range(1, 20);
        }

        public Ability ChooseBestCombatAbility(EntityController target)
        {
            Ability bestAbility = null;
            var bestScore = float.MinValue;

            foreach (var ability in Abilities)
            {
                var score = ability.Evaluate(this, target);

                score += UnityEngine.Random.Range(-2f, 2f);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestAbility = ability;
                }
            }

            return bestAbility;
        }

        #endregion

        #region Status Effect Management

        public void AddStatusEffect(StatusEffect effect)
        {
            activeEffects.Add(effect);
            effect.OnApply(this);
        }

        public void ProcessTurnStartEffects()
        {
            foreach (var effect in activeEffects)
            {
                effect.OnTurnStart(this);
            }
        }

        public void ProcessTurnEndEffects()
        {
            for (var i = activeEffects.Count - 1; i >- 0; i--)
            {
                activeEffects[i].OnTurnEnd(this);
                activeEffects[i].Tick(this);

                if (activeEffects[i].IsExpired)
                {
                    activeEffects.RemoveAt(i);
                }
            }
        }

        #endregion

        #region Ability Scoring (AI)

        private Dictionary<Ability, float> primedAbilities = new();

        public bool HasAbility(Ability ability)
        {
            return Abilities.Contains(ability);
        }

        public void PrimeAbility(AbilitySynergy synergy)
        {
            if (synergy == null || synergy.ability == null)
            {
                return;
            }

            primedAbilities[synergy.ability] = synergy.bonusScore;
        }

        public float GetPrimeBonus(Ability ability)
        {
            if (!primedAbilities.TryGetValue(ability, out var bonus))
            {
                return 0f;
            }

            return bonus;
        }

        public void ConsumePrimeBonus(Ability ability)
        {
            primedAbilities[ability] = 0f;
        }

        #endregion
    }
}