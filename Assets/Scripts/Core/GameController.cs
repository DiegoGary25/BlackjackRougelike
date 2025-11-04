using System.Collections.Generic;
using UnityEngine;
using HouseTakes21.Blackjack;
using HouseTakes21.Items;
using HouseTakes21.Trinkets;
using HouseTakes21.UI;

namespace HouseTakes21.Core
{
    /// <summary>
    /// Bootstraps the run and coordinates high level systems.
    /// </summary>
    public sealed class GameController : MonoBehaviour
    {
        [SerializeField]
        private DealerSO dealer;

        [SerializeField]
        private ShoeConfig shoeConfig;

        [SerializeField]
        private ItemLibrary itemLibrary = default!;

        [SerializeField]
        private TrinketLibrary trinketLibrary = default!;

        [SerializeField]
        private RunUIController runUI = default!;

        private readonly GameStateMachine stateMachine = new();
        private readonly GameResources resources = new();

        private RNGService rngService = default!;
        private BlackjackEngine blackjackEngine = default!;
        private TrinketBus trinketBus = default!;
        private ItemController itemController = default!;
        private TableLoop tableLoop = default!;

        /// <summary>
        /// Gets the state machine for external observers.
        /// </summary>
        public GameStateMachine StateMachine => stateMachine;

        private void Awake()
        {
            int seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            rngService = new RNGService(seed);
            blackjackEngine = new BlackjackEngine(resources, rngService);
            trinketBus = new TrinketBus();
            blackjackEngine.SetTrinketBus(trinketBus);
            itemController = new ItemController(resources, rngService, blackjackEngine, trinketBus);
            tableLoop = new TableLoop(stateMachine, blackjackEngine, itemController, trinketBus, resources, rngService, dealer, shoeConfig);

            runUI.Initialize(resources, blackjackEngine, itemController, trinketBus, stateMachine);

            stateMachine.SetState(GameStateMachine.GameState.Boot);
            tableLoop.StartNewRun();
        }

        private void Update()
        {
            tableLoop.Tick();
        }

        /// <summary>
        /// Grants a trinket to the player by id.
        /// </summary>
        /// <param name="trinketId">Identifier.</param>
        public void GrantTrinket(string trinketId)
        {
            TrinketSO? trinket = trinketLibrary.Resolve(trinketId);
            if (trinket != null)
            {
                TrinketSO runtime = Instantiate(trinket);
                runtime.Initialize(rngService);
                trinketBus.Register(runtime);
                runUI.RefreshTrinkets(trinketBus.ActiveTrinkets);
            }
        }

        /// <summary>
        /// Grants an item by identifier to the player.
        /// </summary>
        /// <param name="itemId">Identifier.</param>
        public void GrantItem(string itemId)
        {
            ItemSO? item = itemLibrary.Resolve(itemId);
            if (item != null)
            {
                ItemSO runtime = Instantiate(item);
                itemController.TryAddItem(runtime);
                runUI.RefreshItems(itemController.Items);
            }
        }
    }
}
