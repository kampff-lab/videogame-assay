#version 400
uniform mat4 mvp = mat4(1);
uniform vec2 scale = vec2(1, 1);
uniform vec2 shift;
in vec2 vp;
in vec2 vt;
out vec2 texCoord;

void main()
{
  gl_Position = vec4(vp * scale + shift, 0.0, 1.0) * mvp;
  texCoord = vt;
}
