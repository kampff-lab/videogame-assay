#version 400
// scale factor to correct for box aspect ratio (121 : 97)
//uniform vec2 ratiofactor = vec2(0.802,1);
//uniform vec2 ratiofactor = vec2(1,1.25);
uniform vec2 ratiofactor = vec2(1,1.25);

// extended sprite vertex shader to allow arbitrary warping
uniform mat4 mvp = mat4(1);

uniform vec2 scale = vec2(1, 1);
uniform vec2 shift;
in vec2 vp;
in vec2 vt;
out vec2 texCoord;

void main()
{
  gl_Position = vec4(ratiofactor * (vp * scale + shift), 0.0, 1.0) * mvp;
  texCoord = vt;
}
