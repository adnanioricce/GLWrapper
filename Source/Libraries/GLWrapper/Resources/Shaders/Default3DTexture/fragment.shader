#version 330

out vec4 outputColor;

in vec2 texCoord;
in vec4 fragColor;
uniform sampler2D texture1;
uniform sampler2D texture2;

void main()
{
    outputColor = mix(texture(texture1, texCoord), texture(texture2, texCoord), 0.2) * fragColor;
}