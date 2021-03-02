#version 330 core
#define PI 3.1415926535897932384626433832795
out vec4 FragColor;  
uniform vec3 objectColor; 
uniform vec3 lightColor; 
uniform vec3 lightPos; 
uniform vec3 viewPos;
uniform float time;
in vec3 FragPos;
in vec3 Normal;
void main()
{
    float ambientStrength = 0.1;
    vec3 ambient = ambientStrength * lightColor;

    vec3 norm = normalize(Normal);
    vec3 p = cos(lightPos * time);
    vec3 lightDir = normalize(p.x - FragPos);
    
    float diff = max(dot(norm, lightDir),0.0);
    vec3 diffuse = diff * lightColor;

    float specularStrength = 0.5;
    vec3 viewDir = normalize(viewPos - FragPos);
    vec3 reflectDir = reflect(-lightDir, norm);
    float spec = pow(max(dot(viewDir, reflectDir),0.0),64);
    vec3 specular = specularStrength * spec * lightColor;    
    
    vec3 result = (ambient + diffuse + specular) * objectColor;
       
    FragColor = vec4(result, 1.0);
}