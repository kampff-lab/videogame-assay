#version 400
uniform sampler2D tex;
uniform vec2 texShift;
in vec2 texCoord;
out vec4 fragColor;

void main()
{
  vec4 texel = texture(tex, (texCoord + texShift) * 10);
  fragColor = texel;
}
