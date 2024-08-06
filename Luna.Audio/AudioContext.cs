using Silk.NET.OpenAL;

namespace Luna.Audio;

public unsafe class AudioContext : Disposable
{
    public static AL Al { get => Instance._al; }

    private readonly AL _al;
    private readonly ALContext ALContext;
    private readonly Device* _device;
    private readonly Context* _context;

    private static AudioContext Instance
    {
        get => _instance ??= new(); 
    }

    private static AudioContext? _instance;

    private AudioContext()
    {
        ALContext = ALContext.GetApi();
        _device = ALContext.OpenDevice("");

        _context = ALContext.CreateContext(_device, null);
        ALContext.MakeContextCurrent(_context);

        _al = AL.GetApi();
         _al.GetError();
    }

    public override void Dispose(bool disposing)
    {
        if (_disposed) return;
        
        ALContext.DestroyContext(_context);
        ALContext.CloseDevice(_device);

        ALContext.Dispose();
        
        base.Dispose(disposing);
    }
}