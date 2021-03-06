#version 330 core
#define PI 3.1415926535897932384626433832795
struct Material {
    sampler2D diffuse;    
    sampler2D specular;
    
    float shininess;
};
struct Light {
    vec3 position;
    
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};
out vec4 FragColor;  
uniform Material material;
uniform Light light;
uniform vec3 objectColor; 
uniform vec3 lightColor; 
uniform vec3 lightPos; 
uniform vec3 viewPos;
uniform float time;
in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoords;
void main()
{    
    vec3 ambient = light.ambient * vec3(texture(material.diffuse, TexCoords));

    vec3 norm = normalize(Normal);    
    vec3 lightDir = normalize(light.position - FragPos);
    
    float diff = max(dot(norm, lightDir),0.0);
    vec3 diffuse = diff * light.diffuse * vec3(texture(material.diffuse, TexCoords));
    
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir),0.0),material.shininess);
    vec3 specular = vec3(texture(material.specular,TexCoords)) * light.specular * spec;
    
    vec3 result = (ambient + diffuse + specular);
    FragColor = vec4(result, 1.0);
}