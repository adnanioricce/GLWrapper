#version 330 core
in vec3 aPosition;
in vec4 aColor;
out vec4 fColor;
uniform vec3 offsets[100];
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
uniform float u_time;
uniform float u_PI;
void main()
{
	vec3 offset = offsets[gl_InstanceID];
	gl_Position = projection * view * model * vec4(aPosition + offset, 1.0);
	fColor = aColor;
}
