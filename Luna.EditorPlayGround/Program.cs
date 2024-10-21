using Luna;
using Luna.Audio;
using Luna.EditorPlayGround;
using Luna.OpenGL;

LunaOpenGL.AddServices();
LunaAudio.AddServices();
Host.CreateWindow();
var root = new Test();
Host.Run(root);
