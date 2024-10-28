#version 330 core
layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;
layout(location = 2) in vec2 texCoord;

const int SHADOW_LENGTH = 10;

out vec3 FragPos;
out vec3 Normal;
out vec2 TexCoord;
out vec4 FragPosLightSpace[SHADOW_LENGTH];
flat out int ShadowMapsLength;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform mat4 lightSpaceMatrix[SHADOW_LENGTH];
uniform int shadowMapsLength;

void main()
{
    FragPos = vec3(model * vec4(position, 1.0));
    Normal = mat3(transpose(inverse(model))) * normal;
    TexCoord = texCoord;
    ShadowMapsLength = shadowMapsLength;
    for (int i = 0; i < shadowMapsLength; i++)
    {
        FragPosLightSpace[i] = lightSpaceMatrix[i] * vec4(FragPos, 1.0);
    }

    gl_Position = projection * view * vec4(FragPos, 1.0);
}