#version 330 core

struct Material {
    sampler2D diffuse0;
    sampler2D specular0;
    vec4 color;
    float shininess;
};

struct DirLight {
    vec3 direction;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

struct PointLight {
    vec3 position;
    
    float constant;
    float linear;
    float quadratic;
	
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

struct SpotLight {
    vec3 position;
    vec3 direction;
    float cutOff;
    float outerCutOff;
  
    float constant;
    float linear;
    float quadratic;
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;       
};

vec4 Light(vec3 normal, vec3 viewPos, vec3 fragPos, vec2 texCoord);
vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir, vec2 texCoord, float shadow);
vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir, vec2 texCoord);
vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir, vec2 texCoord);
float CalcShadow(vec4 fragPosLightSpace, sampler2D shadowMap, vec3 lightDirection);

const int DIR_LIGHT_LENGTH = 1;
const int POINT_LIGHT_LENGTH = 10;
const int SPOT_LIGHT_LENGTH = 10;
const int SHADOW_LENGTH = 10;

out vec4 FragColor;

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoord;
in vec4 FragPosLightSpace[SHADOW_LENGTH];
flat in int ShadowMapsLength;

uniform int dirLightLength;
uniform int pointLightLength;
uniform int spotLightLength;

uniform Material material;
uniform vec3 viewPos;

uniform DirLight dirLights[DIR_LIGHT_LENGTH];
uniform PointLight pointLights[POINT_LIGHT_LENGTH];
uniform SpotLight spotLights[SPOT_LIGHT_LENGTH];
uniform sampler2D shadowMaps[SHADOW_LENGTH];

uniform bool isAffectedByLight;

void main()
{
    FragColor = Light(Normal, viewPos, FragPos, TexCoord);

    // float depth = texture(material.diffuse0, TexCoord).r;
    // FragColor = vec4(vec3(depth), 1.0);
}

vec4 Light(vec3 normal, vec3 viewPos, vec3 fragPos, vec2 texCoord)
{
    vec3 norm = normalize(normal);
    vec3 viewDir = normalize(viewPos - fragPos);

    if (!isAffectedByLight)
        return material.color * texture(material.diffuse0, texCoord);
    
    vec3 result = vec3(0, 0, 0);
    for (int i = 0; i < dirLightLength; i++)
    {
        float shadow = i < ShadowMapsLength? CalcShadow(FragPosLightSpace[i], shadowMaps[i], normalize(-dirLights[i].direction)) : 1;
        result += CalcDirLight(dirLights[i], norm, viewDir, texCoord, shadow);
    }

    for (int i = 0; i < pointLightLength; i++)
        result += CalcPointLight(pointLights[i], norm, fragPos, viewDir, texCoord);    
    
    for (int i = 0; i < spotLightLength; i++)
        result += CalcSpotLight(spotLights[i], norm, fragPos, viewDir, texCoord);
    
    return vec4(result, texture(material.diffuse0, texCoord).a);
}

vec3 CalcDirLight(DirLight light, vec3 normal, vec3 viewDir, vec2 texCoord, float shadow)
{
    vec3 lightDir = normalize(-light.direction);

    vec3 halfwayDir = normalize(lightDir + viewDir);
    
    float diff = max(dot(normal, lightDir), 0.0);

    float spec = pow(max(dot(normal, halfwayDir), 0.0), material.shininess);

    vec3 ambient = light.ambient * vec3(texture(material.diffuse0, texCoord)) * material.color.rgb;
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse0, texCoord)) * material.color.rgb;
    vec3 specular = light.specular * spec * vec3(texture(material.specular0, texCoord)) * material.color.rgb;
    return ambient + (1.0 - shadow) * (diffuse + specular);
}

vec3 CalcPointLight(PointLight light, vec3 normal, vec3 fragPos, vec3 viewDir, vec2 texCoord)
{
    vec3 lightDir = normalize(light.position - fragPos);

    vec3 halfwayDir = normalize(lightDir + viewDir);
    
    float diff = max(dot(normal, lightDir), 0.0);

    float spec = pow(max(dot(normal, halfwayDir), 0.0), material.shininess);

    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));    

    vec3 ambient = light.ambient * vec3(texture(material.diffuse0, texCoord)) * material.color.rgb;
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse0, texCoord)) * material.color.rgb;
    vec3 specular = light.specular * spec * vec3(texture(material.specular0, texCoord)) * material.color.rgb;
    ambient *= attenuation;
    diffuse *= attenuation;
    specular *= attenuation;
    return (ambient + diffuse + specular);
}

vec3 CalcSpotLight(SpotLight light, vec3 normal, vec3 fragPos, vec3 viewDir, vec2 texCoord)
{
    vec3 lightDir = normalize(light.position - fragPos);

    vec3 halfwayDir = normalize(lightDir + viewDir);

    float diff = max(dot(normal, lightDir), 0.0);

    float spec = pow(max(dot(normal, halfwayDir), 0.0), material.shininess);

    float distance = length(light.position - fragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + light.quadratic * (distance * distance));    

    float theta = dot(lightDir, normalize(-light.direction)); 
    float epsilon = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

    vec3 ambient = light.ambient * vec3(texture(material.diffuse0, texCoord)) * material.color.rgb;
    vec3 diffuse = light.diffuse * diff * vec3(texture(material.diffuse0, texCoord)) * material.color.rgb;
    vec3 specular = light.specular * spec * vec3(texture(material.specular0, texCoord)) * material.color.rgb;
    ambient *= attenuation * intensity;
    diffuse *= attenuation * intensity;
    specular *= attenuation * intensity;
    return (ambient + diffuse + specular);
}

float CalcShadow(vec4 fragPosLightSpace, sampler2D shadowMap, vec3 lightDirection)
{
    vec3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
    projCoords = projCoords * 0.5 + 0.5;

    if(projCoords.z > 1.0)
        return 0.0;

    float currentDepth = projCoords.z;
    float bias = max(0.05 * (1.0 - dot(normalize(Normal), lightDirection)), 0.005);
    float shadow = 0.0;
    vec2 texelSize = 1.0 / textureSize(shadowMap, 0);

    for(int x = -1; x <= 1; ++x)
    {
        for(int y = -1; y <= 1; ++y)
        {
            float pcfDepth = texture(shadowMap, projCoords.xy + vec2(x, y) * texelSize).r; 
            shadow += currentDepth - bias > pcfDepth  ? 1.0 : 0.0;        
        }    
    }
    
    return shadow /= 9.0;
}

