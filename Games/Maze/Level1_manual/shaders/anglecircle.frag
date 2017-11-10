#version 400
uniform float angle;
uniform float epsilon = 0.1;
uniform float smoothness = 0.05;
uniform vec4 colour;
in vec2 texCoord;
out vec4 fragColour;

void main()
{
  vec2 circlepoint = texCoord * 2 - 1.0f;
  float dist = length(circlepoint);
  float scale = smoothstep(0f, smoothness, 1 - dist);
  fragColour = scale * colour;
  float circleangle = atan(circlepoint.y, circlepoint.x);
  if (abs(circleangle - angle) < epsilon)
  {
    fragColour.rgb = vec3(1.0,0.0, 0.0);
  }
}
