#version 400
uniform vec2 texshift;
uniform sampler2D tex;
in vec2 texCoord;
out vec4 fragColour;

void main()
{
  vec4 texel = texture(tex, (texCoord + texshift) * 10);
  fragColour = texel;
}
