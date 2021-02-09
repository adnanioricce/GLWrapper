#version 330 core
#define PI 3.14159274
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec4 aColor;
out vec4 outColor;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform float u_time;
vec3 getWave(vec3 pos){
	return vec3(0.0,0.0,0.0);
}
void main()
{
	gl_Position = projection * view * model * vec4(aPosition, 1.0);
	outColor = aColor;
}