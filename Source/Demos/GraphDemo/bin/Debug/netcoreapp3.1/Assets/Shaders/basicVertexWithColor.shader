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
vec4 getPosition(){
	float step = 2.0 / 100.0;
	float u = (aPosition.x + 0.5) * step - 1;
	float v = (aPosition.x + 0.5);
	float y = sin(3.14 * (u + u_time));
	return vec4(u,y,v,1.0);
}
void main()
{
	vec3 offset = offsets[gl_InstanceID];
	gl_Position = projection * view * model * vec4(aPosition + offset, 1.0);
	fColor = aColor;
}

