#version 330 core
layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;
layout(location = 2) in vec2 texCoord;

out vec3 FragPos;
out vec3 Normal;
out vec2 TexCoord;
out vec4 FragPosLightSpace;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 lightSpaceMatrix;

void main()
{
    FragPos = vec3(model * vec4(position, 1.0));
    Normal = mat3(transpose(inverse(model))) * normal;
    TexCoord = texCoord;
    FragPosLightSpace = lightSpaceMatrix * vec4(FragPos, 1.0);
    
    gl_Position = projection * view * vec4(FragPos, 1.0);
}