#version 400
uniform vec4 colour;
in vec2 tex_coord;
out vec4 frag_colour;

void main()
{
  float dx = 2 * tex_coord.x - 1.0;
  float dy = 2 * tex_coord.y - 1.0;
  float scale = (dx * dx + dy * dy) < 1 ? 1.0 : 0.0;
  frag_colour = scale * colour;
}
