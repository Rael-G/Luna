using System.Numerics;
using Luna.Maths;

namespace Luna.Editor.ViewPort;

public class Player(PerspectiveCamera camera3D) : Node
{
    bool firstMouse = true;
    float yaw   = -90.0f;
    float pitch =  0.0f;
    float lastX =  Window.Size.X / 2;
    float lastY =  Window.Size.Y / 2;
    float fov   =  45.0f;
    Vector3 cameraFront;

    public override void Update()
    {
        Movement();
        camera3D.Target = Transform.GlobalPosition + cameraFront.Normalize();
    }

    public override void Input(InputEvent inputEvent)
    {
        if (inputEvent is MousePositionEvent mouseEvent)
            Mouse((float)mouseEvent.X, (float)mouseEvent.Y);

        if (inputEvent is MouseScrollEvent scrollEvent)
            Scroll((float)scrollEvent.Y);

        if (inputEvent is MouseButtonEvent buttonEvent)
        {
            if (buttonEvent.Button == MouseButton.Left && buttonEvent.Action == InputAction.Down)
                Window.CursorMode = CursorMode.Disabled;
        }

        if (inputEvent is KeyboardEvent keyEvent)
        {
            if (keyEvent.Key == Keys.Escape && keyEvent.Action == InputAction.Down)
                Window.CursorMode = CursorMode.Normal;
        }
    }

    public void Movement()
    {
        float cameraSpeed;
        var right = cameraFront.Cross(camera3D.Up).Normalize();

        if (Keyboard.KeyDown(Keys.ShiftLeft))
            cameraSpeed = 10f * Time.DeltaTime;
        else
            cameraSpeed = 5f * Time.DeltaTime;

        if (Keyboard.KeyDown(Keys.W))
            camera3D.Transform.Position += cameraSpeed * cameraFront;
        if (Keyboard.KeyDown(Keys.S))
            camera3D.Transform.Position += -cameraSpeed * cameraFront;
        if (Keyboard.KeyDown(Keys.A))
            camera3D.Transform.Position += -cameraSpeed * right;
        if (Keyboard.KeyDown(Keys.D))
            camera3D.Transform.Position += cameraSpeed * right;
        if (Keyboard.KeyDown(Keys.Space))
            camera3D.Transform.Position += cameraSpeed * camera3D.Up;
        if (Keyboard.KeyDown(Keys.ControlLeft))
            camera3D.Transform.Position += -cameraSpeed * camera3D.Up;
    }

    public void Mouse(float xpos, float ypos)
    {

        if (firstMouse)
        {
            lastX = xpos;
            lastY = ypos;
            firstMouse = false;
        }

        float xoffset = xpos - lastX;
        float yoffset = lastY - ypos;
        lastX = xpos;
        lastY = ypos;

        float sensitivity = 0.1f; 
        xoffset *= sensitivity;
        yoffset *= sensitivity;

        yaw += xoffset;
        pitch += yoffset;

        if (pitch > 89.0f)
            pitch = 89.0f;
        if (pitch < -89.0f)
            pitch = -89.0f;

        Vector3 front;
        front.X = (float)Math.Cos(yaw.ToRadians()) * (float)Math.Cos(pitch.ToRadians());
        front.Y = (float)Math.Sin(pitch.ToRadians());
        front.Z = (float)Math.Sin(yaw.ToRadians()) * (float)Math.Cos(pitch.ToRadians());
    
        cameraFront = Vector3.Normalize(front);
    }

    public void Scroll(float yoffset)
    {
        fov -= yoffset;
        if (fov < 1.0f)
            fov = 1.0f;
        if (fov > 45.0f)
            fov = 45.0f;

        camera3D.Fov = fov.ToRadians();
    }

}
