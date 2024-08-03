#version 330 core
out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoord;

struct Material {
    sampler2D diffuseMap;
    sampler2D specularMap;
    vec4 color;
    float shininess;
};

struct Light {
    vec3 position;
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Material material;
uniform vec3 viewPos;
uniform Light light;

void main()
{
    vec3 ambient = light.ambient * texture(material.diffuseMap, TexCoord).rgb * material.color.rgb;

    vec3 norm = normalize(Normal);
    vec3 lightDir = normalize(light.position - FragPos);
    float diff = max(dot(norm, lightDir), 0.0);
    vec3 diffuse = light.diffuse * diff * texture(material.diffuseMap, TexCoord).rgb * material.color.rgb;

    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    vec3 specular = light.specular * spec * texture(material.specularMap, TexCoord).rgb * material.color.rgb;

    vec3 result = ambient + diffuse + specular;
    FragColor = vec4(result, 1.0);
}