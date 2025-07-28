using System.Collections.Concurrent;
using System.Numerics;
using Box2DSharp.Dynamics;
using Luna.Maths;

namespace Luna.Box2D;

using Iteration = (int Velocity, int Position);

public class WorldManager : Node
{
    public static WorldManager Instance
    {
        get
        {
            if (_instance is null)
            {
                var worldManager = Tree.Root.FindNode<WorldManager>("WorldManager");
                if (worldManager is null)
                {
                    return _instance = new();
                }

                return worldManager;
            }

            return _instance;
        }
    }

    public static float PixelsPerMeter { get; set; } = 1;

    private static readonly Dictionary<int, World> Worlds = [];
    private static readonly Dictionary<int, Iteration> Iterations = [];

    private static WorldManager? _instance = null;

    private static ContactListener _contactListener = new();

    private static CancellationTokenSource? _physicsCancellationTokenSource;
    private static Task? _physicsLoopTask;
    private static readonly ConcurrentDictionary<string, (Vector2 Position, float Angle)> _physicsResultsBuffer = [];

    private static float _accumulator;

    private WorldManager()
    {
    }

    public override void Start()
    {
        base.Start();

        StartPhysicsLoopThread();
    }

    public override void FixedUpdate()
    {
        var collisionBodies = Tree.GetAllNodesOfType<CollisionBody2D>();
        ApplyPhysicsResultsToGameObjects(collisionBodies);
    }

    public static World GetWorld(int worldIndex = 0)
    {
        if (Tree.FindNodeByUID<WorldManager>(Instance.UID) is null)
        {
            Tree.Root.AddChild(Instance);
        }

        if (Worlds.TryGetValue(worldIndex, out var world))
        {
            return world;
        }

        var newWorld = new World();
        newWorld.SetContactListener(_contactListener);
        Worlds.Add(worldIndex, newWorld);
        if (!Iterations.TryGetValue(worldIndex, out _))
        {
            Iterations[worldIndex] = (8, 3);
        }

        return newWorld;
    }

    public static void SetIterations(int worldIndex, Iteration iteration)
        => Iterations[worldIndex] = iteration;

    private static void StartPhysicsLoopThread()
    {
        if (_physicsLoopTask != null && !_physicsLoopTask.IsCompleted)
            return;

        _physicsCancellationTokenSource = new CancellationTokenSource();
        var token = _physicsCancellationTokenSource.Token;

        _physicsLoopTask = Task.Run(async () =>
        {
            var stopWatch = System.Diagnostics.Stopwatch.StartNew();
            long lastFrameTicks = stopWatch.ElapsedTicks;

            while (!token.IsCancellationRequested)
            {
                long currentTicks = stopWatch.ElapsedTicks;
                float deltaTime = (float)((currentTicks - lastFrameTicks) / (double)System.Diagnostics.Stopwatch.Frequency);
                lastFrameTicks = currentTicks;

                // Acumulador de tempo próprio para a física.
                // Não dependa de Physics.Time.DeltaTime, pois ele é da thread principal.
                _accumulator += deltaTime; // _accumulator aqui é um campo de WorldManager, não Physics

                while (_accumulator >= Physics.TimeStep) // Usando Physics.TimeStep do Core
                {
                    // === EXECUÇÃO DO PASSO DE FÍSICA DO BOX2D ===
                    foreach (var pair in Worlds)
                    {
                        var worldId = pair.Key;
                        var worldInstance = pair.Value;
                        var iteration = Iterations[worldId]; // Acessa diretamente o dictionary

                        // Executa o Step do Box2D. Isto é feito SEQUENCIALMENTE para cada mundo,
                        // mas todos os passos para TODOS os mundos acontecem NESTA thread de física.
                        worldInstance.Step(Physics.TimeStep, iteration.Velocity, iteration.Position);

                        // === COLETAR RESULTADOS E ENFILEIRAR PARA A THREAD PRINCIPAL ===
                        foreach (var body in worldInstance.BodyList)
                        {
                            // Supondo que Body.UserData é o UID (Guid) do seu CollisionBody2D
                            if (body.UserData is string bodyUid)
                            {
                                // Atualiza o buffer de resultados. ConcurrentDictionary é thread-safe.
                                _physicsResultsBuffer[bodyUid] = (body.GetPosition(), body.GetAngle());
                            }
                        }
                    }
                    _accumulator -= Physics.TimeStep;
                }

                // Pequena pausa para evitar CPU 100%
                await Task.Delay(1, token);
            }
        }, token);
    }
    
    private static void StopPhysicsLoopThread()
    {
        _physicsCancellationTokenSource?.Cancel();
        try
        {
            _physicsLoopTask?.Wait(); // Espera a tarefa de física terminar
        }
        catch (OperationCanceledException) { }
        _physicsLoopTask = null;
    }

    private static void ApplyPhysicsResultsToGameObjects(IEnumerable<CollisionBody2D> gameBodies)
    {
        foreach (var body in gameBodies)
        {
            if (_physicsResultsBuffer.TryRemove(body.UID, out var result)) // TryRemove para consumir o dado
            {
                body.Transform.Position = result.Position.ToPixels().ToVector3();
                body.Transform.Rotation = new (body.Transform.Rotation.X, body.Transform.Rotation.Y, result.Angle);
            }
        }
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;

        StopPhysicsLoopThread();

        base.Dispose(disposing);
    }
}