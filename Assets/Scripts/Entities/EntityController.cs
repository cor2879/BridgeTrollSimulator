using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Attributes;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Combat.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Components;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Audio;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Enums;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.GameStateManagement;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Core.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Demands.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Dialog;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterSkills;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.CharacterStats;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.Personalities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Entities.StatusEffects;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Feedback.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.InputHandling;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Reactions.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Rewards;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Rewards.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Abilities;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Events;
using OldSchoolGames.BridgeTrollSimulator.Scripts.SocialDuel.Interfaces;
using OldSchoolGames.BridgeTrollSimulator.Scripts.UI;
using OldSchoolGames.BridgeTrollSimulator.Scripts.Utilities;
using UnityEngine;

namespace OldSchoolGames.BridgeTrollSimulator.Scripts.Entities
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public abstract class EntityController 
        : MonoBehaviour, IReceiver, IEncounterable
    {
        protected Animator animator;
        protected Rigidbody2D rb;
        protected Collider2D cldr;
        protected SpriteRenderer sr;
        protected IInputSource inputSource;
        protected EntityCombatUI entityCombatUI;
        protected DemandComponent demandComponent;
        [Header("UI and Sprites")]
        [SerializeField]
        protected GoldPopupUI goldPopupUI;
        [SerializeField]
        private SpeechBubbleUI speechBubble;
        [SerializeField]
        private Sprite battleIntroSprite;
        [SerializeField]
        private Sprite socialDuelIntroSprite;
        [SerializeField]
        private Sprite victorySprite;
        [SerializeField]
        private Sprite defeatedSprite;
        [SerializeField]
        private Sprite deadSprite;

        #region Character Stats

        [Header("Character Stats")]
        [SerializeField]
        protected string entityName;
        [SerializeField]
        protected bool isPlayerControlled;
        [SerializeField]
        protected EntitySize size;
        [SerializeField]
        private int experience;
        [SerializeField]
        private int totalExperience;
        [SerializeField]
        private int progressionPoints;
        [SerializeField]
        private int experienceReward;
        [SerializeField]
        private int fame;
        [SerializeField]
        private int respect;
        [SerializeField, Range(-100, 100)]
        private int reputation;
        [SerializeField]
        protected int level = 1;
        [SerializeField, Range(0f, 1f)]
        protected float bravery = 0.5f;
        [SerializeField]
        protected int gold;
        [SerializeField]
        protected int maxHealth;
        [SerializeField]
        protected int maxStamina = 10;
        [SerializeField]
        protected int maxResolve = 100;
        [SerializeField, ReadOnly]
        protected int currentResolve;
        [SerializeField]
        protected int attackCost = 3;
        [SerializeField]
        protected int defendRestore = 2;
        [SerializeField, ReadOnly]
        protected int currentStamina;
        [SerializeField, ReadOnly]
        protected int currentHealth;
        [SerializeField]
        protected Stats baseStats = new();
        [SerializeField]
        protected Skills baseSkills = new();
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
        private int momentum;
        [SerializeField]
        protected CombatFaction faction;
        [SerializeField]
        protected Ability[] abilities;
        [SerializeField]
        protected SocialAbility[] socialAbilities;
        [SerializeField]
        protected SocialResponseProfile socialResponseProfile;
        [SerializeField]
        protected Personality personality;

        #endregion

        #region Sound Effects

        [Header("Sound Effects")]
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
        public Collider2D Collider => cldr;
        public Rigidbody2D RigidBody => rb;

        public SpriteRenderer SpriteRenderer 
        {
            get
            {
                if (sr == null)
                {
                    sr = GetComponent<SpriteRenderer>();
                }

                return sr;
            }
        }

        public DemandComponent DemandComponent => demandComponent;

        public GameObject GameObject => this.gameObject;
        public ControlMode CurrentControlMode => controlMode;
        public IInputSource InputSource => inputSource;
        public EntityCombatUI CombatUI => entityCombatUI;
        public GoldPopupUI GoldPopupUI => goldPopupUI;
        public SpeechBubbleUI SpeechBubble => speechBubble;
        public bool IsPlayerControlled => isPlayerControlled;
        public EntityDialogLibrary DialogLibrary { get; private set; }
        public Sprite BattleIntroSprite => battleIntroSprite;
        public Sprite SocialDuelIntroSprite => socialDuelIntroSprite;
        public Sprite VictorySprite => victorySprite;
        public Sprite DefeatedSprite => defeatedSprite;
        public Sprite DeadSprite => deadSprite;

        #region Stats

        public string Name 
        { 
            get => entityName;
            set => this.entityName = value; 
        }

        public EntitySize Size { get => size; }
        public int Level { get => level; private set => level = value; }
        public Stats BaseStats => baseStats;
        public Skills BaseSkills => baseSkills;
        public float Bravery => bravery;
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
        public int Experience { get => experience; private set => experience = value; }
        public int TotalExperience { get => totalExperience; private set => totalExperience = value; }
        public int ProgressionPoints => progressionPoints;
        public int ExperienceReward => experienceReward;
        public int Fame => fame;
        public int Respect => respect;
        public int Reputation => reputation;
        public int Dexterity => BaseStats.Dexterity;
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
        public bool IsFacingRight => facingRight;
        public int Momentum => momentum;
        public int MaxResolve => maxResolve;
        public Ability[] Abilities => abilities;
        public SocialAbility[] SocialAbilities => socialAbilities;
        public Personality Personality { get =>  personality; private set => personality = value; }
        public List<StatusEffect> ActiveEffects => activeEffects;

        #endregion

        #region IEventSource

        public GameSystemType SystemType => GameSystemType.Entity;

        public string SourceName
        {
            get
            {
                if (Personality != null)
                {
                    return $"{Name} ({Personality})";
                }

                return entityName;
            } 
        }

        #endregion

        #region IReactor

        public int Resolve  
        { 
            get => currentResolve; 
            private set => currentResolve = value;
        }

        float IReactor.Aggression
        {
            get
            {
                var value = Bravery;
                value += Momentum * 0.1f;
                return Mathf.Clamp01(value);
            }
        }

        int IReactor.Charisma => BaseStats.Charisma;

        public abstract void AcceptSurrender(IReactor opponent, ITargetedEvent evt);
        public abstract void DenySurrender(IReactor opponent, ITargetedEvent evt);

        public virtual void ConcedeCombat(IReceiver opponent)
        {
            GameEventBus.Publish(
                new ConcedeCombatEvent(
                    this,
                    opponent,
                    null,
                    Time.frameCount));
        }

        public virtual void ConcedeSocialDuel(IReceiver opponent)
        {
            GameEventBus.Publish(
                new ConcedeSocialDuelEvent(
                    this,
                    opponent,
                    Time.frameCount));
        }

        #endregion

        #endregion

        #region Initialization and Iteration

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();
            cldr = GetComponent<Collider2D>();
            demandComponent = GetComponent<DemandComponent>();
            combatUIRoot = GetComponentInChildren<Canvas>();
            instanceId = Guid.NewGuid();
            entityCombatUI = GetComponent<EntityCombatUI>();
            entityCombatUI.SetActive(false);
            DialogLibrary = GetComponent<EntityDialogLibrary>();
            
            if (this.goldPopupUI != null)
            {
                this.goldPopupUI.SetActive(false);
            }

            RecalculateDerivedStats();
            currentHealth = maxHealth;
            currentResolve = maxResolve;
        }

        protected virtual void Update()
        {
            if (GameStateSystem.Instance.IsPaused &&
                CurrentControlMode != ControlMode.Passing &&
                CurrentControlMode != ControlMode.Leaving)
            {
                // animator.speed = 0f;
                animatorSpeed = 0f;
                this.UpdateAnimator();
                return;
            }

            this.ProcessInput();
            this.UpdateAnimator();
        }

        protected virtual void FixedUpdate()
        {
            if (GameStateSystem.Instance.IsPaused &&
                CurrentControlMode != ControlMode.Passing &&
                CurrentControlMode != ControlMode.Leaving)
            {
                return;
            }

            this.ApplyMovement();
        }

        private void CleanupUI()
        {
            ClearSpeech();

            if (entityCombatUI != null)
            {
                entityCombatUI.SetActive(false);
            }

            if (goldPopupUI != null)
            {
                goldPopupUI.SetActive(false);
            }
        }

        #endregion

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
                controlMode == ControlMode.Encounter ||
                controlMode == ControlMode.Combat)
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
            Debug.Log($"{Name} : ControlMode : {mode}");
        }

        public void ResetControlMode(bool overrideDeath = false)
        {
            if (CurrentControlMode != ControlMode.Dead ||
                overrideDeath)
            {
                SetControlMode(DefaultControlMode);
            }
        }

        #endregion

        #region Combat

        public void EnterCombat()
        {
            SetControlMode(ControlMode.Combat);
            CurrentStamina = MaxStamina;
            RollInitiative();
        }

        public abstract void OnCombatVictory(CombatResolutionData data);

        public abstract void OnCombatDefeat(CombatResolutionData data);

        #endregion

        #region Social Duel

        public abstract void OnSocialDuelVictory(SocialDuelResolutionData data);

        public abstract void OnSocialDuelLoss(SocialDuelResolutionData data);

        #endregion

        #region Movement

        protected virtual void ApplyMovement()
        {
            if (CurrentControlMode == ControlMode.Encounter)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            if (CurrentControlMode == ControlMode.Passing)
            {
                SetFacing(false);
                rb.linearVelocity = new Vector2(
                    moveSpeed * (facingRight ? 1f : -1f),
                    rb.linearVelocity.y);
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
                rb.linearVelocity.y);
        }

        public void SetFacing(bool faceRight)
        {
            if (facingRight == faceRight)
            {
                return;
            }

            facingRight = faceRight;
            animator.SetFloat(
                Constants.AnimatorParams.xDirection,
                facingRight ? 1f : -1f);
        }

        protected void Flip()
        {
            SetFacing(!facingRight);
        }

        #endregion

        #region Animation

        protected virtual void UpdateAnimator()
        {
            animator.speed = 1f;
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

        public void OnDeathAnimationComplete()
        {
            
        }

        public void BeginDespawn()
        {
            CleanupUI();
            StartCoroutine(DespawnAfterDelay(0.5f));
            GameEventBus.Publish(
                new EntityDespawningEvent(this, Time.frameCount));
        }

        private IEnumerator DespawnAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy (gameObject);
        }

        #endregion

        #region Collision Detection

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<EntityController>(out var otherEntity))
            {
                return;
            } 

            // Only trigger encounter if exactly one is player-controlled
            if (this.IsPlayerControlled == otherEntity.IsPlayerControlled)
            {
                return;
            }

            var player = this.IsPlayerControlled ? this : otherEntity;
            var npc    = this.IsPlayerControlled ? otherEntity : this;

            if (npc.CurrentControlMode != ControlMode.Passing &&
                npc.CurrentControlMode != ControlMode.Leaving)
            {
                player.RigidBody.linearVelocity = Vector2.zero;

                GameEventBus.Publish(
                    new EntityEncounterEvent(player, npc, Time.frameCount));
            }
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
            
            GameEventBus.Publish(new GoldDeductedEvent(
                this,
                goldReturned,
                Time.frameCount));

            return goldReturned;
        }

        public virtual void AddGold(int amount)
        {
            this.gold += amount;

            if (amount > 0)
            {
                GameEventBus.Publish(new GoldAddedEvent(
                    this,
                    amount,
                    Time.frameCount));
            }
        }
        
        public virtual void TakeDamage(int amount, bool isCrit = false)
        {
            this.CurrentHealth -= amount;
            GameEventBus.Publish(
                new DamageTakenEvent(this, amount, isCrit));

            AudioSystem.Instance.PlaySFX(hurtSfx);

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }

        public virtual void TakeResolveDamage(int amount, bool isCrit = false)
        {
            var oldResolve = Resolve;

            Resolve = Mathf.Max(Resolve - amount, 0);
            var delta = Resolve - oldResolve;

            GameEventBus.Publish(
                new ResolveChangedEvent(
                    this,
                    delta,
                    isCrit,
                    Time.frameCount));

            if (amount > 0)
            {
                GameEventBus.Publish(
                    new ResolveDamageTakenEvent(
                        this,
                        amount,
                        isCrit));
            }

            if (oldResolve > 0 && Resolve <= 0)
            {
                GameEventBus.Publish(
                    new ResolveBrokenEvent(
                        this));
            }
        }

        public virtual void Die()
        {
            SetControlMode(ControlMode.Dead);
            TriggerAction(Constants.Triggers.Die);
            GameEventBus.Publish(new EntityDiedEvent(this, Time.frameCount));
            GameEventBus.Publish(new SoundEffectEvent(this, deathSfx, Time.frameCount));
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

            GameEventBus.Publish(
                new DefendEvent(this, Time.frameCount));
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

            #if UNITY_EDITOR

            var sb = new StringBuilder();
            sb.AppendLine($"----- [AI] {Name} evaluating combat abilities vs {target.Name} -----");

            #endif

            foreach (var ability in Abilities)
            {
                var baseScore = ability.Evaluate(this, target);
                var randomFactor = UnityEngine.Random.Range(-2f, 2f);
                var finalScore = baseScore + randomFactor;

                #if UNITY_EDITOR

                sb.AppendLine($"\t* Ability: {ability.Name} | " +
                             $"Base: {baseScore:F2} | " +
                             $"Rand: {randomFactor:F2} | " +
                             $"Final: {finalScore:F2}");

                #endif

                if (finalScore > bestScore)
                {
                    bestScore = finalScore;
                    bestAbility = ability;
                }
            }

            #if UNITY_EDITOR

            sb.AppendLine($"[AI] {Name} selected: {bestAbility?.Name} " +
                        $"with score {bestScore:F2}");
            Debug.Log(sb.ToString());

            #endif

            return bestAbility;
        }

        public void PayToll(IReceiver payee, int amount)
        {
            GameEventBus.Publish(
                    new TollPaidEvent(
                        this, 
                        payee, 
                        DeductGold(amount), 
                        Time.frameCount));

            this.SpeechBubble.Show(
                this.Personality.GetPayTollDialog());
        }

        private float CalculatePayChance(int tollAmount)
        {
            float braveryFactor = Mathf.Pow(1f - Bravery, 2f);

            float wealthRatio = Gold / (float)tollAmount;
            float wealthFactor = Mathf.Clamp01(Mathf.Log(wealthRatio + 1f));

            float payChance = (braveryFactor * 0.7f) + (wealthFactor * 0.3f);

            payChance += UnityEngine.Random.Range(-0.25f, 0.25f);

            return Mathf.Clamp01(payChance);
        }

        public virtual void Receive<TEvent>(TEvent evt) where TEvent : ITargetedEvent
        {
            if (evt is RewardEvent rewardEvent)
            {
                AddExperience(rewardEvent.Reward.Experience);
                AddFame(rewardEvent.Reward.FameDelta);
                AddRespect(rewardEvent.Reward.RespectDelta);
                AddReputation(rewardEvent.Reward.ReputationDelta);
                AddGold(rewardEvent.Reward.Gold);
                RestoreResolve(rewardEvent.Reward.Resolve);
            }

            if (evt is LevelUpConfirmedEvent levelUp)
            {
                HandleLevelUpConfirmed(levelUp);
            }
        }

        public void AddFame(int amount)
        {
            fame += amount;
        }

        public void AddRespect(int amount)
        {
            respect += amount;
        }

        public void AddReputation(int amount)
        {
            reputation = Mathf.Clamp(reputation + amount, -100, 100);
        }

        protected virtual void HandleLevelUpConfirmed(LevelUpConfirmedEvent levelUp)
        {
            ApplyLevelUpBenefits();
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
            for (var i = activeEffects.Count - 1; i >= 0; i--)
            {
                activeEffects[i].OnTurnEnd(this);
                activeEffects[i].Tick(this);

                if (activeEffects[i].IsExpired)
                {
                    activeEffects.RemoveAt(i);
                }
            }
        }

        public void RestoreResolve(int amount)
        {
            Debug.Log($"{SourceName}::RestoreResolve:{amount}");
            currentResolve = Mathf.Min(MaxResolve, Resolve + amount);
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

        #region Leveling and Stats Management

        private int GetXpRequiredForNextLevel()
        {
            const float baseXp = 100f;
            return Mathf.RoundToInt(baseXp * Mathf.Pow(Level, 1.5f));
        }

        public void AddExperience(int amount)
        {
            if (amount <= 0) return;

            TotalExperience += amount;
            Experience += amount;
            CheckForLevelUp();
        }

        private void CheckForLevelUp()
        {
            while (Experience >= GetXpRequiredForNextLevel())
            {
                Experience -= GetXpRequiredForNextLevel();
                Level++;

                ApplyPerLevelGrowth();

                int pointsGranted = 1; // for now, fixed
                progressionPoints += pointsGranted;

                GameEventBus.Publish(
                    new LevelUpEvent(
                        this,
                        this,
                        Level,
                        pointsGranted,
                        Time.frameCount));
            }
        }

        private void ApplyPerLevelGrowth()
        {
            maxHealth += 5;
            maxStamina += 1;
            maxResolve += 3;

            CurrentHealth = maxHealth;
            CurrentStamina = maxStamina;
            Resolve = maxResolve;
        }

        private void ApplyLevelUpBenefits()
        {
            RecalculateDerivedStats();
        }

        private void RecalculateDerivedStats()
        {
            attack = 2 + BaseStats.GetModifier(StatType.Strength);
            defense = BaseStats.GetModifier(StatType.Constitution);

            int mentalStat = Mathf.Max(
                BaseStats.GetModifier(StatType.Charisma),
                BaseStats.GetModifier(StatType.Wisdom),
                BaseStats.GetModifier(StatType.Intelligence));

            maxHealth =  40 + (Level * (5 + BaseStats.GetModifier(StatType.Constitution)));
            maxStamina = 10 + (Level * (1 + BaseStats.GetModifier(StatType.Dexterity)));
            maxResolve = (Level * 3) + 30 + mentalStat;

            critChance = 0.05f + (BaseStats.Luck * 0.005f);
            critChance = Mathf.Clamp(critChance, 0f, 0.5f);
            GameEventBus.Publish(
                new EntityStatsRecalculatedEvent(this, Time.frameCount));
        }

        public bool TrySpendPoint(StatType stat)
        {
            if (progressionPoints <= 0)
                return false;

            baseStats.Add(stat, 1);
            progressionPoints--;

            return true;
        }

        public void ConsumeProgressionPoints(int amount)
        {
            progressionPoints -= amount;
        }

        #endregion

        #region Dialog

        public void Speak(string line)
        {
            if (speechBubble == null)
            {
                Debug.Log($"{Name} Speech Bubble activated but no Speech Bubble designated.");
                return;
            }

            ClearSpeech();
            Debug.Log($"{SourceName}::{nameof(Speak)}::\"line\" @ Frame {Time.frameCount}");
            speechBubble.Show(line);
        }

        public void ClearSpeech()
        {
            if (speechBubble == null)
            {
                return;
            }

            speechBubble.Hide();
        }

        public string GetSocialResponse(
            SocialExchangeOutcome outcome,
            int currentResolve,
            int maxResolve)
        {
            if (Personality == null)
                return string.Empty;

            var reaction = Personality.GetReaction(
                outcome.GoverningSkill,
                outcome.Result);

            if (string.IsNullOrEmpty(reaction))
            {
                return "...";
            }

            return reaction;
        }

        public void ModifyMomentum(int amount)
        {
            momentum = Mathf.Clamp(momentum + amount, -3, 3);
        }

        public void DecayMomentum()
        {
            if (momentum > 0) momentum--;
            if (momentum < 0) momentum++;
        }

        public void AssignPersonality(Personality personality)
        {
            Personality = personality;
        }

        #endregion
    }
}