#version 400
uniform vec4 colour;
in vec2 tex_coord;
out vec4 frag_colour;

void main()
{
  vec2 circlepoint = tex_coord * 2 - 1.0f;
  float dist = length(circlepoint);
  float scale = dist < 1 ? 1 : 0;
  frag_colour = scale * colour;
}
