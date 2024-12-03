#version 330 core
layout(location = 0) in vec3 position;
layout(location = 1) in vec3 normal;
layout(location = 2) in vec2 texCoord;
layout(location = 3) in vec3 tangent;
layout(location = 4) in vec3 bitangent;

out vec3 FragPos;
out vec3 Normal;
out vec2 TexCoord;
out vec4 FragPosLightSpace;
out mat3 TBN;

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

    TBN = mat3(
        normalize(vec3(model * vec4(tangent,   0.0))), 
        normalize(vec3(model * vec4(bitangent, 0.0))), 
        normalize(vec3(model * vec4(normal,    0.0)))
    );
    
    gl_Position = projection * view * vec4(FragPos, 1.0);
}