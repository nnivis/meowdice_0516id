# Dice Battle (Unity) — Документация по проекту

Пошаговая настольная игра с кубиками на двоих. Каждый ход активный игрок бросает
кубик (1–6) и кладёт его в свободную клетку **своего** поля 3×3. Когда оба поля
заполнены — считаются очки по колонкам, определяется победитель или ничья.

Документ описывает **текущую (реальную) архитектуру** и **поток запуска**.
Полностью реализован режим **Vs AI**. `LocalPvp` и `OnlinePvp` — заготовки
(`GameMode` есть, источники оппонента — заглушки).

---

## 1) Цели и принципы

- **Domain** не зависит от Unity-инфраструктуры (правила/состояние матча — чистый C#;
  отдельные доменные классы пока используют `UnityEngine.Random`/presenter-MonoBehaviour, см. §10).
- Оркестрация боя вынесена в **Services** (Flow, Turn, GameStart, StateMachine).
- Режимы переключаются через провайдеры участников и фабрики контроллеров + DI.
- Направление зависимостей: `Infrastructure/Unity → Services → Domain`.
- Сборка зависимостей — **VContainer**.

---

## 2) Слои

| Слой | Содержит | Чего НЕ должно быть |
|---|---|---|
| **Domain** (`CodeBase.Domain`) | `Match`, `MatchState`, `Board`, `Field`, `FieldColumn`, `Dice`, правила и подсчёт очков, presenter'ы/view доменных сущностей | сохранения, сеть, оркестрация флоу |
| **Services** (`CodeBase.Services`) | `GameCoordinator`, `GameFlow`, `TurnSystem`, контроллеры/AI, state machine, GameStart-конфиг, результат матча | прямой доступ к сценам/файлам |
| **Infrastructure** (`CodeBase.Infrastructure`) | DI-скоупы, bootstrap, загрузка сцен, провайдеры данных | доменные правила |
| **Data** (`CodeBase.Data`) | `PlayerData`, `PlayerId`, заготовки онлайна | логика |
| **UI** (`CodeBase.UI`) | `GameplayUiRoot` — ссылки на корневые view сцены | доменная логика |

> `CodeBase.Logick` (`GameSession`, `PlayerSessionFactory`) и `CodeBase.Domain.GameState`
> — пустые/legacy-заготовки, в текущем флоу не задействованы.

---

## 3) Запуск игры (Runtime Flow)

Две сцены и два DI-скоупа: **BootstrapScene** (`RootLifetimeScope`) и
**GameplayScene** (`GameplayLifetimeScope`). Имена сцен — в `SceneCatalog`.

```
RootLifetimeScope.Awake()
  ├─ DontDestroyOnLoad(self)
  ├─ Configure(): Infrastructure + Bootstrap + PersistentData
  └─ _bootstrapper.Run()
        └─ Bootstrapper.Run() → StartCoroutine(StartupSequence.Run())
              StartupSequence:
                ├─ LoadingScreen.Show()
                ├─ DataBootstrap.Initialize()   // создать/починить PlayerData
                └─ SceneLoader.Load(GameplayScene, loading)

GameplayScene → GameplayLifetimeScope.Configure()
  ├─ RegisterComponentInHierarchy: BootstrapComponents, MainSceneMode,
  │                                GameCoordinator, GameplayState, GameResultState
  ├─ GameStart / Participants / FirstTurn / MatchResultStorage
  ├─ Domain: FieldScoreCalculator, DefaultMatchRules, MatchFactory, OpponentFactory
  ├─ Turn: RandomAiMoveStrategy, VsAiControllerFactory
  └─ EntryPoint: GameplayEntryPoint (IStartable)

GameplayEntryPoint.Start()
  └─ BootstrapComponents.Run()
        ├─ (Construct) GameCoordinator.InitData(dataProvider, persistentData)
        ├─ MainSceneMode.Bootstrap()   // создаёт StateMachine, входит в state #0
        └─ MainSceneMode.GoToMain()    // → MainSceneState
```

> Внимание: в проекте **два разных** bootstrap-класса.
> `Bootstrapper` (root-сцена) запускает корутину `StartupSequence`.
> `BootstrapComponents` (gameplay-сцена) поднимает state machine.

### Жизненный цикл матча (state machine)

`MainSceneMode : StateModeBehavior` управляет состояниями. Состояния — это
`StateMachineBehavior` на сцене, у каждого свой список `_views`, который
машина включает/выключает на `Enter`/`Exit`.

```
MainSceneState   --[Start button]-->  GameplayState
GameplayState.OnEnter:
  ├─ subscribe GameCoordinator.OnResultReady
  ├─ config = GameStartConfigFactory.CreateConfig(GameMode.VsAi)
  └─ GameCoordinator.StartGame(config)

(матч завершился) → Match.GameEnded
  → GameCoordinator.OnGameEnded → строит Services.GameResult.MatchResult
  → OnResultReady → GameplayState.OnResultReady
       ├─ MatchResultStorage.Set(result)
       └─ MainSceneMode.GoToGameResult()

GameResultState.OnEnter: GameResultPanel.Show(MatchResultStorage.Current)
GameResultState  --[Exit button]-->  MainSceneState
```

`RatingState` существует, но в `MainSceneMode` перехода на него пока нет.

---

## 4) Старт матча: `GameCoordinator.StartGame(config)`

`GameCoordinator` (MonoBehaviour на gameplay-сцене) — центральная точка сборки боя:

1. `MatchFactory.Create(players, config.FirstTurnPlayer)` → доменный `Match`
   (внутри `MatchState` с `Board`/`Field` на каждого игрока + `TurnOrder`).
2. Подписка на `Match.GameEnded`.
3. Сборка `MatchPlayerContext`: локальный игрок из `IPersistentData.PlayerData`
   (id, имя, скин), оппонент — через `IOpponentFactory.Create(...)`.
4. `MatchPresenter.StartMatch(match, context)` — биндит UI (`GameplayUiRoot`:
   `Boards`, `Field`, `Characters`).
5. `IControllerFactory.CreateControllers(config, match)` → словарь
   `PlayerId → IPlayerController`.
6. Локальный контроллер пробрасывается в `DiceDragDropController.Init(...)`.
7. Создаются `TurnSystem(match, controllers, maxAttempts: 1)` и
   `GameFlow(match, turnSystem)`, `GameFlow.Start()`.

`StopGame()` останавливает `GameFlow`, отписывается от `GameEnded`,
вызывает `MatchPresenter.StopMatch()`.

### `GameStartConfig`

`readonly struct` с тремя полями: `Mode`, `Participants` (`MatchParticipants`:
`Local`/`Opponent` `PlayerId`), `FirstTurnPlayer`. Фабрику контроллеров
**не** хранит — она инжектится в `GameCoordinator` отдельно.

`GameStartConfigFactory.CreateConfig(mode)`:
- `IParticipantsProvider.GetParticipants()` (валидирует local ≠ opponent),
- `IFirstTurnSelector.SelectFirstTurn(participants)`,
- повторная проверка на дубль id.

---

## 5) Игровой цикл: `GameFlow` + `TurnSystem`

`GameFlow.RunAsync` — `async` цикл с `CancellationToken`:

```
while (!match.IsFinished && !ct.Cancelled)
    await turnSystem.PlayTurnAsync(ct)
```

`TurnSystem.PlayTurnAsync` для одного хода:

1. `player = match.ActivePlayer`, `ctrl = controllers[player]`.
2. `match.IsStartTurn(player)`:
   - бросает кубик (`Board.RollDice()`), если поле не полно;
   - если поле игрока полно — `EndTurn()` и ход пропускается;
   - если правила определили результат — матч финишируется.
3. Цикл попыток размещения:
   - `pos = await ctrl.RequestPlacementAsync(player, ct)`,
   - `match.TryPlaceDice(player, pos)`:
     - успех → `ctrl.NotifyPlacementAccepted()`, `EndTurn()` (если матч не окончен),
     - неуспех → `ctrl.NotifyPlacementRejected()` и повтор.

`IPlayerController`:
- `LocalHumanPlayerController` — ждёт ввод через `TaskCompletionSource<CellPosition>`;
  `DiceDragDropController` ловит drag&drop и вызывает `TrySubmitPlacement(pos)`.
- `AiController` — `Task.Delay(500)`, затем `IAiMoveStrategy.ChoosePlacement(...)`
  (`RandomAiMoveStrategy` выбирает случайную свободную клетку своего поля).

---

## 6) Правила и подсчёт очков (Domain)

Поле — 3×3 (`Field` → 3 × `FieldColumn` × 3 `Cell`). Кубик: `DicePointType`
(One..Six → `Value` 1..6) и `DiceStateType` (`Normal`, `MatchedPair`, `FullColumn`).
`FieldColumn` пересчитывает состояние при каждом размещении.

**`DefaultMatchRules.ResolveAfterPlacement`** — после хода в колонке игрока:
- `MatchedPair` (2 одинаковых в колонке) и у оппонента та же колонка тоже
  `MatchedPair` с тем же значением → у оппонента удаляются кубики этого
  значения в этой колонке.
- `FullColumn` из трёх одинаковых, и у оппонента та же колонка тоже
  однородный `FullColumn` с тем же значением → колонка оппонента полностью
  очищается.

**Завершение матча** — `TryGetResult`: только когда **все** поля заполнены.
Иначе `null`. Сравниваются суммарные очки двух игроков → `Win(player)` или `Draw()`.

**Подсчёт очков** (`FieldScoreCalculator`): очко колонки = сумма по каждому
различному значению `value × count²` (т.е. совпадения квадратично усиливают вклад,
три одинаковых `v` дают `v × 9`). Очки поля = сумма по колонкам.

---

## 7) Участники и режимы

| Интерфейс | Реализация (текущая) | Назначение |
|---|---|---|
| `ILocalPlayerDataProvider` | `PersistentLocalPlayerDataProvider` | id локального игрока из `PlayerData` |
| `IOpponentIdSource` | `AiOpponentIdSource` → `PlayerId(999)` | id оппонента |
| `IParticipantsProvider` | `ParticipantsProvider` | собирает `MatchParticipants`, проверяет local ≠ opponent |
| `IFirstTurnSelector` | `LocalFirstTurnSelector` (Random — закомментирован) | кто ходит первым |
| `IControllerFactory` | `VsAiControllerFactory` | local → `LocalHumanPlayerController`, opponent → `AiController` |
| `IAiMoveStrategy` | `RandomAiMoveStrategy` | выбор хода ИИ |
| `IOpponentFactory` | `OpponentFactory` | `MatchPlayerInfo` оппонента (имя/скин) |

Заготовки для будущих режимов: `SecondLocalOpponentIdSource`,
`OnlineOpponentIdSource`, `IOnlineLobby`/`OnlineLobby` — в DI не зарегистрированы.

---

## 8) Данные и сохранение

- `PlayerData`: `Id (PlayerId)`, `Name`, `Score`, `SkinId (CatSkinId)`,
  сериализуется через Newtonsoft.Json.
- `IPersistentData` (`PersistentData`) — runtime-хранилище текущего `PlayerData`.
- `IDataProvider` (`DataLocalProvider`) — `TryLoad()/Save()` на диск.
- `DataBootstrap.Initialize()` — если данных нет, создаёт нового игрока;
  чинит дефолты (имя `Murchik`, скин `White`).
- Косметика котов: `CatSkinId`, `CatSkinCatalog`, `CatSkinDefinition`,
  `SpritePartVariant`, рисуется `CatSkinView`/`PlayerCharacterPresenter`.

---

## 9) VContainer: что где регистрируется

### `RootLifetimeScope` (BootstrapScene, `DontDestroyOnLoad`)
- `SceneCatalog`, `ICoroutineRunner`, `ILoadingScreen`, `ISceneLoader`
- `IDataBootstrap`, `IStartupSequence`, `Bootstrapper` (component)
- `IPersistentData → PersistentData`, `IDataProvider → DataLocalProvider`

### `GameplayLifetimeScope` (GameplayScene)
- Scene components: `BootstrapComponents`, `MainSceneMode`, `GameCoordinator`,
  `GameplayState`, `GameResultState`
- `ILocalPlayerDataProvider`, `IOpponentIdSource`, `IParticipantsProvider`,
  `IFirstTurnSelector`, `GameStartConfigFactory`, `MatchResultStorage`
- `IFieldScoreCalculator`, `IMatchRules`, `MatchFactory`, `IOpponentFactory`
- `IAiMoveStrategy`, `IControllerFactory`
- `RegisterEntryPoint<GameplayEntryPoint>()`

> `RegisterComponentInHierarchy<T>()` регистрирует уже существующий
> MonoBehaviour со сцены в DI (для инъекций), но **не** запускает его.

---

## 10) Граф зависимостей

Разрешено: `Infrastructure/Unity → Services → Domain`.
Запрещено: `Domain → Infrastructure`, `Domain → Services`.

Известные компромиссы (кандидаты на рефакторинг):
- `Domain.Board.Board`, `Domain.Dice.Dice` используют `UnityEngine.Random`/`UnityEngine`.
- presenter'ы (`MatchPresenter`, `FieldPresenter`, `BoardsPresenter`,
  `PlayerCharacterPresenter`) — MonoBehaviour в слое Domain (мосты к UI).

---

## 11) Структура папок (фактическая)

```
Assets/CodeBase/
  Data/
    PlayerData.cs
    PlayerDataComponents/PlayerId.cs
    Online/IOnlineLobby.cs, OnlineLobby.cs           (заготовка)
  Domain/
    GameState.cs                                     (legacy, не используется)
    PlayerSlot.cs, PlayerSlotResolver.cs
    Board/   Board.cs, BoardView.cs, BoardsPresenter.cs
    Dice/    Dice.cs, DiceView*.cs, DiceViewRegistry/Factory
    Field/   Field.cs, FieldColumn.cs, FieldPanel.cs, FieldPresenter.cs
      Cell/  Cell.cs, CellPosition.cs, CellView.cs
      View/  FieldView.cs, FieldColumnView(.Factory).cs
    Character/ PlayerCharacterPresenter.cs
      CatSkin/ CatSkinView.cs
        Content/ CatSkinId, CatSkinCatalog, CatSkinDefinition, SpritePartVariant
    Match/   Match.cs, MatchFactory.cs, MatchPresenter.cs, IMatchReadModel.cs
      Module/ MatchState.cs, TurnOrder.cs, MatchResult.cs
      Data/   MatchPlayerInfo.cs, MatchPlayerContext.cs, MatchPlayerState.cs
      Content/ IOpponentFactory.cs, OpponentFactory.cs
      Rules/  IMatchRules.cs, DefaultMatchRules.cs
        ScoreCalculator/ IFieldScoreCalculator.cs, FieldScoreCalculator.cs
  Infrastructure/
    Bootstrapper.cs, BootstrapComponents.cs, GameplayEntryPoint.cs
    DataBootstrap.cs, StartupSequence.cs
    VContainer/   RootLifetimeScope.cs, GameplayLifetimeScope.cs
    DataProvider/ IPersistentData, PersistentData, IDataProvider,
                  DataLocalProvider, IDataBootstrap,
                  ILocalPlayerDataProvider, PersistentLocalPlayerDataProvider
    SceneLoad/    SceneCatalog, ISceneLoader/SceneLoader,
                  ILoadingScreen/LoadingScreen, ICoroutineRunner/CoroutineRunner,
                  IStartupSequence
  Services/
    GameCoordinator.cs
    Flow/        GameFlow.cs, TurnSystem.cs
    GameStart/   GameMode.cs, GameStartConfig.cs, GameStartConfigFactory.cs,
                 MatchParticipants.cs
    Participants/ IParticipantsProvider, ParticipantsProvider,
                  IOpponentIdSource, AiOpponentIdSource,
                  SecondLocalOpponentIdSource, OnlineOpponentIdSource
    FirstTurn/   IFirstTurnSelector, LocalFirstTurnSelector, RandomFirstTurnSelector
    Turn/        IPlayerController, LocalHumanPlayerController, Move
      Controllers/ IControllerFactory, VsAiControllerFactory
      Ai/        IAiMoveStrategy, RandomAiMoveStrategy, AiController
    Interaction/ DiceDragDropController, DiceDragContext,
                 DiceDropTargetInfo, IDiceDropCellTarget, DiceDropCellTarget
    StateMachine/ StateMachine, IStateMachine, StateMachineBehavior,
                  StateModeBehavior, MainSceneMode
      States/    MainSceneState, GameplayState, GameResultState, RatingState
    GameResult/  MatchResult.cs, MatchResultStorage.cs, GameResultPanel.cs
  Logick/        GameSession.cs, PlayerSessionFactory.cs   (legacy/пустые)
  UI/            GameplayUiRoot.cs
```

---

## 12) Расширение режимов (на будущее)

**LocalPvp**: зарегистрировать `IOpponentIdSource → SecondLocalOpponentIdSource`
и фабрику контроллеров, где оба игрока — `LocalHumanPlayerController`
(переключение по `GameMode`, отдельным скоупом или резолвером).

**OnlinePvp**: реализовать `IOnlineLobby`, инжектить в `OnlineOpponentIdSource`,
добавить онлайн-фабрику контроллеров и сетевую синхронизацию ходов.

---

## 13) Чек-лист частых проблем

- `ParticipantsProvider` требует **оба**: `ILocalPlayerDataProvider` и
  `IOpponentIdSource`. Нет биндинга → ошибка контейнера.
- `AiOpponentIdSource` отдаёт `PlayerId(999)`; локальный id берётся из
  сохранения — они должны различаться (иначе `GameStartConfigFactory` бросит исключение).
- Состояния (`StateMachineBehavior`) должны быть в списке `_states`
  у `MainSceneMode`, иначе `StateMachine.Change` залогирует «state not found».
- `GameResultState` без `MatchResultStorage.Current` залогирует ошибку
  и не покажет панель — проверь, что `GameplayState.OnResultReady` отработал.
- `MatchFactory`/`Match` ожидают ровно 2 игроков; `DefaultMatchRules` —
  тоже (бросает `InvalidOperationException` при ином количестве).

---

## 14) Ответственность ключевых классов

| Компонент | Ответственность |
|---|---|
| `RootLifetimeScope` | DI root-сцены; стартует `Bootstrapper` |
| `Bootstrapper` / `StartupSequence` | загрузка данных и переход в GameplayScene |
| `GameplayLifetimeScope` / `GameplayEntryPoint` | DI gameplay-сцены; запуск `BootstrapComponents` |
| `BootstrapComponents` | инициализация данных координатора + старт state machine |
| `MainSceneMode` / `StateMachine` | переключение экранов (Main/Gameplay/Result) |
| `GameplayState` | собирает конфиг, стартует/останавливает матч |
| `GameCoordinator` | сборка матча: factory + context + presenter + controllers + flow |
| `GameStartConfigFactory` | `GameStartConfig` из провайдеров участников/первого хода |
| `MatchFactory` | создаёт доменный `Match`/`MatchState` |
| `Match` | доменная модель: ход, размещение, события, финиш |
| `GameFlow` / `TurnSystem` | async-цикл матча и одного хода |
| `LocalHumanPlayerController` / `AiController` | источники решения о размещении |
| `DiceDragDropController` | drag&drop кубика → submit в локальный контроллер |
| `DefaultMatchRules` / `FieldScoreCalculator` | правила взаимодействия колонок и подсчёт очков |
| `MatchResultStorage` / `GameResultPanel` | передача и показ итога матча |

---

## 15) Технологии

- Unity, C#
- VContainer (DI, `IStartable`/EntryPoint, `LifetimeScope`)
- Newtonsoft.Json (Unity package) — сериализация `PlayerData`
- `async/await` + `CancellationToken` для игрового цикла
