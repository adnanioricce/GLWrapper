#version 330 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform float time;
uniform vec3 lightPos;
out vec3 Normal;
out vec3 FragPos;
void main()
{
    gl_Position = projection * view * model * vec4(aPosition, 1.0);
    FragPos = vec3(model * vec4(aPosition,1.0)) * sin(lightPos * time);
    Normal = aNormal * mat3(transpose(inverse(model)));
}