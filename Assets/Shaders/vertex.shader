#version 330 core
layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;
in vec4 vColor;
out vec4 fColor;
out vec2 texCoord;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
void main()
{
    texCoord = aTexCoord;
    fColor = vColor;
    gl_Position = projection * view * model * vec4(aPosition, 1.0);    
}