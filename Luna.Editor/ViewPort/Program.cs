using Luna;
using Luna.Audio;
using Luna.Editor;
using Luna.OpenGL;

LunaOpenGL.AddServices();
LunaAudio.AddServices();
Host.CreateWindow();
var root = new Test();
Host.Run(root);
