#version 400
uniform float smoothness = 0.05;
uniform vec4 colour;
in vec2 tex_coord;
out vec4 frag_colour;

void main()
{
  vec2 circlepoint = tex_coord * 2 - 1.0f;
  float dist = length(circlepoint);
  float scale = smoothstep(0f, smoothness, 1 - dist);
  frag_colour = scale * colour;
}
